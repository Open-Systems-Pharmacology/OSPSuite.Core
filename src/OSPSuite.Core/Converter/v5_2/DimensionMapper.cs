using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IDimensionMapper
   {
      /// <summary>
      ///    Returns a list of dimensiosn that are only use for project converter
      /// </summary>
      IEnumerable<IDimension> DummyDimensionsForConversion { get; }

      /// <summary>
      ///    Retrieve the name of the new dimension based on the old dimension name. If includeDummy is set to true
      ///    dummy dimmensions are also included
      /// </summary>
      string DimensionNameFor(string oldDimensionName, bool includeDummy = false);

      double ConversionFactor(string dimensionName);
      double ConversionFactor(IDimension dimension);
      double ConversionFactor(IWithDimension dimension);

      /// <summary>
      /// Returns if any object with this dimension required a conversion.
      /// Rename is not a conversion
      /// </summary>
      bool NeededConversion(IDimension newDimension);
      bool NeededConversion(string newDimensionName);
   }

   internal class DimensionMapper : IDimensionMapper
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly ICache<string, Tuple<string, double>> _cache;
      private readonly List<IDimension> _dummyDimensionsForConversion;

      public DimensionMapper(IDimensionFactory dimensionFactory )
      {
         _dimensionFactory = dimensionFactory;
         _cache = new Cache<string, Tuple<string, double>>();
         _dummyDimensionsForConversion = new List<IDimension>();
         ConverterConstants.DummyDimensions.Each(addDummyDimension);
         intitializeMapping();   
      }

      private void addDummyDimension(KeyValuePair<string, string> dimensions)
      {
         var newDimension = _dimensionFactory.Dimension(dimensions.Value);
         var dummyDimension = new Dimension(new BaseDimensionRepresentation(), dimensions.Key,newDimension.BaseUnit.Name);
         foreach (var unit in newDimension.Units)
         {
            if(unit==newDimension.BaseUnit)
               continue;
            dummyDimension.AddUnit(unit.Name, unit.Factor, unit.Offset);
         }
         _dummyDimensionsForConversion.Add(dummyDimension);
      }

      private void intitializeMapping()
      {
         map("AbundancePerMassProtein", "Abundance per mass protein", 1E6);
         map("AbundancePerTissue", "Abundance per tissue", 1E-3);
         map("AgeInWeeks", "Age in weeks");
         map("AUC", "AUC (mass)", 1E-9);
         map("AUCMolar", "AUC (molar)");
         map("BMI", 1E-2);
         map("CellQuantityPerTissue", "Count per mass", 1E3);
         map("CellQuantityPerVolume", "Count per volume", 1E3);
         map("CLInVitroPerMassProtein", "CL per mass protein");
         map("Concentration", 1E-9); // Do not Rename here
         map("DiffusionCoefficient", 1E-2); // Do not Rename here
         map("DosePerBodyWeight", 1E-6);
         map("Energy_kcal", "Energy", 4184 * 10 * 10 * 60 * 60);
         map("Flow (volume)", "Flow");
         map("Flow (volume) per organ weight", "Flow per weight organ");
         map("Flow (volume) per weight", "Flow per weight");
         map("HydraulicConductivity", "Hydraulic conductivity", 1E-3 / (10 * 60 * 60));
         map("InputDose","Input dose", 1E-6);
         map("InversedLength", "Inversed length", 1E1);
         map("InversedMol", "Inversed mol", 1E-6);
         map("InversedTime", "Inversed time");
         map("InversedVolume", "Inversed volume");
         map("InVitroRecombinantEnzyme", "CL per recombinant enzyme");
         map("Length", 1E-1);
         map("Lipophilicity", "Log Units");
         map("MassAmount", 1E-6); // Do not Rename here
         map("MassProteinPerTissue", "Mass per tissue", 1E-3);
         map("MassProteinPerVolume", 1E-3);
         map("MolarConcentration", "Concentration (molar)");
         map("MolecularWeight", "Molecular weight", 1E-9);
         map("Nanolength", 1E-8);
         map("Resolution", 1.0 / 60);
         map("RT", 10 * 60 * 60 * 0.1 * 1E-6);
         map("SecondOrderRateConstant", "Second order rate constant");
         map("SpecificClearancePerProtein", "Second order rate constant");
         map("SpecificVmaxPerProtein", "Inversed time");
         map("Surface", 1E-2);
         map("Velocity", 1E-1);
         map("VmaxInVitroPerMassProtein", "Vmax per mass protein", 1E6);
         map("VmaxInVitroTransporter", "Vmax per transporter", 1E3);
         map("VmaxPerOrganTissueWeight", "Vmax per weight organ tissue");
         map("VmaxRecombinantEnzyme", "Vmax per recombinant enzyme", 1E3);
         map("VolumePerBodyWeight", "Volume per body weight");
      }

      /// <summary>
      ///    Add the dimension to the cache.
      /// </summary>
      /// <param name="oldDim">Old Dimension</param>
      /// <param name="newDim">New Dimension</param>
      /// <param name="factor">Conversion factor from old dimension to new dimension</param>
      private void map(string oldDim, string newDim, double factor = 1)
      {
         if(factor==0)
            throw new ArgumentException(string.Format("Factor for dimension {0} is 0", oldDim));

         var conversion = new Tuple<string, double>(newDim, factor);
         _cache.Add(oldDim, conversion);
         if (string.Equals(oldDim, newDim)) return;

         if (_cache.Contains(newDim))
         {
            double existingFactor = ConversionFactor(newDim);

            //same factor nothing to do
            if (existingFactor == factor) return;

            throw new ArgumentException(string.Format("Dimension {0} already exist with a different conversion factor {1} instead of {2}", newDim, existingFactor, factor));
         }
         _cache.Add(newDim, conversion);
      }

      /// <summary>
      ///    Dimension name is the same as before. Just base dimension has changed
      /// </summary>
      private void map(string dimension, double factor)
      {
         map(dimension, dimension, factor);
      }

      public IEnumerable<IDimension> DummyDimensionsForConversion
      {
         get { return _dummyDimensionsForConversion; }
      }

      public string DimensionNameFor(string oldDimensionName, bool includeDummy = false)
      {
         
         var testDimension = createTestDimensionName(oldDimensionName);
         
         if (_cache.Contains(testDimension))
            return createDimensionName(oldDimensionName,_cache[testDimension].Item1);

         if (includeDummy && ConverterConstants.DummyDimensions.ContainsKey(testDimension))
            return createDimensionName(oldDimensionName, ConverterConstants.DummyDimensions[testDimension]);

         return oldDimensionName;
      }

      private static string createDimensionName(string oldDimensionName, string newDimensionName)
      {
         return oldDimensionName.Contains(Constants.Dimension.RHS_DIMENSION_SUFFIX) ? string.Format("{0}{1}", newDimensionName, Constants.Dimension.RHS_DIMENSION_SUFFIX) : newDimensionName;
      }

      public double ConversionFactor(string dimensionName)
      {
         var testName = createTestDimensionName(dimensionName);
         if (!_cache.Contains(testName))
            return 1;

         return _cache[testName].Item2;
      }

      private static string createTestDimensionName(string dimensionName)
      {
         return dimensionName.Contains(Constants.Dimension.RHS_DIMENSION_SUFFIX)
                   ? dimensionName.Replace(Constants.Dimension.RHS_DIMENSION_SUFFIX, string.Empty).Trim()
                   : dimensionName;
      }

      public double ConversionFactor(IDimension dimension)
      {
         return ConversionFactor(dimension.Name);
      }

      public double ConversionFactor(IWithDimension dimension)
      {
         return ConversionFactor(dimension.Dimension);
      }

      public bool NeededConversion(IDimension newDimension)
      {
         return NeededConversion(newDimension.Name);
      }

      public bool NeededConversion(string newDimensionName)
      {
         var oldDimensionName = _cache.KeyValues.Where(x => string.Equals(x.Value.Item1, newDimensionName)).Select(x => x.Key).FirstOrDefault();
         if (string.IsNullOrEmpty(oldDimensionName))
            oldDimensionName = ConverterConstants.DummyDimensions.All().Where(x => string.Equals(x.Value, newDimensionName)).Select(x => x.Key).FirstOrDefault();

         if (string.IsNullOrEmpty(oldDimensionName))
            return false;

         return ConversionFactor(oldDimensionName) != 1;

      }
   }
}