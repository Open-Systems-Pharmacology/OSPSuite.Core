using System;
using System.Collections.Generic;
using System.Linq;
using LumenWorks.Framework.IO.Csv;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.Dimension;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface ISimulationPKAnalysesImporter
   {
      IEnumerable<QuantityPKParameter> ImportPKParameters(string fileFullPath, IModelCoreSimulation modelCoreSimulation, IImportLogger logger);
   }

   public class SimulationPKAnalysesImporter : ISimulationPKAnalysesImporter
   {
      private readonly IDimensionFactory _dimensionFactory;
      private Cache<string, QuantityPKParameter> _importedPK;
      private Cache<QuantityPKParameter, List<(int individualId, float value)>> _valuesCache;
      private IImportLogger _logger;
      private const int INDIVIDUAL_ID = 0;
      private const int QUANTITY_PATH = 1;
      private const int PARAMETER_NAME = 2;
      private const int VALUE = 3;
      private const int UNIT = 4;
      private const int NUMBER_OF_COLUMNS = UNIT;

      public SimulationPKAnalysesImporter(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public IEnumerable<QuantityPKParameter> ImportPKParameters(string fileFullPath, IModelCoreSimulation modelCoreSimulation, IImportLogger logger)
      {
         try
         {
            _logger = logger;
            _importedPK = new Cache<string, QuantityPKParameter>(x => x.Id);
            //cache containing a list of tuple<individual Id, value in core unit>
            _valuesCache = new Cache<QuantityPKParameter, List<(int individualId, float value)>>();
            using (var reader = new CsvReaderDisposer(fileFullPath))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               validateFileFormat(headers);
               while (csv.ReadNextRecord())
               {
                  var pkParameter = retrieveOrCreatePKParameterFor(csv, modelCoreSimulation);
                  addValues(pkParameter, csv);
               }
            }

            //We might have created Quantity PK Parameters with merged dimension to satisfy conversion from one dimension to the other. 
            //We need to ensure that we have the expected core dimension
            foreach (var quantityPKParameter in _valuesCache.Keys)
            {
               quantityPKParameter.Dimension = coreDimensionFor(quantityPKParameter.Dimension);
            }

            //Update values for each pk parameters
            foreach (var keyValue in _valuesCache.KeyValues)
            {
               var pkParameter = keyValue.Key;
               var values = keyValue.Value;
               //0-based id
               var maxIndividualId = values.Select(x => x.individualId).Max();
               pkParameter.SetNumberOfIndividuals(maxIndividualId + 1);

               foreach (var (individualId, value) in values)
               {
                  pkParameter.SetValue(individualId, value);
               }
            }

            return _valuesCache.Keys.ToList();
         }
         catch (Exception e)
         {
            _logger.AddError(e.FullMessage());
            return Enumerable.Empty<QuantityPKParameter>();
         }
         finally
         {
            _importedPK.Clear();
            _valuesCache.Clear();
            _logger = null;
         }
      }

      private void addValues(QuantityPKParameter pkParameter, CsvReader csv)
      {
         if (!_valuesCache.Contains(pkParameter))
            _valuesCache.Add(pkParameter, new List<ValueTuple<int, float>>());

         var coreUnit = convertValueToCoreValue(pkParameter.Dimension, csv.DoubleAt(VALUE), csv[UNIT]);
         _valuesCache[pkParameter].Add((individualId: csv.IntAt(INDIVIDUAL_ID), coreUnit.ToFloat()));
      }

      private double convertValueToCoreValue(IDimension dimension, double valueInUnit, string unitName)
      {
         var unit = dimension.Unit(unitName);
         if (unit == null)
            throw new OSPSuiteException(Error.UnitIsNotDefinedInDimension(unitName, dimension.Name));

         return dimension.UnitValueToBaseUnitValue(unit, valueInUnit);
      }

      private QuantityPKParameter retrieveOrCreatePKParameterFor(CsvReader csv, IModelCoreSimulation modelCoreSimulation)
      {
         var parameterName = csv[PARAMETER_NAME];
         var quantityPath = csv[QUANTITY_PATH];
         var id = QuantityPKParameter.CreateId(quantityPath, parameterName);
         if (_importedPK.Contains(id))
            return _importedPK[id];


         var dimension = findDimensionFor(parameterName, csv[UNIT]);
         var pkParameter = new QuantityPKParameter {Name = parameterName, QuantityPath = quantityPath, Dimension = dimension};
         var quantityPKParameterContext = new QuantityPKParameterContext(pkParameter, modelCoreSimulation.MolWeightFor(quantityPath));
         pkParameter.Dimension = _dimensionFactory.MergedDimensionFor(quantityPKParameterContext);
         _importedPK.Add(pkParameter);

         return _importedPK[id];
      }

      private IDimension findDimensionFor(string parameterName, string unit)
      {
         if (string.IsNullOrEmpty(unit))
            return _dimensionFactory.NoDimension;


         var dimension = _dimensionFactory.DimensionForUnit(unit);

         if (dimension == null)
         {
            _logger.AddWarning(Error.CouldNotFindDimensionWithUnit(unit));
            return _dimensionFactory.CreateUserDefinedDimension(parameterName, unit);
         }

         // for a mass dimension, we will return the Molar dimension to ensure that we return the value of the PK-Parameter in the base supported unit
         switch (dimension.Name)
         {
            case MASS_AUC:
               return _dimensionFactory.Dimension(MOLAR_AUC);
            case MASS_AMOUNT:
               return _dimensionFactory.Dimension(MOLAR_AMOUNT);
            case MASS_CONCENTRATION:
               return _dimensionFactory.Dimension(MOLAR_CONCENTRATION);
            default:
               return dimension;
         }
      }

      private IDimension coreDimensionFor(IDimension dimension)
      {
         // Name might not me found for a user defined dimension
         return _dimensionFactory.Has(dimension.Name) ? _dimensionFactory.Dimension(dimension.Name) : dimension;
      }

      private void validateFileFormat(string[] headers)
      {
         var exception = new OSPSuiteException(Error.SimulationPKAnalysesFileDoesNotHaveTheExpectedFormat);
         if (headers.Length <= NUMBER_OF_COLUMNS)
            throw exception;

         //check if headers are actually real strings
         if (double.TryParse(headers[0], out _))
            throw exception;
      }
   }
}