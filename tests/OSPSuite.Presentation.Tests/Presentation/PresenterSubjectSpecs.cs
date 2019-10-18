using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_PresenterSubject : ContextSpecification<PresenterSubject>
   {
      protected IReactionBuildingBlock _buildingBlock;

      protected override void Context()
      {
         _buildingBlock = A.Fake<IReactionBuildingBlock>();
         sut = new PresenterSubject {Subject = _buildingBlock};
      }
   }

   public class When_checking_if_a_presenter_matches_another_presenter : concern_for_PresenterSubject
   {
      private ISubjectPresenter _presenterToMatch;
      private IReactionBuildingBlock _anotherBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _presenterToMatch = A.Fake<ISubjectPresenter>();
         _anotherBuildingBlock = A.Fake<IReactionBuildingBlock>();
      }

      [Observation]
      public void should_return_true_if_the_individuals_are_same()
      {
         A.CallTo(() => _presenterToMatch.Subject).Returns(_buildingBlock);
         sut.Matches(_presenterToMatch).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_individuals_have_different_name()
      {
         A.CallTo(() => _presenterToMatch.Subject).Returns(_anotherBuildingBlock);
         sut.Matches(_presenterToMatch).ShouldBeFalse();
      }

      [Observation]
      public void should_return_false_if_the_presenter_is_not_a_single_start_presenter()
      {
         sut.Matches(A.Fake<IPresenter>()).ShouldBeFalse();
      }
   }
}