using System;
using System.Windows.Forms;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Charts;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IChartEditorAndDisplayPresenter : ICommandCollectorPresenter
   {
      Control Control { get; }
      IChartDisplayPresenter DisplayPresenter { get; }
      IChartEditorPresenter EditorPresenter { get; }
      void CopySettingsFrom(ChartEditorAndDisplaySettings settings);

      /// <summary>
      ///    Initialize layout from the current settings. If <paramref name="loadEditorLayout" /> is set to true, editor layout
      ///    will be loaded.
      ///    if <paramref name="loadColumnSettings" /> is set to true, column settings will be loaded
      /// </summary>
      void CopySettingsFrom(ChartEditorAndDisplaySettings settings, bool loadEditorLayout, bool loadColumnSettings);

      ChartEditorAndDisplaySettings CreateSettings();

      void SetCurveNameDefinition(Func<DataColumn, string> curveNameDefinition);

      /// <summary>
      ///    If no curves have been added to the chart, then <paramref name="hint" /> text will appear in place of the empty
      ///    chart
      /// </summary>
      void SetNoCurvesSelectedHint(string hint);

      IMenuBarItem ChartLayoutButton { get; }
      Action PostEditorLayout { set; }
   }

   public class ChartEditorAndDisplayPresenter : AbstractCommandCollectorPresenter<IChartEditorAndDisplayControl, IChartEditorAndDisplayPresenter>, IChartEditorAndDisplayPresenter
   {
      private readonly IChartEditorAndDisplayControl _chartEditorAndDisplayControl;
      private readonly IChartEditorLayoutTask _chartEditorLayoutTask;
      private readonly IStartOptions _startOptions;
      private readonly IPresentationUserSettings _presentationUserSettings;
      public Action PostEditorLayout { get; set; } = () => { };

      public ChartEditorAndDisplayPresenter(IChartEditorAndDisplayControl chartEditorAndDisplayControl, IChartDisplayPresenter chartDisplayPresenter,
         IChartEditorPresenter chartEditorPresenter, IChartEditorLayoutTask chartEditorLayoutTask, IStartOptions startOptions,
         IPresentationUserSettings presentationUserSettings)
         : base(chartEditorAndDisplayControl)
      {
         _chartEditorAndDisplayControl = chartEditorAndDisplayControl;
         DisplayPresenter = chartDisplayPresenter;
         EditorPresenter = chartEditorPresenter;
         _chartEditorLayoutTask = chartEditorLayoutTask;
         _startOptions = startOptions;
         _presentationUserSettings = presentationUserSettings;
         _chartEditorAndDisplayControl.AddDisplay(DisplayPresenter.Control);
         _chartEditorAndDisplayControl.AddEditor(EditorPresenter.View);

         AddSubPresenters(EditorPresenter, chartDisplayPresenter);
      }

      public Control Control => _chartEditorAndDisplayControl as Control;

      public void SetCurveNameDefinition(Func<DataColumn, string> curveNameDefinition)
      {
         DisplayPresenter.SetCurveNameDefinition(curveNameDefinition);
         EditorPresenter.SetCurveNameDefinition(curveNameDefinition);
      }

      public void SetNoCurvesSelectedHint(string hint)
      {
         DisplayPresenter.SetNoCurvesSelectedHint(hint);
      }

      public IChartDisplayPresenter DisplayPresenter { get; }

      public IChartEditorPresenter EditorPresenter { get; }

      public void CopySettingsFrom(ChartEditorAndDisplaySettings settings)
      {
         CopySettingsFrom(settings, true, true);
      }

      public void CopySettingsFrom(ChartEditorAndDisplaySettings settings, bool loadEditorLayout, bool loadColumnSettings)
      {
         if (settings == null) return;
         EditorPresenter.CopySettingsFrom(settings.EditorSettings, loadEditorLayout, loadColumnSettings);
         _chartEditorAndDisplayControl.LoadLayoutFromString(settings.DockingLayout);
      }

      public ChartEditorAndDisplaySettings CreateSettings()
      {
         return new ChartEditorAndDisplaySettings
         {
            EditorSettings = EditorPresenter.CreateSettings(),
            DockingLayout = _chartEditorAndDisplayControl.SaveLayoutToString()
         };
      }

      private void editorLayoutSelected(ChartEditorLayoutTemplate template)
      {
         _chartEditorLayoutTask.InitEditorLayout(this, template, loadColumnSettings: true);
         PostEditorLayout();
      }

      private void saveCurrentChartEditorLayoutToUserSettings()
      {
         _presentationUserSettings.ChartEditorLayout = _chartEditorLayoutTask.SaveEditorLayoutToString(this);
      }

      public IMenuBarItem ChartLayoutButton
      {
         get
         {
            var chartLayoutButton = CreateSubMenu.WithCaption(MenuNames.Layout);
            _chartEditorLayoutTask.AllTemplates().Each(t =>
               chartLayoutButton.AddItem(CreateMenuButton.WithCaption(t.Name)
                  .WithActionCommand(() => editorLayoutSelected(t))));

            chartLayoutButton.AddItem(CreateMenuButton.WithCaption(MenuNames.SaveToUserSettings)
               .WithActionCommand(saveCurrentChartEditorLayoutToUserSettings)
               .AsGroupStarter());

            if (_startOptions.IsDeveloperMode)
            {
               chartLayoutButton.AddItem(CreateMenuButton.WithCaption(MenuNames.CustomizeEditorLayout)
                  .WithActionCommand(EditorPresenter.ShowCustomizationForm)
                  .AsGroupStarter()
                  .ForDeveloper());

               chartLayoutButton.AddItem(CreateMenuButton.WithCaption(MenuNames.SaveChartLayoutToFile)
                  .WithActionCommand(() => _chartEditorLayoutTask.SaveEditorLayoutToFile(this))
                  .ForDeveloper());
            }

            return chartLayoutButton;
         }
      }
   }
}