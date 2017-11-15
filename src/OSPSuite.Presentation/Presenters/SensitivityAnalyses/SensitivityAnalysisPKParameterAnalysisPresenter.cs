using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Chart.SensitivityAnalyses;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisPKParameterAnalysisPresenter : 
      ICanCopyToClipboard,
      IPresenter<ISensitivityAnalysisPKParameterAnalysisView>, 
      ISimulationAnalysisPresenter,
      IListener<SensitivityAnalysisResultsUpdatedEvent>
   {
      IReadOnlyList<string> AllPKParameters { get; }
      string ActivePKParameter { get; set; }
      string ActiveOutput { get; set; }
      IReadOnlyList<string> AllOutputPaths { get; }
      string DisplayValueForPKParameter(string pkParameterName);
   }

   public class SensitivityAnalysisPKParameterAnalysisPresenter : AbstractPresenter<ISensitivityAnalysisPKParameterAnalysisView, ISensitivityAnalysisPKParameterAnalysisPresenter>, ISensitivityAnalysisPKParameterAnalysisPresenter
   {
      private const double TOTAL_SENSITIVITY_THRESHOLD = 0.9;
      private readonly IPresentationSettingsTask _presentationSettingsTask;
      private DefaultPresentationSettings _settings;
      private SensitivityAnalysis _sensitivityAnalysis;
      private readonly string _nameProperty;
      private readonly IPKParameterRepository _pkParameterRepository;
      private readonly IApplicationSettings _applicationSettings;

      public SensitivityAnalysisPKParameterAnalysisPresenter(
         ISensitivityAnalysisPKParameterAnalysisView view, 
         IPresentationSettingsTask presentationSettingsTask, 
         IPKParameterRepository pkParameterRepository,
         IApplicationSettings applicationSettings) : base(view)
      {
         _presentationSettingsTask = presentationSettingsTask;
         _pkParameterRepository = pkParameterRepository;
         _applicationSettings = applicationSettings;
         _nameProperty = ReflectionHelper.PropertyFor<SensitivityAnalysisPKParameterAnalysis, string>(x => x.Name).Name;
      }

      public void LoadSettingsForSubject(IWithId subject)
      {
         _settings = _presentationSettingsTask.PresentationSettingsFor<DefaultPresentationSettings>(this, subject);
      }

      public string PresentationKey => PresenterConstants.PresenterKeys.SensitivityAnalysisPKParameterAnalysisPresenter;
      public ISimulationAnalysis Analysis => Chart;
      public IReadOnlyList<string> AllPKParameters => allPKParameterSensitivities.OrderBy(x => x.PKParameterName).Select(x => x.PKParameterName).Distinct().ToList();
      public IReadOnlyList<string> AllOutputPaths => allPKParameterSensitivities.OrderBy(x => x.QuantityPath).Select(x => x.QuantityPath).Distinct().ToList();
      private IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivities => _sensitivityAnalysis.Results.AllPKParameterSensitivities;

      public void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable)
      {
         Chart = simulationAnalysis.DowncastTo<SensitivityAnalysisPKParameterAnalysis>();
         setViewCaption();
         AddChartEventHandlers();
         UpdateAnalysisBasedOn(analysable);
         ActivePKParameter = Chart.PKParameterName ?? AllPKParameters.FirstOrDefault();
         ActiveOutput = Chart.OutputPath ?? AllOutputPaths.FirstOrDefault();
         _view.BindTo(this);
      }

      public SensitivityAnalysisPKParameterAnalysis Chart { get; set; }

      public void UpdateAnalysisBasedOn(IAnalysable analysable)
      {
         _sensitivityAnalysis = analysable.DowncastTo<SensitivityAnalysis>();
      }

      protected virtual void AddChartEventHandlers()
      {
         if (Chart == null) return;
         Chart.PropertyChanged += chartPropertyChanged;
      }

      private void chartPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == _nameProperty)
            setViewCaption();
      }

      private void setViewCaption()
      {
         _view.Caption = Chart.Name;
      }

      protected virtual void RemoveChartEventHandlers()
      {
         if (Chart == null) return;
         Chart.PropertyChanged -= chartPropertyChanged;
      }

      public void Clear()
      {
         RemoveChartEventHandlers();
      }

      public string DisplayValueForPKParameter(string pkParameterName)
      {
         return _pkParameterRepository.DisplayNameFor(pkParameterName);
      }

      public string ActivePKParameter
      {
         get => Chart.PKParameterName;
         set
         {
            Chart.PKParameterName = value;
            updateView();
         }
      }

      private void updateView()
      {
         _view.UpdateChart(allPKParameterSensitivitiesFor(Chart.PKParameterName, Chart.OutputPath), 
            Captions.SensitivityAnalysis.PkParameterOfOutput(DisplayValueForPKParameter(Chart.PKParameterName), Chart.OutputPath));
      }

      public string ActiveOutput
      {
         get => Chart.OutputPath;
         set
         {
            Chart.OutputPath = value;
            updateView();
         }
      }

      private IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivitiesFor(string pkParameterName, string outputPath)
      {
         return _sensitivityAnalysis.AllPKParameterSensitivitiesFor(pkParameterName, outputPath, TOTAL_SENSITIVITY_THRESHOLD);
      }

      public void Handle(SensitivityAnalysisResultsUpdatedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         updateView();
      }

      private bool canHandle(SensitivityAnalysisEvent eventToHandle)
      {
         return Equals(_sensitivityAnalysis, eventToHandle.SensitivityAnalysis);
      }

      public void CopyToClipboard()
      {
         _view.CopyToClipboard(_applicationSettings.WatermarkTextToUse);
      }
   }
}
