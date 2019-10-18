using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_EditDisplayUnitsPresenter : ContextSpecification<IEditDisplayUnitsPresenter>
   {
      protected IEditDisplayUnitsView _view;
      private ICloneManagerForModel _cloner;
      protected DisplayUnitsManager _displayUnitManager;
      protected DisplayUnitsManager _clonedUnitManager;
      protected IDisplayUnitsPresenter _displayUnitPresenter;
      protected IDialogCreator _dialogCreator;
      protected ISerializationTask _serializationTask;

      protected override void Context()
      {
         _view = A.Fake<IEditDisplayUnitsView>();
         _cloner = A.Fake<ICloneManagerForModel>();
         _displayUnitPresenter = A.Fake<IDisplayUnitsPresenter>();
         sut = new EditDisplayUnitsPresenter(_view, _cloner, _displayUnitPresenter);

         sut.Initialize();
         _displayUnitManager = new DisplayUnitsManager();
         _clonedUnitManager = new DisplayUnitsManager();
         A.CallTo(() => _cloner.Clone(_displayUnitManager)).Returns(_clonedUnitManager);
      }
   }

   public class When_editing_the_default_units_and_the_user_cancels_the_action : concern_for_EditDisplayUnitsPresenter
   {
      private DisplayUnitMap _displayUnitMap;

      protected override void Context()
      {
         base.Context();
         _displayUnitMap = new DisplayUnitMap();
         _displayUnitManager.AddDisplayUnit(_displayUnitMap);
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.Edit(_displayUnitManager, Captions.User);
      }

      [Observation]
      public void should_not_change_the_units_manager_that_was_given_for_editing()
      {
         _displayUnitManager.AllDisplayUnits.ShouldContain(_displayUnitMap);
      }
   }

   public class When_editing_the_default_units : concern_for_EditDisplayUnitsPresenter
   {
      private DisplayUnitMap _displayUnitMap;

      protected override void Context()
      {
         base.Context();
         _displayUnitMap = new DisplayUnitMap();
         _displayUnitManager.AddDisplayUnit(_displayUnitMap);
         A.CallTo(() => _view.Canceled).Returns(true);
      }

      protected override void Because()
      {
         sut.Edit(_displayUnitManager, Captions.User);
      }

      [Observation]
      public void should_edit_a_clone_of_the_units_given()
      {
         A.CallTo(() => _displayUnitPresenter.Edit(_clonedUnitManager)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_units_to_be_edited()
      {
         A.CallTo(() => _view.AddUnitsView(_displayUnitPresenter.View)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_ok_button_depending_on_the_current_units_being_displayed()
      {
         A.CallTo(() => _view.HasError).Returns(false);

         A.CallTo(() => _displayUnitPresenter.CanClose).Returns(false);
         _displayUnitPresenter.StatusChanged += Raise.WithEmpty();
         _view.OkEnabled.ShouldBeFalse();

         A.CallTo(() => _displayUnitPresenter.CanClose).Returns(true);
         _displayUnitPresenter.StatusChanged += Raise.WithEmpty();
         _view.OkEnabled.ShouldBeTrue();
      }
   }
}