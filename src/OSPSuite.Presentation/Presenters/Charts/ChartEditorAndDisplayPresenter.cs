using System;
using OSPSuite.Assets;
using OSPSuite.Core;
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

      IMenuBarItem ChartLayoutButton { get; }
      Action PostEditorLayout { set; }
   }

   public class ChartEditorAndDisplayPresenter : AbstractCommandCollectorPresenter<IChartEditorAndDisplayView, IChartEditorAndDisplayPresenter>, IChartEditorAndDisplayPresenter
   {
      private readonly IChartEditorAndDisplayView _chartEditorAndDisplayView;
      private readonly IChartEditorLayoutTask _chartEditorLayoutTask;
      private readonly IStartOptions _startOptions;
      private readonly IPresentationUserSettings _presentationUserSettings;
      public Action PostEditorLayout { get; set; } = () => { };
      public IChartDisplayPresenter DisplayPresenter { get; }
      public IChartEditorPresenter EditorPresenter { get; }

      public ChartEditorAndDisplayPresenter(
         IChartEditorAndDisplayView chartEditorAndDisplayView, 
         IChartDisplayPresenter chartDisplayPresenter,
         IChartEditorPresenter chartEditorPresenter, 
         IChartEditorLayoutTask chartEditorLayoutTask, 
         IStartOptions startOptions,
         IPresentationUserSettings presentationUserSettings)
         : base(chartEditorAndDisplayView)
      {
         _chartEditorAndDisplayView = chartEditorAndDisplayView;
         DisplayPresenter = chartDisplayPresenter;
         EditorPresenter = chartEditorPresenter;
         _chartEditorLayoutTask = chartEditorLayoutTask;
         _startOptions = startOptions;
         _presentationUserSettings = presentationUserSettings;
         _chartEditorAndDisplayView.AddDisplay(DisplayPresenter.View);
         _chartEditorAndDisplayView.AddEditor(EditorPresenter.View);

         AddSubPresenters(EditorPresenter, chartDisplayPresenter);
      }

      public void CopySettingsFrom(ChartEditorAndDisplaySettings settings)
      {
         CopySettingsFrom(settings, true, true);
      }

      public void CopySettingsFrom(ChartEditorAndDisplaySettings settings, bool loadEditorLayout, bool loadColumnSettings)
      {
         if (settings == null) return;
         EditorPresenter.CopySettingsFrom(settings.EditorSettings, loadEditorLayout, loadColumnSettings);
         _chartEditorAndDisplayView.LoadLayoutFromString(settings.DockingLayout);
      }

      public ChartEditorAndDisplaySettings CreateSettings()
      {
         return new ChartEditorAndDisplaySettings
         {
            EditorSettings = EditorPresenter.CreateSettings(),
            DockingLayout = _chartEditorAndDisplayView.SaveLayoutToString()
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