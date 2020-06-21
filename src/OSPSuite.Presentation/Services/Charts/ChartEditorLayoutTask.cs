using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Charts;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Services.Charts
{
   public interface IChartEditorLayoutTask
   {
      /// <summary>
      ///    Init the editor layout using the default layout defined for the current user settings.
      ///    If <paramref name="loadColumnSettings" /> is set to <c>true</c>,  column settings (such as width, visibility) will
      ///    be updated as well according to template. Default is <c>false</c>
      /// </summary>
      void InitEditorLayout(IChartEditorAndDisplayPresenter chartEditorPresenter, bool loadColumnSettings = false);

      /// <summary>
      ///    Init the editor layout using the given layout.
      ///    If <paramref name="loadColumnSettings" /> is set to <c>true</c>,  column settings (such as width, visibility) will
      ///    be updated as well according to template. Default is <c>false</c>
      /// </summary>
      void InitEditorLayout(IChartEditorAndDisplayPresenter chartEditorPresenter, ChartEditorLayoutTemplate chartEditorLayoutTemplate, bool loadColumnSettings = false);

      /// <summary>
      ///    Init the editor layout using the given <paramref name="serializedChartEditorLayout" />
      ///    If <paramref name="loadColumnSettings" /> is set to <c>true</c>,  column settings (such as width, visibility) will
      ///    be updated as well according to template. Default is <c>false</c>
      /// </summary>
      void InitEditorLayout(IChartEditorAndDisplayPresenter chartEditorPresenter, string serializedChartEditorLayout, bool loadColumnSettings = false);

      /// <summary>
      ///    Saves the current layout used by the presenter as file
      /// </summary>
      void SaveEditorLayoutToFile(IChartEditorAndDisplayPresenter chartEditorPresenter);

      /// <summary>
      ///    Returns the current layout used by the presenter as file
      /// </summary>
      string SaveEditorLayoutToString(IChartEditorAndDisplayPresenter chartEditorPresenter);

      /// <summary>
      ///    Returns all <see cref="ChartEditorLayoutTemplate" /> available in the application
      /// </summary>
      IEnumerable<ChartEditorLayoutTemplate> AllTemplates();

      /// <summary>
      ///    Returns the template with the given name or null if the template does not exist
      /// </summary>
      ChartEditorLayoutTemplate TemplateByName(string templateName);

      /// <summary>
      ///    Initializes the chart editor from <paramref name="chartEditorAndDisplayPresenter" />according to the user settings
      /// </summary>
      void InitFromUserSettings(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter);
   }

   public class ChartEditorLayoutTask : IChartEditorLayoutTask
   {
      private readonly IPresentationUserSettings _userSettings;
      private readonly IChartLayoutTemplateRepository _chartLayoutTemplateRepository;
      private readonly IDataPersistor _dataPersistor;
      private readonly IDialogCreator _dialogCreator;

      public ChartEditorLayoutTask(
         IPresentationUserSettings userSettings,
         IChartLayoutTemplateRepository chartLayoutTemplateRepository,
         IDataPersistor dataPersistor,
         IDialogCreator dialogCreator)
      {
         _userSettings = userSettings;
         _chartLayoutTemplateRepository = chartLayoutTemplateRepository;
         _dataPersistor = dataPersistor;
         _dialogCreator = dialogCreator;
      }

      public void InitEditorLayout(IChartEditorAndDisplayPresenter chartEditorPresenter, bool loadColumnSettings = false)
      {
         InitEditorLayout(chartEditorPresenter, TemplateByName(_userSettings.DefaultChartEditorLayout), loadColumnSettings);
      }

      public void InitEditorLayout(IChartEditorAndDisplayPresenter chartEditorPresenter, ChartEditorLayoutTemplate chartEditorLayoutTemplate, bool loadColumnSettings = false)
      {
         if (chartEditorLayoutTemplate != null)
         {
            copySettings(chartEditorPresenter, chartEditorLayoutTemplate.Settings, loadColumnSettings);
            _userSettings.DefaultChartEditorLayout = chartEditorLayoutTemplate.Name;
         }
         else
         {
            applyColumnSettings(chartEditorPresenter);
         }
      }

      private static void applyColumnSettings(IChartEditorAndDisplayPresenter chartEditorPresenter)
      {
         chartEditorPresenter.EditorPresenter.ApplyAllColumnSettings();
      }

      public void InitEditorLayout(IChartEditorAndDisplayPresenter chartEditorPresenter, string serializedChartEditorLayout, bool loadColumnSettings = false)
      {
         var settings = _dataPersistor.FromString<ChartEditorAndDisplaySettings>(serializedChartEditorLayout);
         copySettings(chartEditorPresenter, settings, loadColumnSettings);
      }

      private static void copySettings(IChartEditorAndDisplayPresenter chartEditorPresenter, ChartEditorAndDisplaySettings chartEditorAndDisplaySettings, bool loadColumnSettings)
      {
         chartEditorPresenter.CopySettingsFrom(chartEditorAndDisplaySettings, loadEditorLayout: true, loadColumnSettings: loadColumnSettings);
         applyColumnSettings(chartEditorPresenter);
      }

      public void SaveEditorLayoutToFile(IChartEditorAndDisplayPresenter chartEditorPresenter)
      {
         var fileName = _dialogCreator.AskForFileToSave(Captions.SaveChartLayoutToTemplateFile, Constants.Filter.CHART_LAYOUT_FILTER, Constants.DirectoryKey.MODEL_PART);
         if (string.IsNullOrEmpty(fileName)) return;

         var settings = chartEditorPresenter.CreateSettings();
         _dataPersistor.Save(settings, fileName);
      }

      public string SaveEditorLayoutToString(IChartEditorAndDisplayPresenter chartEditorPresenter)
      {
         var settings = chartEditorPresenter.CreateSettings();
         return _dataPersistor.ToString(settings);
      }

      public IEnumerable<ChartEditorLayoutTemplate> AllTemplates() => _chartLayoutTemplateRepository.All();

      public ChartEditorLayoutTemplate TemplateByName(string templateName) => AllTemplates().FindByName(templateName);

      private void loadEditorLayoutFromDefaultTemplate(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter)
      {
         InitEditorLayout(chartEditorAndDisplayPresenter, loadColumnSettings: true);
      }

      private void loadEditorLayoutFromUserSettings(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter)
      {
         InitEditorLayout(chartEditorAndDisplayPresenter, _userSettings.ChartEditorLayout, loadColumnSettings: true);
      }

      public void InitFromUserSettings(IChartEditorAndDisplayPresenter chartEditorAndDisplayPresenter)
      {
         if (!string.IsNullOrEmpty(_userSettings.ChartEditorLayout))
            loadEditorLayoutFromUserSettings(chartEditorAndDisplayPresenter);
         else
            loadEditorLayoutFromDefaultTemplate(chartEditorAndDisplayPresenter);
      }
   }
}