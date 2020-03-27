namespace OSPSuite.Core.Domain.UnitSystem
{
   public abstract class QuantityPKParameterContextDimensionConverter : MolWeightDimensionConverter
   {
      protected readonly QuantityPKParameterContext _context;

      protected QuantityPKParameterContextDimensionConverter(QuantityPKParameterContext context, IDimension sourceDimension, IDimension targetDimension) : base(sourceDimension, targetDimension)
      {
         _context = context;
      }

      public override bool CanResolveParameters()
      {
         return _context.MolWeight.HasValue;
      }

      protected override double MolWeight => _context.MolWeight.GetValueOrDefault(double.NaN);
   }

   public class QuantityPKParameterMolarToMassConverter : QuantityPKParameterContextDimensionConverter
   {
      public QuantityPKParameterMolarToMassConverter(QuantityPKParameterContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)
      {
      }

      public override double ConvertToTargetBaseUnit(double molarConcentration) => ConvertToMass(molarConcentration);

      public override double ConvertToSourceBaseUnit(double massConcentration) => ConvertToMolar(massConcentration);
   }

   public class QuantityPKParameterMassToMolarConverter : QuantityPKParameterContextDimensionConverter
   {
      public QuantityPKParameterMassToMolarConverter(QuantityPKParameterContext context, IDimension sourceDimension, IDimension targetDimension) : base(context, sourceDimension, targetDimension)
      {
      }

      public override double ConvertToTargetBaseUnit(double massConcentration) => ConvertToMolar(massConcentration);

      public override double ConvertToSourceBaseUnit(double molarConcentration) => ConvertToMass(molarConcentration);
   }
}