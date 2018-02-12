using System;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Comparison
{
   public class WithValueOriginComparison<T> where T : class, IWithValueOrigin
   {
      public void AddValueOriginToComparison(IComparison<T> comparison, DiffBuilder<T> diffBuilder, Action<IComparison<T>> comparisonAction)
      {
         var numberOfItemsInComparison = comparison.Report.Count;

         comparisonAction(comparison);

         if (!shouldAddValueOriginToComparison(numberOfItemsInComparison, comparison))
            return;

         diffBuilder.CompareValues(x => x.ValueOrigin, x => x.ValueOrigin, comparison);
      }

      private bool shouldAddValueOriginToComparison(int numberOfItemsInComparisonBeforeStartingComparison, IComparison<T> comparison)
      {
         //Value origin should always be shown as a difference if all properties should be displayed
         if (!comparison.Settings.OnlyComputingRelevant)
            return true;

         //no change in compared entities, we do not show value origin difference
         if (numberOfItemsInComparisonBeforeStartingComparison == comparison.Report.Count)
            return false;

         return comparison.Settings.ShowValueOrigin;
      }
   }
}