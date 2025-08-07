using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public static class ComparisonExtensions
   {
      public static IComparison<IFormula> FormulaComparison<TObject>(this IComparison<TObject> comparison) where TObject : class, IUsingFormula
      {
         return ChildComparison(comparison, x => x.Formula);
      }

      public static IComparison<TChild> ChildComparison<TObject, TChild>(this IComparison<TObject> comparison, Func<TObject, TChild> childRetriever) where TObject : class where TChild : class
      {
         return ChildComparison(comparison, childRetriever, comparison.Object1);
      }

      public static IComparison<TChild> ChildComparison<TObject, TChild>(this IComparison<TObject> comparison, Func<TObject, TChild> childRetriever, object commonAncestor)
         where TObject : class
         where TChild : class
      {
         return new Comparison<TChild>(
            childRetriever(comparison.Object1),
            childRetriever(comparison.Object2),
            comparison.Settings,
            comparison.Report,
            commonAncestor
            );
      }
   }
}