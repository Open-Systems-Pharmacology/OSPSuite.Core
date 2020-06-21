namespace OSPSuite.Core.Domain.UnitSystem
{
   public class QuantityPKParameterMolarToMassConverter : MolarToMassDimensionConverter<QuantityPKParameterContext>
   {
      public QuantityPKParameterMolarToMassConverter(QuantityPKParameterContext context, IDimension molarDimension, IDimension massDimension) : base(
         molarDimension,
         massDimension, context, x => x.MolWeight)
      {
      }
   }

   public class QuantityPKParameterMassToMolarConverter : MassToMolarDimensionConverter<QuantityPKParameterContext>
   {
      public QuantityPKParameterMassToMolarConverter(QuantityPKParameterContext context, IDimension massDimension, IDimension molarDimension) : base(
         massDimension,
         molarDimension, context, x => x.MolWeight)
      {
      }
   }
}