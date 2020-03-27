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

   public class DoubleArrayMolarToMassConverter : DoubleArrayContextDimensionConverter
   {
      public DoubleArrayMolarToMassConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)
      {
      }

      public override double ConvertToTargetBaseUnit(double molarConcentration) => ConvertToMass(molarConcentration);

      public override double ConvertToSourceBaseUnit(double massConcentration) => ConvertToMolar(massConcentration);
   }

   public class DoubleArrayMassToMolarConverter : DoubleArrayContextDimensionConverter
   {
      public DoubleArrayMassToMolarConverter(DoubleArrayContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)
      {
      }

      public override double ConvertToTargetBaseUnit(double massConcentration) => ConvertToMolar(massConcentration);

      public override double ConvertToSourceBaseUnit(double molarConcentration) => ConvertToMass(molarConcentration);
   }
}