using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Comparison;
using OSPSuite.Presentation.Views.Comparisons;

namespace OSPSuite.Presentation.Presenters.Comparisons
{
   public interface IComparerSettingsPresenter : IPresenter<IComparerSettingsView>
   {
      void Edit(ComparerSettings comparerSettings);
      IEnumerable<FormulaComparison> FormulaComparisonValues { get; }
      string FormulaComparisonDisplayValueFor(FormulaComparison formulaComparison);

      /// <summary>
      ///    Ensure that the settings being edited are saved into the edited object
      /// </summary>
      void SaveChanges();
   }

   public class ComparerSettingsPresenter : AbstractPresenter<IComparerSettingsView, IComparerSettingsPresenter>, IComparerSettingsPresenter
   {
      public ComparerSettingsPresenter(IComparerSettingsView view): base(view)
      {
      }

      public void Edit(ComparerSettings comparerSettings)
      {
         _view.BindTo(comparerSettings);
      }

      public IEnumerable<FormulaComparison> FormulaComparisonValues => EnumHelper.AllValuesFor<FormulaComparison>();

      public string FormulaComparisonDisplayValueFor(FormulaComparison formulaComparison)
      {
         switch (formulaComparison)
         {
            case FormulaComparison.Value:
               return Captions.Comparisons.FormulaComparisonValue;
            case FormulaComparison.Formula:
               return Captions.Comparisons.FormulaComparisonFormula;
            default:
               throw new ArgumentOutOfRangeException(nameof(formulaComparison));
         }
      }

      public void SaveChanges()
      {
         _view.SaveChanges();
      }
   }
}