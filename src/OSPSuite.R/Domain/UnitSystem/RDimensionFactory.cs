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
            case QuantityPKParameterContext quantityPKParameterContext:
               return createQuantityPKParameterConverter(quantityPKParameterContext, sourceDimension, targetDimension);
            default:
               return null;
         }
      }

      private IDimensionConverter createDoubleArrayConverter(DoubleArrayContext doubleArrayContext, IDimension sourceDimension,
         IDimension targetDimension)
      {
         switch (sourceDimension.Name)
         {
            case Constants.Dimension.MOLAR_AMOUNT:
            case Constants.Dimension.MOLAR_CONCENTRATION:
            case Constants.Dimension.MOLAR_AUC:
               return new DoubleArrayMolarToMassConverter(doubleArrayContext, sourceDimension, targetDimension);
            case Constants.Dimension.MASS_CONCENTRATION:
            case Constants.Dimension.MASS_AMOUNT:
            case Constants.Dimension.MASS_AUC:
               return new DoubleArrayMassToMolarConverter(doubleArrayContext, sourceDimension, targetDimension);
         }

         return null;
      }

      private IDimensionConverter createQuantityPKParameterConverter(QuantityPKParameterContext quantityPKParameterContext, IDimension sourceDimension,
         IDimension targetDimension)
      {
         switch (sourceDimension.Name)
         {
            case Constants.Dimension.MOLAR_AMOUNT:
            case Constants.Dimension.MOLAR_CONCENTRATION:
            case Constants.Dimension.MOLAR_AUC:
               return new QuantityPKParameterMolarToMassConverter(quantityPKParameterContext, sourceDimension, targetDimension);
            case Constants.Dimension.MASS_CONCENTRATION:
            case Constants.Dimension.MASS_AMOUNT:
            case Constants.Dimension.MASS_AUC:
               return new QuantityPKParameterMassToMolarConverter(quantityPKParameterContext, sourceDimension, targetDimension);
         }

         return null;
      }
   }
}