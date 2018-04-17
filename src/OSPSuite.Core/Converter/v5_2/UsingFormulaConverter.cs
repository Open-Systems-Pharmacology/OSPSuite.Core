using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Converter.v5_2
{
   public interface IUsingFormulaConverter
   {
      /// <summary>
      ///    Converts the specified using formula.
      /// </summary>
      /// <param name="usingFormula">The using formula.</param>
      /// <param name="convertFormulasAtUsingFormula">
      ///    if set to <c>true</c> [convert formulas at using formula]. Use
      ///    <see langword="true" /> for UsingFormulas in Simulation. Use
      ///    <see langword="false" /> in BuildingBlocks to prevent multiple
      ///    Conversion of identical formula. Formulas should be only converted in
      ///    <see cref="FormulaCache" />
      /// </param>
      void Convert(IUsingFormula usingFormula, bool convertFormulasAtUsingFormula);

      void Convert(IFormula formula);
      void Convert(IEnumerable<IFormula> formulas);
   }

   internal class UsingFormulaConverter : IUsingFormulaConverter
   {
      private readonly IDimensionMapper _dimensionMapper;
      private readonly IFormulaMapper _formulaMapper;
      private readonly IUsingDimensionConverter _usingDimensionConverter;

      public UsingFormulaConverter(IDimensionMapper dimensionMapper, IFormulaMapper formulaMapper, IUsingDimensionConverter usingDimensionConverter)
      {
         _dimensionMapper = dimensionMapper;
         _formulaMapper = formulaMapper;
         _usingDimensionConverter = usingDimensionConverter;
      }

      public void Convert(IUsingFormula usingFormula, bool convertFormulasAtUsingFormula)
      {
         if (usingFormula == null) return;
         // In Old Projects from PK-Sim some Formulas heave no Dimension set. in this case we have to copy the Dimension from the usingFormula Object  
         // We need do this before converting the Dimension, because the need for conversion the original Dimension
         if (shouldUpdateFormulaDimension(usingFormula))
            usingFormula.Formula.Dimension = usingFormula.Dimension;

         _usingDimensionConverter.Convert(usingFormula);

         if (usingFormula.Formula.IsConstant() || convertFormulasAtUsingFormula)
         {
            Convert(usingFormula.Formula);
         }
      }

      private static bool shouldUpdateFormulaDimension(IUsingFormula usingFormula)
      {
         return usingFormula.Formula != null && (usingFormula.Formula.Dimension.Equals(Constants.Dimension.NO_DIMENSION) && !usingFormula.Dimension.Equals(Constants.Dimension.NO_DIMENSION));
      }

      public void Convert(IFormula formula)
      {
         if (formula == null) return;

         //this has to be done before converting the formula
         var conversionFactor = _dimensionMapper.ConversionFactor(formula);

         _usingDimensionConverter.Convert(formula);

         if (formula.IsAnImplementationOf<DynamicFormula>())
            return;

         if (formula.IsAnImplementationOf<TableFormulaWithOffset>())
            return;

         if (formula.IsAnImplementationOf<TableFormulaWithXArgument>())
            return;

         foreach (var objectPath in formula.ObjectPaths)
         {
            _usingDimensionConverter.Convert(objectPath);
         }

         var constantFormula = formula as ConstantFormula;
         if (constantFormula != null)
         {
            constantFormula.Value *= conversionFactor;
            return;
         }

         var tableFormula = formula as TableFormula;
         if (tableFormula != null)
         {
            foreach (var point in tableFormula.AllPoints())
            {
               point.Y *= conversionFactor;
            }
         }

         var explicitFormula = formula as ExplicitFormula;
         if (explicitFormula == null)
            return;

         var newFormula = _formulaMapper.NewFormulaFor(explicitFormula.FormulaString);
         if (string.IsNullOrEmpty(newFormula))
            return;

         explicitFormula.FormulaString = newFormula;
      }

      public void Convert(IEnumerable<IFormula> formulas)
      {
         if (formulas == null) return;
         formulas.Each(Convert);
      }
   }
}