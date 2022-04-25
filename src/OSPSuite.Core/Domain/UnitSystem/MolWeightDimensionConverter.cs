using System;
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
      
      protected float ConvertToMass(float molar) => Convert.ToSingle(molar * MolWeight);

      public double ConvertToMolar(double mass) => mass / MolWeight;
      
      public float ConvertToMolar(float mass) => Convert.ToSingle(mass / MolWeight);

      protected abstract double MolWeight { get; }
   }

   public abstract class MolWeightDimensionConverter<TContext> : MolWeightDimensionConverter
   {
      private readonly TContext _context;
      private readonly Func<TContext, double?> _molWeightFunc;

      protected MolWeightDimensionConverter(IDimension sourceDimension, IDimension targetDimension, TContext context, Func<TContext, double?> molWeightFunc) : base(sourceDimension, targetDimension)
      {
         _context = context;
         _molWeightFunc = molWeightFunc;
      }


      public override bool CanResolveParameters() => _molWeightFunc(_context).HasValue;

      protected override double MolWeight => _molWeightFunc(_context).GetValueOrDefault(double.NaN);
   }

   public abstract class MolarToMassDimensionConverter<TContext> : MolWeightDimensionConverter<TContext>
   {
      protected MolarToMassDimensionConverter(IDimension molarDimension, IDimension massDimension, TContext context, Func<TContext, double?> molWeightFunc) : base(molarDimension, massDimension, context, molWeightFunc)
      {
      }

      public override double ConvertToTargetBaseUnit(double molarValue) => ConvertToMass(molarValue);

      public override double ConvertToSourceBaseUnit(double massValue) => ConvertToMolar(massValue);
   }

   public abstract class MassToMolarDimensionConverter<TContext> : MolWeightDimensionConverter<TContext>
   {
      protected MassToMolarDimensionConverter(IDimension massDimension, IDimension molarDimension, TContext context, Func<TContext, double?> molWeightFunc) : base(massDimension, molarDimension, context, molWeightFunc)
      {
      }

      public override double ConvertToTargetBaseUnit(double massValue) => ConvertToMolar(massValue);

      public override double ConvertToSourceBaseUnit(double molarValue) => ConvertToMass(molarValue);
   }
}