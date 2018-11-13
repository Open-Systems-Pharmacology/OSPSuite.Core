using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationResidualHistogramPresenter : 
      IParameterIdentificationSingleRunAnalysisPresenter,
      ICanCopyToClipboard
   {
   }

   public class ParameterIdentificationResidualHistogramPresenter : AbstractPresenter<IParameterIdentificationSingleRunAnalysisView, IParameterIdentificationSingleRunAnalysisPresenter>, IParameterIdentificationResidualHistogramPresenter
   {
      private readonly IPresentationSettingsTask _presentationSettingsTask;
      private readonly IResidualDistibutionDataCreator _residualDistibutionDataCreator;
      private readonly INormalDistributionDataCreator _normalDistributionDataCreator;
      private readonly IApplicationSettings _applicationSettings;
      private ParameterIdentification _parameterIdentification;
      public ParameterIdentificationResidualHistogram Chart { get; private set; }
      private DefaultPresentationSettings _settings = new DefaultPresentationSettings();
      private readonly string _nameProperty;
      private readonly DistributionSettings _distributionSettings;
      private ParameterIdentificationRunResult _selectedRunResults;
      private readonly IParameterIdentificationResidualHistogramView _histogramView;
      public IReadOnlyList<ParameterIdentificationRunResult> AllRunResults { get; private set; }

      public string PresentationKey => PresenterConstants.PresenterKeys.ParameterIdentificationResidualHistogramPresenter;
      public ISimulationAnalysis Analysis => Chart;

      public ParameterIdentificationResidualHistogramPresenter(
         IParameterIdentificationSingleRunAnalysisView view, 
         IParameterIdentificationResidualHistogramView histogramView, 
         IPresentationSettingsTask presentationSettingsTask,
         IResidualDistibutionDataCreator residualDistibutionDataCreator, 
         INormalDistributionDataCreator normalDistributionDataCreator, 
         IApplicationSettings applicationSettings) : base(view)
      {
         _presentationSettingsTask = presentationSettingsTask;
         _histogramView = histogramView;
         _residualDistibutionDataCreator = residualDistibutionDataCreator;
         _normalDistributionDataCreator = normalDistributionDataCreator;
         _applicationSettings = applicationSettings;
         _nameProperty = ReflectionHelper.PropertyFor<ParameterIdentificationResidualHistogram, string>(x => x.Name).Name;
         view.ApplicationIcon = ApplicationIcons.ResidualHistogramAnalysis;
         _distributionSettings = new DistributionSettings
         {
            AxisCountMode = AxisCountMode.Count,
            BarType = BarType.SideBySide,
            XAxisTitle = Captions.ParameterIdentification.Residuals,
            YAxisTitle = Captions.ParameterIdentification.ResidualCount
         };
         _view.SetAnalysisView(histogramView);
         _histogramView.CopyToClipboardManager = this;
      }

      public void UpdateAnalysisBasedOn(IAnalysable analysable)
      {
         _parameterIdentification = analysable.DowncastTo<ParameterIdentification>();

         AllRunResults = _parameterIdentification.Results.OrderBy(x => x.TotalError).ToList();
         _view.CanSelectRunResults = AllRunResults.Count > 1;

         if (!AllRunResults.Any()) return;

         SelectedRunResults = AllRunResults.First();

         _view.BindToSelectedRunResult();

         updateHistogram();
      }

      private void updateHistogram()
      {
         var allResidualValues = SelectedRunResults.BestResult.AllResidualValues.ToFloatArray();
         var mean = allResidualValues.ArithmeticMean();
         var std = allResidualValues.ArithmeticStandardDeviation();

         var distributionData = _residualDistibutionDataCreator.CreateFor(SelectedRunResults.BestResult);
         var gaussData = _normalDistributionDataCreator.CreateNormalData(mean, std);

         _histogramView.BindTo(gaussData, distributionData, _distributionSettings);
      }

      public ParameterIdentificationRunResult SelectedRunResults
      {
         get => _selectedRunResults;
         set
         {
            _selectedRunResults = value;
            updateHistogram();
         }
      }

      public void LoadSettingsForSubject(IWithId subject)
      {
         _settings = _presentationSettingsTask.PresentationSettingsFor<DefaultPresentationSettings>(this, subject);
      }

      public void InitializeAnalysis(ISimulationAnalysis simulationAnalysis, IAnalysable analysable)
      {
         RemoveChartEventHandlers();

         Chart = simulationAnalysis.DowncastTo<ParameterIdentificationResidualHistogram>();

         AddChartEventHandlers();
         _view.Caption = Chart.Name;
         UpdateAnalysisBasedOn(analysable);
      }

      private void chartPropertyChanged(object sender, PropertyChangedEventArgs e)
      {
         if (e.PropertyName == _nameProperty)
            _view.Caption = Chart.Name;
      }

      protected virtual void AddChartEventHandlers()
      {
         if (Chart == null) return;
         Chart.PropertyChanged += chartPropertyChanged;
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

      public void CopyToClipboard()
      {
         _histogramView.CopyToClipboard(_applicationSettings.WatermarkTextToUse);
      }
   }
}