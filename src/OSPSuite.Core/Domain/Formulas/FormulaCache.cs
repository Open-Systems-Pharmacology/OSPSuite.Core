using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Formulas
{
   public interface IFormulaCache : ICache<string, IFormula>
   {
      /// <summary>
      ///    return true if the cache contains a formula with the id equals to the id of the <paramref name="formula" />
      ///    given as parameter, otherwise false.
      /// </summary>
      bool Contains(IFormula formula);

      void Remove(IFormula formula);

      /// <summary>
      ///    Update the keys used in the formula cache
      /// </summary>
      void Refresh();
   }

   public class FormulaCache : Cache<string, IFormula>, IFormulaCache
   {
      public FormulaCache() : base(formula => formula.Id)
      {
      }

      public bool Contains(IFormula formula)
      {
         return formula != null && Contains(formula.Id);
      }

      public void Remove(IFormula formula)
      {
         if (formula == null) return;
         Remove(formula.Id);
      }

      public void Refresh()
      {
         var allFormulas = this.ToList();
         Clear();
         AddRange(allFormulas);
      }
   }

   public class BuildingBlockFormulaCache : FormulaCache
   {
      public override void Add(IFormula formula)
      {
         if (formula.IsCachable() && !isAlreadyCached(formula))
            base.Add(formula);
      }

      private bool isAlreadyCached(IFormula formula)
      {
         return Contains(formula.Id) && ReferenceEquals(formula, this[formula.Id]);
      }
   }
}