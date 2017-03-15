using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Chart.SensitivityAnalyses;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SensitivityAnalysisPKParameterAnalysisPresenter : ContextSpecification<SensitivityAnalysisPKParameterAnalysisPresenter>
   {
      private IPresentationSettingsTask _presentationSettingsTask;
      protected ISensitivityAnalysisPKParameterAnalysisView _view;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected SensitivityAnalysisPKParameterAnalysis _sensitivityAnalysisPKParameterAnalysis;
      protected PKParameterSensitivity _pkParameterSensitivity;
      protected PKParameterSensitivity _pkParameterSensitivity2;
      protected PKParameterSensitivity _pkParameterSensitivity3;

      protected override void Context()
      {
         _sensitivityAnalysis = new SensitivityAnalysis();
         _sensitivityAnalysisPKParameterAnalysis = new SensitivityAnalysisPKParameterAnalysis();
         _sensitivityAnalysis.Results = new SensitivityAnalysisRunResult();
         _pkParameterSensitivity = new PKParameterSensitivity
         {
            PKParameterName = "pkParameterName",
            ParameterName = "parameterName",
            QuantityPath = "quantityPath",
            Value = 1.0
         };

         _pkParameterSensitivity2 = new PKParameterSensitivity
         {
            PKParameterName = "pkParameterName",
            ParameterName = "parameterName",
            QuantityPath = "quantityPath",
            Value = 0.0
         };

         _pkParameterSensitivity3 = new PKParameterSensitivity
         {
            PKParameterName = "pkParameterName",
            ParameterName = "parameterName",
            QuantityPath = "quantityPath",
            Value = double.NaN
         };

         _sensitivityAnalysis.Results.AddPKParameterSensitivity(_pkParameterSensitivity);
         _sensitivityAnalysis.Results.AddPKParameterSensitivity(_pkParameterSensitivity2);
         _view = A.Fake<ISensitivityAnalysisPKParameterAnalysisView>();
         _presentationSettingsTask = A.Fake<IPresentationSettingsTask>();

         var pkParameterRepository = A.Fake<IPKParameterRepository>();
         sut = new SensitivityAnalysisPKParameterAnalysisPresenter(_view, _presentationSettingsTask, pkParameterRepository);
      }
   }

   public class When_initializing_a_sensitivity_analysis_presenter : concern_for_SensitivityAnalysisPKParameterAnalysisPresenter
   {
      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysisPKParameterAnalysis.PKParameterName = _pkParameterSensitivity.PKParameterName;
         _sensitivityAnalysisPKParameterAnalysis.OutputPath = _pkParameterSensitivity.QuantityPath;
      }

      protected override void Because()
      {
         sut.InitializeAnalysis(_sensitivityAnalysisPKParameterAnalysis, _sensitivityAnalysis);
      }

      [Observation]
      public void the_activated_pk_selection_should_match_the_analysis()
      {
         sut.ActivePKParameter.ShouldBeEqualTo(_sensitivityAnalysisPKParameterAnalysis.PKParameterName);
      }

      [Observation]
      public void the_view_must_be_updated_with_the_selected_pk_selection()
      {
         A.CallTo(() => _view.UpdateChart(A<IReadOnlyList<PKParameterSensitivity>>.That.Matches(x => containsOnlyPkParameterSensitivity(x)), A<string>._)).MustHaveHappened();
      }

      private bool containsOnlyPkParameterSensitivity(IReadOnlyList<PKParameterSensitivity> x)
      {
         return x.Contains(_pkParameterSensitivity) && !x.Contains(_pkParameterSensitivity2) && !x.Contains(_pkParameterSensitivity3);
      }

      [Observation]
      public void should_bind_the_view_with_the_presenter()
      {
         A.CallTo(() => _view.BindTo(sut)).MustHaveHappened();
      }
   }
}
