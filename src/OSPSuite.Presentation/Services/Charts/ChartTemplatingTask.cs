using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.Mappers;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services.Charts
{
   public interface IChartTemplatingTask
   {
      /// <summary>
      ///    Starts the management interface for curve chart templates
      /// </summary>
      /// <param name="withChartTemplates">An object which contains simulation settings, which contains curve chart templates</param>
      /// <returns>Any commands used to manipulate the templates</returns>
      ICommand ManageTemplates(IWithChartTemplates withChartTemplates);

      /// <summary>
      ///    Returns a default template. The user will be asked to enter a unique name for the template
      /// </summary>
      CurveChartTemplate CloneTemplate(CurveChartTemplate templateToClone, IEnumerable<CurveChartTemplate> existingTemplates);

      /// <summary>
      ///    Saves the given <paramref name="template" /> to the file with path <paramref name="filePath" />
      /// </summary>
      void SaveTemplateToFile(CurveChartTemplate template, string filePath);

      /// <summary>
      ///    Returns the template supposedly serialized in the file with path <paramref name="filePath" />.
      ///    The user will be asked to enter a unique name for the template if the deserialized template name already exists.
      /// </summary>
      CurveChartTemplate LoadTemplateFromFile(string filePath, IReadOnlyList<CurveChartTemplate> existingTemplates);

      /// <summary>
      ///    Returns the serialized chart from <paramref name="chart" />
      /// </summary>
      string TemplateStringFrom(CurveChart chart);

      /// <summary>
      /// Initializes the given <paramref name="chart"/> from the <paramref name="template"/>.
      /// </summary>
      /// <param name="chart">Chart to initialize</param>
      /// <param name="dataColumns">Available columns that will be used to pattern match the curve to create automatically</param>
      /// <param name="template">Template to use</param>
      /// <param name="nameDefinition">Application specific name definition for columns</param>
      /// <param name="warnIfNumberOfCurvesAboveThreshold">If <c>true</c> a warning will be shown to the user if too many curves are created</param>
      /// <param name="propogateChartChangeEvent">If <c>true</c> a change event will be propagated when updating a chart from template</param>
      void InitializeChartFromTemplate(CurveChart chart, IEnumerable<DataColumn> dataColumns, CurveChartTemplate template, Func<DataColumn, string> nameDefinition, bool warnIfNumberOfCurvesAboveThreshold, bool propogateChartChangeEvent = true);

      /// <summary>
      ///    Returns a template based on the given <paramref name="chart" />. The template name will be set to the name of the
      ///    chart. If the flag <paramref name="validateTemplate" /> is set to <c>true</c> (default), an exception is thrown
      ///    if the <paramref name="chart" /> does not have any curve.
      /// </summary>
      CurveChartTemplate TemplateFrom(CurveChart chart, bool validateTemplate = true);

      /// <summary>
      ///    Returns the deserialized chart from <paramref name="serializedChart" />
      /// </summary>
      CurveChartTemplate TemplateFrom(string serializedChart);

      /// <summary>
      ///    Returns a template based on the given <paramref name="chart" />. The user will be asked to enter a unique name for
      ///    the template
      ///    based on the <paramref name="existingTemplates" />
      /// </summary>
      CurveChartTemplate CreateNewTemplateFromChart(CurveChart chart, IEnumerable<CurveChartTemplate> existingTemplates);

      ICommand AddChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates);

      /// <summary>
      ///    Updates the chart template named <paramref name="templateName" /> within the <paramref name="withChartTemplates" />.
      ///    The <paramref name="template" /> must be named <paramref name="templateName" /> and the original template will be
      ///    replaced by <paramref name="template" />
      /// </summary>
      /// <returns>The command that was run to update the chart template</returns>
      ICommand UpdateChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates, string templateName);

      /// <summary>
      ///    Adds simulation outputs for the <paramref name="simulations" /> to the chart.
      ///    if <paramref name="customUpdate" /> is not null, this action will be called after all outputs were added to the
      ///    simulation
      /// </summary>
      void UpdateDefaultSettings(IChartEditorPresenter chartEditorPresenter, IReadOnlyCollection<DataColumn> allAvailableColumns, IReadOnlyCollection<ISimulation> simulations, bool addCurveIfNoSourceDefined = true, Action customUpdate = null);
   }

   public abstract class ChartTemplatingTask : IChartTemplatingTask
   {
      private readonly IApplicationController _applicationController;
      private readonly IChartTemplatePersistor _chartTemplatePersistor;
      private readonly ICloneManager _cloneManager;
      private readonly ICurveChartToCurveChartTemplateMapper _chartTemplateMapper;
      private readonly IChartFromTemplateService _chartFromTemplateService;
      private readonly IChartUpdater _chartUpdater;
      private readonly IDialogCreator _dialogCreator;

      protected ChartTemplatingTask(IApplicationController applicationController, IChartTemplatePersistor chartTemplatePersistor, ICloneManager cloneManager,
         ICurveChartToCurveChartTemplateMapper chartTemplateMapper, IChartFromTemplateService chartFromTemplateService, IChartUpdater chartUpdater, IDialogCreator dialogCreator)
      {
         _applicationController = applicationController;
         _chartTemplatePersistor = chartTemplatePersistor;
         _cloneManager = cloneManager;
         _chartTemplateMapper = chartTemplateMapper;
         _chartFromTemplateService = chartFromTemplateService;
         _chartUpdater = chartUpdater;
         _dialogCreator = dialogCreator;
      }

      public void SaveTemplateToFile(CurveChartTemplate template, string filePath)
      {
         _chartTemplatePersistor.SerializeToFile(template, filePath);
      }

      public CurveChartTemplate CloneTemplate(CurveChartTemplate templateToClone, IEnumerable<CurveChartTemplate> existingTemplates)
      {
         return createTemplate(existingTemplates, () => _cloneManager.Clone(templateToClone), Captions.CloneTemplate);
      }

      private CurveChartTemplate createTemplate(IEnumerable<CurveChartTemplate> existingTemplates, Func<CurveChartTemplate> createAction, string caption, string defaultName = null)
      {
         var template = createAction();
         if (!template.Curves.Any())
            throw new OSPSuiteException(Error.TemplateShouldContainAtLeastOneCurve);

         var newName = retrieveNewNamesForTemplate(existingTemplates, caption, defaultName);
         if (string.IsNullOrEmpty(newName))
            return null;

         return createAction().WithName(newName);
      }

      public CurveChartTemplate LoadTemplateFromFile(string filePath, IReadOnlyList<CurveChartTemplate> existingTemplates)
      {
         var template = _chartTemplatePersistor.DeserializeFromFile(filePath);
         if (template == null)
            return null;

         if (existingTemplates.ExistsByName(template.Name))
         {
            var newName = retrieveNewNamesForTemplate(existingTemplates, Captions.RenameTemplate, template.Name);
            if (string.IsNullOrEmpty(newName))
               return null;

            template.Name = newName;
         }
         return template;
      }

      private string retrieveNewNamesForTemplate(IEnumerable<CurveChartTemplate> existingTemplates, string caption, string defaultName)
      {
         var usedNames = existingTemplates.Select(x => x.Name).ToList();
         return AskForInput(Captions.NewName, caption, defaultName, usedNames);
      }

      protected string AskForInput(string caption, string text, string defaultName, List<string> usedNames)
      {
         return _dialogCreator.AskForInput(caption, text, defaultName, usedNames);
      }

      protected abstract ICommand ReplaceTemplatesCommand(IWithChartTemplates withChartTemplates, IEnumerable<CurveChartTemplate> curveChartTemplates);

      public ICommand ManageTemplates(IWithChartTemplates withChartTemplates)
      {
         using (var presenter = _applicationController.Start<IModalChartTemplateManagerPresenter>())
         {
            presenter.EditTemplates(withChartTemplates.ChartTemplates);

            presenter.Display();

            if (!presenter.HasChanged || presenter.Canceled())
               return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();

            var curveChartTemplates = presenter.EditedTemplates;
            return ReplaceTemplatesCommand(withChartTemplates, curveChartTemplates);
         }
      }

      protected static void AddCurveForColumnWithOptionsFromSourceCurve(IChartEditorPresenter chartEditorPresenter, DataColumn column, Curve sourceCurve)
      {
         chartEditorPresenter.AddCurveForColumn(column, sourceCurve?.CurveOptions);
      }

      private void addSimulationOutputs(IChartEditorPresenter chartEditorPresenter, IReadOnlyCollection<DataColumn> allAvailableColumns, IReadOnlyCollection<ISimulation> simulations, bool addCurveIfNoSourceDefined)
      {
         var selectedColumns = simulations.SelectMany(x => x.OutputSelections).Take(Constants.MAX_NUMBER_OF_CURVES_TO_SHOW_AT_ONCE)
            .SelectMany(selection => allAvailableColumns.ColumnsForPath(selection.Path));

         selectedColumns.Each(column =>
         {
            var sourceCurve = CurvePlotting(simulations, column);
            if (addCurveIfNoSourceDefined || sourceCurve != null)
               AddCurveForColumnWithOptionsFromSourceCurve(chartEditorPresenter, column, sourceCurve);
         });
      }

      private static IEnumerable<Curve> allCurvesFromSimulations(IEnumerable<ISimulation> simulations)
      {
         return simulations.SelectMany(allCurvesForSimulation);
      }

      private static IEnumerable<Curve> allCurvesForSimulation(ISimulation simulation)
      {
         return simulation.Charts.SelectMany(x => x.Curves);
      }

      protected Curve CurvePlotting(ISimulation simulation, DataColumn dataColumn)
      {
         return allCurvesForSimulation(simulation).FirstOrDefault(x => x.PlotsColumn(dataColumn));
      }

      protected Curve CurvePlotting(IReadOnlyCollection<ISimulation> simulations, DataColumn dataColumn)
      {
         return allCurvesFromSimulations(simulations).FirstOrDefault(x => x.PlotsColumn(dataColumn));
      }

      public void UpdateDefaultSettings(IChartEditorPresenter chartEditorPresenter, IReadOnlyCollection<DataColumn> allAvailableColumns, IReadOnlyCollection<ISimulation> simulations, bool addCurveIfNoSourceDefined = true, Action customUpdate = null)
      {
         using (_chartUpdater.UpdateTransaction(chartEditorPresenter.Chart))
         {
            addSimulationOutputs(chartEditorPresenter, allAvailableColumns, simulations, addCurveIfNoSourceDefined);
            customUpdate?.Invoke();
         }
      }

      public CurveChartTemplate CreateNewTemplateFromChart(CurveChart chart, IEnumerable<CurveChartTemplate> existingTemplates)
      {
         return createTemplate(existingTemplates, () => TemplateFrom(chart), Captions.CreateNewTemplate, chart.Name);
      }

      public abstract ICommand AddChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates);

      public abstract ICommand UpdateChartTemplateCommand(CurveChartTemplate template, IWithChartTemplates withChartTemplates, string templateName);

      public string TemplateStringFrom(CurveChart chart) => _chartTemplatePersistor.SerializeAsStringBasedOn(chart);

      public CurveChartTemplate TemplateFrom(string serializedChart) => _chartTemplatePersistor.DeserializeFromString(serializedChart);

      public CurveChartTemplate TemplateFrom(CurveChart chart, bool validateTemplate = true)
      {
         var template = _chartTemplateMapper.MapFrom(chart);

         if (!template.Curves.Any() && validateTemplate)
            throw new OSPSuiteException(Error.TemplateShouldContainAtLeastOneCurve);

         return template;
      }

      public void InitializeChartFromTemplate(CurveChart chart, IEnumerable<DataColumn> dataColumns, CurveChartTemplate template, Func<DataColumn, string> nameDefinition, bool warnIfNumberOfCurvesAboveThreshold, bool propogateChartChangeEvent=true)
      {
         if (dataColumns == null || template == null)
            return;

         _chartFromTemplateService.InitializeChartFromTemplate(chart, dataColumns, template, nameDefinition, warnIfNumberOfCurvesAboveThreshold, propogateChartChangeEvent);
      }
   }
}