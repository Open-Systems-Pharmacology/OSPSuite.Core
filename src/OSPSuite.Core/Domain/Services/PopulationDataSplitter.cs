using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.SimModel;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public class PopulationDataSplitter
   {
      private readonly DataTable _populationData;
      private readonly DataTable _agingData;
      private readonly DataTable _initialValues;

      public PopulationDataSplitter(DataTable populationData, DataTable agingData = null, DataTable initialValues = null)
      {
         _populationData = populationData;
         _agingData = agingData ?? undefinedAgingData();
         _initialValues = initialValues ?? undefinedInitialValues();
      }

      private DataTable undefinedAgingData()
      {
         var table = new DataTable();
         table.AddColumn<int>(Constants.Population.INDIVIDUAL_ID_COLUMN);
         table.AddColumn<string>(Constants.Population.PARAMETER_PATH_COLUMN);
         return table;
      }

      private DataTable undefinedInitialValues()
      {
         var table = new DataTable();
         table.AddColumn<int>(Constants.Population.INDIVIDUAL_ID_COLUMN);
         return table;
      }

      public void UpdateParametersAndSpeciesValuesForIndividual(int individualId, IReadOnlyList<ParameterProperties> parameterProperties,
         IReadOnlyList<SpeciesProperties> speciesProperties, PathCache<IParameter> parameterCache)
      {
         fillParameterAndSpeciesInitialValuesFor(individualId, parameterProperties, speciesProperties, parameterCache);
      }

      public int GetIndividualIdsFor(int index)
      {
         return individualIdFrom(_populationData.Rows[index]);
      }

      public int NumberOfIndividuals => _populationData.Rows.Count;

      private static int individualIdFrom(DataRow popParametersDataRow)
      {
         return Convert.ToInt32(popParametersDataRow[Constants.Population.INDIVIDUAL_ID_COLUMN]);
      }

      private string parameterPathFrom(DataRow popParametersDataRow)
      {
         return popParametersDataRow[Constants.Population.PARAMETER_PATH_COLUMN].ToString();
      }

      private DataRow parameterDataRowFromIndividualId(int individualId)
      {
         return dataRowFromIndividualId(_populationData, individualId);
      }

      private DataRow initialValuesDataRowFromIndividualId(int individualId)
      {
         return dataRowFromIndividualId(_initialValues, individualId);
      }

      private DataRow dataRowFromIndividualId(DataTable dataTable, int individualId)
      {
         return dataTable.Rows.Cast<DataRow>().FirstOrDefault(r => individualIdFrom(r) == individualId);
      }

      private void fillParameterAndSpeciesInitialValuesFor(int individualId, IReadOnlyList<ParameterProperties> parameters,
         IReadOnlyList<SpeciesProperties> speciesProperties, PathCache<IParameter> parameterCache)
      {
         var nonTableParameterValues = parameterDataRowFromIndividualId(individualId);

         foreach (var parameter in parameters)
         {
            // first, assign all table points for the parameter from the aging data (if any)
            fillTableParameterValuesFor(parameter, individualId);

            // if no aging data found, parameter value is stored in the population data table
            // (single value)
            if (parameter.TablePoints.Any())
               continue;

            var parameterValue = nonTableParameterValues[parameter.Path].ConvertedTo<double>();
            //reset the parameter to its default value if the value in the table is NaN
            parameter.Value = double.IsNaN(parameterValue) ? parameterCache[parameter.Path].Value : parameterValue;
         }

         var initialValues = initialValuesDataRowFromIndividualId(individualId);
         if (initialValues == null)
            return;

         foreach (var species in speciesProperties)
         {
            species.InitialValue = initialValues[species.Path].ConvertedTo<double>();
         }
      }

      private void fillTableParameterValuesFor(ParameterProperties parameterProperties, int individualId)
      {
         var parameterPath = parameterProperties.Path;

         var tablePoints = from DataRow dr in _agingData.Rows
            where individualIdFrom(dr) == individualId
            where string.Equals(parameterPathFrom(dr), parameterPath)
            select tablePointFromAgingData(dr);

         parameterProperties.TablePoints = tablePoints.ToList();
      }

      private ValuePoint tablePointFromAgingData(DataRow dr)
      {
         var time = dr[Constants.Population.TIME_COLUMN].ConvertedTo<double>();
         var value = dr[Constants.Population.VALUE_COLUMN].ConvertedTo<double>();

         // for now always false for aging table
         return new ValuePoint(time, value, restartSolver: false);
      }

      /// <summary>
      ///    Get all parameter paths of parameters to be varied from PopulationData and AgingData.
      ///    Parameter paths DO NOT contain root element (e.g. "Organism|Age")
      /// </summary>
      public IReadOnlyList<string> ParameterPathsToBeVaried()
      {
         var nonTableParameterPathsToBeVaried = from DataColumn dc in _populationData.Columns
            where !dc.ColumnName.Equals(Constants.Population.INDIVIDUAL_ID_COLUMN)
            select dc.ColumnName;

         var tableParameterPathsToBeVaried = (from DataRow dr in _agingData.Rows
            select dr[Constants.Population.PARAMETER_PATH_COLUMN].ToString()).Distinct();

         return nonTableParameterPathsToBeVaried.Union(tableParameterPathsToBeVaried).ToList();
      }

      /// <summary>
      ///    Get all variable paths of molecules to be varied from InitialValues.
      /// </summary>
      public IReadOnlyList<string> InitialValuesPathsToBeVaried()
      {
         var speciesPathsToBeVaried = from DataColumn dc in _initialValues.Columns
            where !dc.ColumnName.Equals(Constants.Population.INDIVIDUAL_ID_COLUMN)
            select dc.ColumnName;

         return speciesPathsToBeVaried.ToList();
      }
   }
}