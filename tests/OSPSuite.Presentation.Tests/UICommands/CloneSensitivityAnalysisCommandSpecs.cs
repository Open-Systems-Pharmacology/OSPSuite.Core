using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_CloneSensitivityAnalysisCommand : ContextSpecification<CloneSensitivityAnalysisCommand>
   {
      protected ISensitivityAnalysisTask _sensitivityAnalysisTask;
      protected ISingleStartPresenterTask _singleStartPresenter;
      protected IApplicationController _applicationController;

      protected override void Context()
      {
         _sensitivityAnalysisTask = A.Fake<ISensitivityAnalysisTask>();
         _singleStartPresenter = A.Fake<ISingleStartPresenterTask>();
         _applicationController = A.Fake<IApplicationController>();

         sut = new CloneSensitivityAnalysisCommand(_sensitivityAnalysisTask,_singleStartPresenter,_applicationController);
      }
   }

   public class When_cloning_a_sensitivity_analysis : concern_for_CloneSensitivityAnalysisCommand
   {
      private SensitivityAnalysis _sensitivityAnalysis;
      private SensitivityAnalysis _cloneSensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis().WithId("Id1");
         _cloneSensitivityAnalysis = new SensitivityAnalysis().WithId("Id2");
         var clonePresenter = A.Fake<ICloneObjectBasePresenter<SensitivityAnalysis>>();
         A.CallTo(() => clonePresenter.CreateCloneFor(_sensitivityAnalysis)).Returns(_cloneSensitivityAnalysis);
         A.CallTo(() => _applicationController.Start<ICloneObjectBasePresenter<SensitivityAnalysis>>()).Returns(clonePresenter);
      }

      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_leverage_the_clone_object_presenter_to_clone_the_sensitivity_and_return_the_clone()
      {
         A.CallTo(() => _sensitivityAnalysisTask.AddToProject(_cloneSensitivityAnalysis));
      }
   }

}