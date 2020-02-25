using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.R.Domain.UnitSystem
{
   public abstract class DoubleArrayContextDimensionConverter : MolWeightDimensionConverter
   {
      protected readonly DoubleArrayContext _context;

      protected DoubleArrayContextDimensionConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(sourceDimension, targetDimension)
      {
         _context = context;
      }

      public override bool CanResolveParameters()
      {
         return _context.MolWeight.HasValue;
      }

      protected override double MolWeight => _context.MolWeight.GetValueOrDefault(double.NaN);
   }

   public class MolarToMassConcentrationDimensionConverter : DoubleArrayContextDimensionConverter
   {
      public MolarToMassConcentrationDimensionConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)
      {
      }

      public override double ConvertToTargetBaseUnit(double molarConcentration) => ConvertToMass(molarConcentration);

      public override double ConvertToSourceBaseUnit(double massConcentration) => ConvertToMolar(massConcentration);
   }

   public class MassToMolarConcentrationDimensionConverter : DoubleArrayContextDimensionConverter
   {
      public MassToMolarConcentrationDimensionConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)
      {
      }

      public override double ConvertToTargetBaseUnit(double massConcentration) => ConvertToMolar(massConcentration);

      public override double ConvertToSourceBaseUnit(double molarConcentration) => ConvertToMass(molarConcentration);
   }

   public class AmountToMassDimensionConverter : DoubleArrayContextDimensionConverter
   {
      public AmountToMassDimensionConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)

      {
      }

      public override double ConvertToTargetBaseUnit(double molarAmount) => ConvertToMass(molarAmount);

      public override double ConvertToSourceBaseUnit(double massAmount) => ConvertToMolar(massAmount);
   }

   public class MassToAmountDimensionConverter : DoubleArrayContextDimensionConverter
   {
      public MassToAmountDimensionConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)

      {
      }

      public override double ConvertToTargetBaseUnit(double massAmount) => ConvertToMolar(massAmount);

      public override double ConvertToSourceBaseUnit(double molarAmount) => ConvertToMass(molarAmount);
   }
}