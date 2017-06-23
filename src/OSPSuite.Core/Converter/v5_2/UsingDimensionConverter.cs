using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IUsingDimensionConverter
   {
      void Convert(IWithDimension dimension);
   }

   internal class UsingDimensionConverter : IUsingDimensionConverter
   {
      private readonly IDimensionFactory _dimensionFactory;

      public UsingDimensionConverter(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }

      public void Convert(IWithDimension withDimension)
      {
         if (withDimension == null) return;
         if (withDimension.Dimension == null)
            return;

         if (!ConverterConstants.DummyDimensions.ContainsKey(withDimension.Dimension.Name))
            return;

         Unit displayUnit = null;
         var formulaUsable = withDimension as IFormulaUsable;
         if (formulaUsable != null)
            displayUnit = formulaUsable.DisplayUnit;

         withDimension.Dimension = _dimensionFactory.Dimension(ConverterConstants.DummyDimensions[withDimension.Dimension.Name]);

         if (formulaUsable == null) return;
         formulaUsable.DisplayUnit = withDimension.Dimension.UnitOrDefault(displayUnit.Name);
      }
   }
}