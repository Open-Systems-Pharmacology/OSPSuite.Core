namespace OSPSuite.Core.Domain.UnitSystem
{
   public interface IDimensionConverter
   {
      bool CanResolveParameters();
      double ConvertToTargetBaseUnit(double sourceBaseUnitValue);
      double ConvertToSourceBaseUnit(double targetBaseUnitValue);
      bool CanConvertTo(IDimension targetDimension);
      bool CanConvertFrom(IDimension sourceDimension);
      string UnableToResolveParametersMessage { get; }
   }
}