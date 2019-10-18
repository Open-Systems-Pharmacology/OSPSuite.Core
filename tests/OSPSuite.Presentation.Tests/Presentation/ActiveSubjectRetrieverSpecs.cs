using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ActiveSubjectRetriever : ContextSpecification<IActiveSubjectRetriever>
   {
      protected IMainViewPresenter _mainViewPresenter;

      protected override void Context()
      {
         _mainViewPresenter = A.Fake<IMainViewPresenter>();
         sut = new ActiveSubjectRetriever(_mainViewPresenter);
      }
   }

   public class When_the_active_subject_retriever_is_retrieving_the_active_subject_selected_by_the_user : concern_for_ActiveSubjectRetriever
   {
      private ISimulation _result;
      private ISimulation _simulation;
      private ISingleStartPresenter _activeScreen;

      protected override void Context()
      {
         base.Context();
         _activeScreen = A.Fake<ISingleStartPresenter>();
         A.CallTo(() => _mainViewPresenter.ActivePresenter).Returns(_activeScreen);
         _simulation = A.Fake<ISimulation>();
         A.CallTo(() => _activeScreen.Subject).Returns(_simulation);
      }

      protected override void Because()
      {
         _result = sut.Active<ISimulation>();
      }

      [Observation]
      public void should_find_the_active_screen_and_return_the_underlying_subject()
      {
         _result.ShouldBeEqualTo(_simulation);
      }
   }

   public class When_the_active_subject_retriever_is_asked_to_retrieve_the_active_subject_and_no_presenter_was_activated : concern_for_ActiveSubjectRetriever
   {
      private ISimulation _result;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _mainViewPresenter.ActivePresenter).Returns(null);
      }

      protected override void Because()
      {
         _result = sut.Active<ISimulation>();
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_the_active_subject_retriever_is_asked_to_retrieve_the_active_subject_for_a_specific_type_that_is_not_the_type_of_the_active_subject : concern_for_ActiveSubjectRetriever
   {
      private IBuildingBlock _result;
      private ISimulation _simulation;
      private ISingleStartPresenter _activeScreen;

      protected override void Context()
      {
         base.Context();
         _activeScreen = A.Fake<ISingleStartPresenter>();
         A.CallTo(() => _mainViewPresenter.ActivePresenter).Returns(_activeScreen);
         _simulation = A.Fake<ISimulation>();
         A.CallTo(() => _activeScreen.Subject).Returns(_simulation);
      }

      protected override void Because()
      {
         _result = sut.Active<IBuildingBlock>();
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }
}