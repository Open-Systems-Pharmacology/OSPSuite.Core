using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Starter
{
   internal class TestConverterter : IDimensionConverterFor
   {
      public bool CanResolveParameters()
      {
         return true;
      }

      public double ConvertToTargetBaseUnit(double sourceBaseUnitValue)
      {
         return sourceBaseUnitValue / 2;
      }

      public double ConvertToSourceBaseUnit(double targetBaseUnitValue)
      {
         return targetBaseUnitValue * 2;
      }

      public bool CanConvertTo(IDimension targetDimension)
      {
         return true;
      }

      public bool CanConvertFrom(IDimension sourceDimension)
      {
         return true;
      }

      public string UnableToResolveParametersMessage
      {
         get { return "You really messed something up"; }
      }
   }
}