using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.R.Domain.UnitSystem
{
   public class RDimensionFactory : DimensionFactory
   {
      protected override IDimensionConverter CreateConverterFor<T>(IDimension sourceDimension, IDimension targetDimension, T hasDimension)
      {
         switch (hasDimension)
         {
            case DoubleArrayContext doubleArrayContext:
               return createDoubleArrayConverter(doubleArrayContext, sourceDimension, targetDimension);
            default:
               return null;
         }
      }

      private IDimensionConverter createDoubleArrayConverter(DoubleArrayContext doubleArrayContext, IDimension sourceDimension, IDimension targetDimension)
      {
         switch (sourceDimension.Name)
         {
            case Constants.Dimension.MOLAR_CONCENTRATION:
               return new MolarToMassConcentrationDimensionConverter(doubleArrayContext, sourceDimension, targetDimension);
            case Constants.Dimension.MASS_CONCENTRATION:
               return new MassToMolarConcentrationDimensionConverter(doubleArrayContext, sourceDimension, targetDimension);
            case Constants.Dimension.AMOUNT:
               return new AmountToMassDimensionConverter(doubleArrayContext, sourceDimension, targetDimension);
            case Constants.Dimension.MASS_AMOUNT:
               return new MassToAmountDimensionConverter(doubleArrayContext, sourceDimension, targetDimension);
         }

         return null;
      }
   }
}