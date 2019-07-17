using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Classifications;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_NameClassificationPresenter : ContextSpecification<INameClassificationPresenter>
   {
      private IProjectRetriever _projectRetriever;
      private IObjectBaseView _view;
      protected IProject _project;
      protected ObjectBaseDTO _dto;

      protected override void Context()
      {
         _projectRetriever = A.Fake<IProjectRetriever>();
         _view = A.Fake<IObjectBaseView>();
         _project = A.Fake<IProject>();
         A.CallTo(() => _projectRetriever.CurrentProject).Returns(_project);
         sut = new NameClassificationPresenter(_view, _projectRetriever);

         A.CallTo(() => _view.BindTo(A<ObjectBaseDTO>._))
            .Invokes(x => _dto = x.GetArgument<ObjectBaseDTO>(0));
      }
   }

   public class When_retrieving_the_name_for_a_classification_stored_under_a_root_folder : concern_for_NameClassificationPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _project.AllClassificationsByType(ClassificationType.ObservedData))
            .Returns(new[] {new Classification {ClassificationType = ClassificationType.ObservedData, Name = "TOTO"}});

         sut.NewName(new RootNodeType("Observed Data", ApplicationIcons.ObservedDataFolder, ClassificationType.ObservedData));
      }

      [Observation]
      public void should_not_allow_the_usage_of_existing_classification_name_directly_under_the_same_root_folder()
      {
         _dto.Validate(x => x.Name, "TOTO").IsEmpty.ShouldBeFalse();
         _dto.Validate(x => x.Name, "NEW_NAME").IsEmpty.ShouldBeTrue();
      }
   }
}