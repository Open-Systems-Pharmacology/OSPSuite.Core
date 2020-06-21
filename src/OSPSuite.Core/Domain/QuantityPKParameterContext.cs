using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public class QuantityPKParameterContext: IWithDimension
   {
      public QuantityPKParameter QuantityPKParameter { get; }
      public double? MolWeight { get; }

      public QuantityPKParameterContext(QuantityPKParameter quantityPKParameter, double? molWeight)
      {
         QuantityPKParameter = quantityPKParameter;
         MolWeight = molWeight;
      }

      public IDimension Dimension
      {
         get  => QuantityPKParameter.Dimension;
         set => QuantityPKParameter.Dimension = value;
      }
   }
}