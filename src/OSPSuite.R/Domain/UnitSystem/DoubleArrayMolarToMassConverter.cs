using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.R.Domain.UnitSystem
{
   public class DoubleArrayMolarToMassConverter : MolarToMassDimensionConverter<DoubleArrayContext>
   {
      public DoubleArrayMolarToMassConverter(DoubleArrayContext context, IDimension molarDimension, IDimension massDimension) : base(molarDimension,
         massDimension, context, x => x.MolWeight)
      {
      }
   }

   public class DoubleArrayMassToMolarConverter : MassToMolarDimensionConverter<DoubleArrayContext>
   {
      public DoubleArrayMassToMolarConverter(DoubleArrayContext context, IDimension massDimension, IDimension molarDimension) : base(massDimension,
         molarDimension, context, x => x.MolWeight)
      {
      }
   }
}