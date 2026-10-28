using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   /// <summary>
   ///    Base comparison for formulas referencing a table formula and another object by alias. The referenced paths are
   ///    compared by alias, the aliases themselves by the derived class.
   /// </summary>
   public abstract class TableFormulaWithReferencesDiffBuilder<TFormula> : DiffBuilder<TFormula> where TFormula : Formula
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      protected TableFormulaWithReferencesDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<TFormula> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         CompareValues(x => x.Dimension, x => x.Dimension, comparison);
         CompareAliases(comparison);
         _enumerableComparer.CompareEnumerables(comparison, x => x.ObjectPaths, x => x.Alias);
      }

      protected abstract void CompareAliases(IComparison<TFormula> comparison);
   }

   public class TableFormulaWithOffsetDiffBuilder : TableFormulaWithReferencesDiffBuilder<TableFormulaWithOffset>
   {
      public TableFormulaWithOffsetDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }

      protected override void CompareAliases(IComparison<TableFormulaWithOffset> comparison)
      {
         CompareStringValues(x => x.TableObjectAlias, x => x.TableObjectAlias, comparison);
         CompareStringValues(x => x.OffsetObjectAlias, x => x.OffsetObjectAlias, comparison);
      }
   }

   public class TableFormulaWithXArgumentDiffBuilder : TableFormulaWithReferencesDiffBuilder<TableFormulaWithXArgument>
   {
      public TableFormulaWithXArgumentDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer) : base(objectBaseDiffBuilder, enumerableComparer)
      {
      }

      protected override void CompareAliases(IComparison<TableFormulaWithXArgument> comparison)
      {
         CompareStringValues(x => x.TableObjectAlias, x => x.TableObjectAlias, comparison);
         CompareStringValues(x => x.XArgumentAlias, x => x.XArgumentAlias, comparison);
      }
   }
}
