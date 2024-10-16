using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Formulas
{
   public static class FormulaExtensions
   {
      /// <summary>
      ///    Returns true the formula is a distributed otherwise false
      /// </summary>
      public static bool IsDistributed(this IFormula formula) => formula.IsAnImplementationOf<DistributionFormula>();

      /// <summary>
      ///    Returns true the formula is a black box formula otherwise false
      /// </summary>
      public static bool IsBlackBox(this IFormula formula) => formula.IsAnImplementationOf<BlackBoxFormula>();

      /// <summary>
      ///    Returns true the formula is a constant formula otherwise false
      /// </summary>
      public static bool IsConstant(this IFormula formula) => formula.IsAnImplementationOf<ConstantFormula>();

      /// <summary>
      ///    Returns true the formula is an explicit formula otherwise false
      /// </summary>
      public static bool IsExplicit(this IFormula formula) => formula.IsAnImplementationOf<ExplicitFormula>();

      
      /// <summary>
      ///    Returns true the formula is a dynamic formula otherwise false
      /// </summary>
      public static bool IsDynamic(this IFormula formula) => formula.IsAnImplementationOf<DynamicFormula>();

      /// <summary>
      ///    Returns true the formula is a table formula (without offset) otherwise false
      /// </summary>
      public static bool IsTable(this IFormula formula) => formula.IsAnImplementationOf<TableFormula>();

      /// <summary>
      ///    Returns true the formula is a table formula (with offset) otherwise false
      /// </summary>
      public static bool IsTableWithOffSet(this IFormula formula) => formula.IsAnImplementationOf<TableFormulaWithOffset>();

      /// <summary>
      ///    Returns true the formula is a table formula with argument otherwise false
      /// </summary>
      public static bool IsTableWithXArgument(this IFormula formula) => formula.IsAnImplementationOf<TableFormulaWithXArgument>();

      /// <summary>
      ///    Returns true if the formula can be added to a <see cref="FormulaCache" /> otherwise false
      /// </summary>
      public static bool IsCachable(this IFormula formula)
      {
         return formula.IsExplicit()
                || formula.IsBlackBox()
                || formula.IsDynamic()
                || formula.IsTable()
                || formula.IsTableWithOffSet()
                || formula.IsTableWithXArgument();
      }

      /// <summary>
      ///    Returns the used object path with the given <paramref name="alias" /> in the <paramref name="formula" /> or null if
      ///    the <paramref name="alias" /> is not used.
      /// </summary>
      public static FormulaUsablePath FormulaUsablePathBy(this IFormula formula, string alias)
      {
         return formula.ObjectPaths.FirstOrDefault(x => string.Equals(x.Alias, alias));
      }

      public static TFormula InitializedWith<TFormula>(this TFormula tableFormula, string xName, string yName, IDimension xDimension, IDimension yDimension) where TFormula : TableFormula
      {
         tableFormula.XName = xName;
         tableFormula.YName = yName;
         tableFormula.XDimension = xDimension;
         tableFormula.Dimension = yDimension;
         return tableFormula;
      }
   }
}