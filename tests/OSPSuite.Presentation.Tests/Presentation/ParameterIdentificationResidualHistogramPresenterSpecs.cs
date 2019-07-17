using System.Data;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationResidualHistogramPresenter : ContextSpecification<IParameterIdentificationResidualHistogramPresenter>
   {
      protected IParameterIdentificationResidualHistogramView _histogramView;
      protected IPresentationSettingsTask _presentationSettingsTask;
      protected IResidualDistibutionDataCreator _residualDataCreator;
      protected INormalDistributionDataCreator _normalDistributionDataCreator;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationResidualHistogram _residualHistogram;
      private IParameterIdentificationSingleRunAnalysisView _view;
      private IApplicationSettings _applicationSettings;


      protected override void Context()
      {
         _view= A.Fake<IParameterIdentificationSingleRunAnalysisView>();
         _histogramView = A.Fake<IParameterIdentificationResidualHistogramView>();
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();
         _residualDataCreator = A.Fake<IResidualDistibutionDataCreator>();
         _normalDistributionDataCreator = A.Fake<INormalDistributionDataCreator>();
         _applicationSettings= A.Fake<IApplicationSettings>();

         sut = new ParameterIdentificationResidualHistogramPresenter(_view, _histogramView,_presentationSettingsTask, _residualDataCreator, _normalDistributionDataCreator, _applicationSettings);

         _parameterIdentification = new ParameterIdentification();
         _residualHistogram = new ParameterIdentificationResidualHistogram();

      }
   }

   public class When_displaying_the_results_of_a_given_parameter_identification_as_residual_histogram : concern_for_ParameterIdentificationResidualHistogramPresenter
   {
      private DataTable _gaussData;
      private ContinuousDistributionData _distributionData;
      private DistributionSettings _distributionSettings;
      private ParameterIdentificationRunResult _badResult;
      private ParameterIdentificationRunResult _goodResult;
      private double[] _residuals;

      protected override void Context()
      {
         base.Context();

         _gaussData = new DataTable();
         _distributionData = A.Fake<ContinuousDistributionData>();

         A.CallTo(_normalDistributionDataCreator).WithReturnType<DataTable>().Returns(_gaussData);

         A.CallTo(() => _histogramView.BindTo(_gaussData, _distributionData, A<DistributionSettings>._))
            .Invokes(x => _distributionSettings = x.GetArgument<DistributionSettings>(2));

         _badResult= A.Fake<ParameterIdentificationRunResult>();
         _goodResult = A.Fake<ParameterIdentificationRunResult>();

         A.CallTo(() => _badResult.TotalError).Returns(5);
         A.CallTo(() => _goodResult.TotalError).Returns(1);

         _parameterIdentification.AddResult(_badResult);
         _parameterIdentification.AddResult(_goodResult);

         A.CallTo(() => _residualDataCreator.CreateFor(_goodResult.BestResult)).Returns(_distributionData);

         _residuals = new [] {10d,20d,30d};
         A.CallTo(() => _goodResult.BestResult.AllResidualValues).Returns(_residuals);

      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_residualHistogram, _parameterIdentification);
      }

      [Observation]
      public void should_update_the_clibpard_manager_in_the_histogram_view()
      {
         _histogramView.CopyToClipboardManager.ShouldBeEqualTo(sut);
      }

      [Observation]
      public void should_create_the_residual_data_for_the_best_optimization_result()
      {
         A.CallTo(() => _residualDataCreator.CreateFor(_goodResult.BestResult)).MustHaveHappened();
      }

      [Observation]
      public void should_create_the_normal_distribution_data_used_to_display_the_gauss_function()
      {
         A.CallTo(() => _normalDistributionDataCreator.CreateNormalData(_residuals.ToFloatArray().ArithmeticMean(), _residuals.ToFloatArray().ArithmeticStandardDeviation(), null, null, 300)).MustHaveHappened();
      }

      [Observation]
      public void should_bind_those_data_to_the_view()
      {
         _distributionSettings.ShouldNotBeNull();
      }

      [Observation]
      public void should_set_the_chart_properties_as_execpted()
      {
         _distributionSettings.AxisCountMode.ShouldBeEqualTo(AxisCountMode.Count);
         _distributionSettings.XAxisTitle.ShouldBeEqualTo(Captions.ParameterIdentification.Residuals);
         _distributionSettings.YAxisTitle.ShouldBeEqualTo(Captions.ParameterIdentification.ResidualCount);
      }
   }
}