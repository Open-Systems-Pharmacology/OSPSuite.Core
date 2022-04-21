using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Starter
{
   internal class TestConverter : IDimensionConverter
   {
      public bool CanResolveParameters()
      {
         return true;
      }

      public double ConvertToTargetBaseUnit(double sourceBaseUnitValue) => sourceBaseUnitValue / 2;

      public float ConvertToTargetBaseUnit(float sourceBaseUnitValue) => sourceBaseUnitValue / 2;

      public double ConvertToSourceBaseUnit(double targetBaseUnitValue) => targetBaseUnitValue * 2;

      public float ConvertToSourceBaseUnit(float targetBaseUnitValue) => targetBaseUnitValue * 2;

      public bool CanConvertTo(IDimension targetDimension)
      {
         return true;
      }

      public bool CanConvertFrom(IDimension sourceDimension)
      {
         return true;
      }

      public string UnableToResolveParametersMessage => "You really messed something up";
   }
}