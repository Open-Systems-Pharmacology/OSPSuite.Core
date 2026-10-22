using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterAnalysablesInExplorerPresenter : ContextSpecification<IParameterAnalysablesInExplorerPresenter>
   {
      protected IParameterIdentificationTask _parameterIdentificationTask;
      protected ISensitivityAnalysisTask _sensitivityAnalysisTask;

      protected override void Context()
      {
         _parameterIdentificationTask = A.Fake<IParameterIdentificationTask>();
         _sensitivityAnalysisTask = A.Fake<ISensitivityAnalysisTask>();
         sut = new ParameterAnalysablesInExplorerPresenter(_parameterIdentificationTask, _sensitivityAnalysisTask, A.Fake<ITreeNodeFactory>());
      }

      protected ClassificationNode FolderFor(ClassificationType classificationType) => new ClassificationNode(new Classification {ClassificationType = classificationType});
   }

   public class When_removing_the_parameter_identifications_defined_under_multiple_folders : concern_for_ParameterAnalysablesInExplorerPresenter
   {
      private ClassificationNode _folder1;
      private ClassificationNode _folder2;
      private ParameterIdentification _parameterIdentification1;
      private ParameterIdentification _parameterIdentification2;
      private IReadOnlyList<ParameterIdentification> _deletedParameterIdentifications;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification1 = new ParameterIdentification().WithId("PI1");
         _parameterIdentification2 = new ParameterIdentification().WithId("PI2");
         _folder1 = FolderFor(ClassificationType.ParameterIdentification);
         _folder2 = FolderFor(ClassificationType.ParameterIdentification);
         _folder1.AddChild(new ParameterIdentificationNode(new ClassifiableParameterIdentification {Subject = _parameterIdentification1}));
         _folder2.AddChild(new ParameterIdentificationNode(new ClassifiableParameterIdentification {Subject = _parameterIdentification2}));

         A.CallTo(() => _parameterIdentificationTask.Delete(A<IReadOnlyList<ParameterIdentification>>._))
            .Invokes(x => _deletedParameterIdentifications = x.GetArgument<IReadOnlyList<ParameterIdentification>>(0))
            .Returns(true);
      }

      protected override void Because()
      {
         _result = sut.RemoveDataUnderClassifications(new[] {_folder1, _folder2});
      }

      [Observation]
      public void should_delete_the_parameter_identifications_of_all_folders_at_once()
      {
         A.CallTo(() => _parameterIdentificationTask.Delete(A<IReadOnlyList<ParameterIdentification>>._)).MustHaveHappenedOnceExactly();
         _deletedParameterIdentifications.ShouldOnlyContain(_parameterIdentification1, _parameterIdentification2);
      }

      [Observation]
      public void should_return_true_if_the_deletion_was_successful()
      {
         _result.ShouldBeTrue();
      }
   }

   public class When_removing_the_sensitivity_analyses_defined_under_multiple_folders : concern_for_ParameterAnalysablesInExplorerPresenter
   {
      private ClassificationNode _folder1;
      private ClassificationNode _folder2;
      private SensitivityAnalysis _sensitivityAnalysis1;
      private SensitivityAnalysis _sensitivityAnalysis2;
      private IReadOnlyList<SensitivityAnalysis> _deletedSensitivityAnalyses;
      private bool _result;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis1 = new SensitivityAnalysis().WithId("SA1");
         _sensitivityAnalysis2 = new SensitivityAnalysis().WithId("SA2");
         _folder1 = FolderFor(ClassificationType.SensitiviyAnalysis);
         _folder2 = FolderFor(ClassificationType.SensitiviyAnalysis);
         _folder1.AddChild(new SensitivityAnalysisNode(new ClassifiableSensitivityAnalysis {Subject = _sensitivityAnalysis1}));
         _folder2.AddChild(new SensitivityAnalysisNode(new ClassifiableSensitivityAnalysis {Subject = _sensitivityAnalysis2}));

         A.CallTo(() => _sensitivityAnalysisTask.Delete(A<IReadOnlyList<SensitivityAnalysis>>._))
            .Invokes(x => _deletedSensitivityAnalyses = x.GetArgument<IReadOnlyList<SensitivityAnalysis>>(0))
            .Returns(false);
      }

      protected override void Because()
      {
         _result = sut.RemoveDataUnderClassifications(new[] {_folder1, _folder2});
      }

      [Observation]
      public void should_delete_the_sensitivity_analyses_of_all_folders_at_once()
      {
         A.CallTo(() => _sensitivityAnalysisTask.Delete(A<IReadOnlyList<SensitivityAnalysis>>._)).MustHaveHappenedOnceExactly();
         _deletedSensitivityAnalyses.ShouldOnlyContain(_sensitivityAnalysis1, _sensitivityAnalysis2);
      }

      [Observation]
      public void should_return_false_if_the_deletion_was_canceled()
      {
         _result.ShouldBeFalse();
      }
   }
}
