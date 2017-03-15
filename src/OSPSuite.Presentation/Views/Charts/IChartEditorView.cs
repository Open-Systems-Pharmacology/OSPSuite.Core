using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartEditorView : IView<IChartEditorPresenter>
   {
      void SetDataBrowserView(IView view);
      void SetCurveSettingsView(IView view);
      void SetAxisSettingsView(IView view);
      void SetChartSettingsView(IView view);
      void SetChartExportSettingsView(IView view);
      void ShowCustomizationForm();
      void LoadLayoutFromString(string dockingLayout);
      string SaveLayoutToString();
      void AddButton(IMenuBarItem menuBarItem);
      void ClearButtons();

      /// <summary>
      /// Adds the checkbox to the menubar
      /// </summary>
      void AddUsedInMenuItemCheckBox();

      /// <summary>
      /// Updates the value of the used in menubar checkbox
      /// </summary>
      /// <param name="checkedState">true if the checkbox should be checked, false if unchecked, null for indeterminate</param>
      void SetSelectAllCheckBox(bool? checkedState);
   }
}