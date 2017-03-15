using OSPSuite.Core.Comparison;
using OSPSuite.Presentation.Presenters.Comparisons;

namespace OSPSuite.Presentation.Views.Comparisons
{
   public interface IComparerSettingsView : IView<IComparerSettingsPresenter>
   {
      void BindTo(ComparerSettings comparerSettings);
      void SaveChanges();
   }
}