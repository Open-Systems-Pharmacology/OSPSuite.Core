using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Helpers
{
   public class DimensionFactoryForIntegrationTests : DimensionFactory
   {
      protected override IDimensionConverterFor CreateConverterFor<T>(IDimension dimension, IDimension dimensionToMerge, T hasDimension)
      {
         if(dimension.Name== Constants.Dimension.MOLAR_CONCENTRATION)
            return new ConcentrationMassToMolarConverter();

         return null;
      }
   }

   public class ConcentrationMassToMolarConverter : IDimensionConverterFor
   {
      public string UnableToResolveParametersMessage { get; } = "Cannot find dimension";

      public bool CanResolveParameters()
      {
         return true;
      }

      public double ConvertToTargetBaseUnit(double sourceBaseUnitValue)
      {
         return sourceBaseUnitValue * 10;
      }

      public double ConvertToSourceBaseUnit(double targetBaseUnitValue)
      {
         return targetBaseUnitValue / 10;
      }

      public bool CanConvertTo(IDimension targetDimension)
      {
         return targetDimension.Name == Constants.Dimension.MASS_CONCENTRATION;
      }

      public bool CanConvertFrom(IDimension sourceDimension)
      {
         return sourceDimension.Name == Constants.Dimension.MOLAR_CONCENTRATION;
      }

   }
}