using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.UnitSystem
{
   public abstract class MolWeightDimensionConverter : IDimensionConverter
   {
      private readonly IDimension _sourceDimension;
      private readonly IDimension _targetDimension;

      protected MolWeightDimensionConverter(IDimension sourceDimension, IDimension targetDimension)
      {
         _sourceDimension = sourceDimension;
         _targetDimension = targetDimension;
      }

      public abstract bool CanResolveParameters();

      public abstract double ConvertToTargetBaseUnit(double sourceBaseUnitValue);

      public abstract double ConvertToSourceBaseUnit(double targetBaseUnitValue);

      public virtual bool CanConvertTo(IDimension targetDimension) => _targetDimension == targetDimension;

      public virtual bool CanConvertFrom(IDimension sourceDimension) => _sourceDimension == sourceDimension;

      public virtual string UnableToResolveParametersMessage => Error.MolWeightNotAvailable;

      protected double ConvertToMass(double molar) => molar * MolWeight;

      public double ConvertToMolar(double mass) => mass / MolWeight;

      protected abstract double MolWeight { get; }
   }
}