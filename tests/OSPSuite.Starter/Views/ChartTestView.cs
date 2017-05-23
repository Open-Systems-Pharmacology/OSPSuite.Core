using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter.Views
{
   public partial class ChartTestView : BaseUserControl, IChartTestView
   {
      private IChartTestPresenter _presenter;

      public ChartTestView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         saveChartButton.Click += (o, e) => OnEvent(saveChart);
         loadChartButton.Click += (o, e) => OnEvent(loadChart);
         saveSettingsButton.Click += (o, e) => OnEvent(saveSettings);
         loadSettingsButton.Click += (o, e) => OnEvent(loadSettings);
         refreshDisplayButton.Click += (o, e) => OnEvent(refreshDisplay);
         addRepository1Button.Click += (o, e) => OnEvent(addRepsoitory1);
         addRepository2Button.Click += (o, e) => OnEvent(addRepsoitory2);
         reloadMenusButton.Click += (o, e) => OnEvent(reloadMenus);
         clearRepository2Button.Click += (o, e) => OnEvent(clearRepository2);
         addCurveToRepo2Button.Click += (o, e) => OnEvent(addCurveToRepository2);
         removeDatalessCurvesButton.Click += (o, e) => OnEvent(removeDatalessCurves);
      }

      private void removeDatalessCurves()
      {
         _presenter.RemoveDatalessCurves();
      }

      private void addCurveToRepository2()
      {
         _presenter.AddCurveToRepository2();
      }

      private void clearRepository2()
      {
         _presenter.ClearRepository2();
      }

      private void reloadMenus()
      {
         _presenter.ReloadMenus();
      }

      private void addRepsoitory1()
      {
         _presenter.AddRepsoitory1();
      }

      private void addRepsoitory2()
      {
         _presenter.AddRepository2();
      }

      private void refreshDisplay()
      {
         _presenter.RefreshDisplay();
      }

      private void saveSettings()
      {
         _presenter.SaveSettings();
      }

      private void loadSettings()
      {
         _presenter.LoadSettings();
      }

      private void loadChart()
      {
         _presenter.LoadChart();
      }

      private void saveChart()
      {
         _presenter.SaveChart();
      }

      public void AttachPresenter(IChartTestPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddChartEditorView(IChartEditorView editorPresenterView)
      {
         chartEditorPanel.FillWith(editorPresenterView);
      }

      public void AddChartDisplayView(IChartDisplayView displayPresenterView)
      {
         chartDisplayPanel.FillWith(displayPresenterView);
      }
   }
}
