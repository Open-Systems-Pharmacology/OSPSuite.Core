using OSPSuite.Presentation.Presenters.Comparisons;

namespace OSPSuite.Presentation.Views.Comparisons
{
   public interface IMainComparisonView : IView<IMainComparisonPresenter>
   {
      void AddSettingsView(IView view);
      void AddComparisonView(IView view);
      bool SettingsVisible { get; set; }
      void UpdateButtonsEnableState(bool enabled);
   }
}