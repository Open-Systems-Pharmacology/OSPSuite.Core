using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CloseAllButMdiViewCommand : ContextSpecification<CloseAllButMdiViewCommand>
   {
      protected IApplicationController _applicationController;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         sut = new CloseAllButMdiViewCommand(_applicationController);
      }
   }

   public class When_executing_the_close_all_view_but_command : concern_for_CloseAllButMdiViewCommand
   {
      private IMdiChildView _mdiView;
      private List<ISingleStartPresenter> _openedPresenters;
      private ISingleStartPresenter _activePresenter;
      private ISingleStartPresenter _onePresenter;
      private ISingleStartPresenter _twoPresenter;

      protected override void Context()
      {
         base.Context();
         _mdiView = A.Fake<IMdiChildView>();
         _activePresenter = A.Fake<ISingleStartPresenter>();
         _onePresenter = A.Fake<ISingleStartPresenter>();
         _twoPresenter = A.Fake<ISingleStartPresenter>();
         _openedPresenters = new List<ISingleStartPresenter>();
         _openedPresenters.Add(_onePresenter);
         _openedPresenters.Add(_activePresenter);
         _openedPresenters.Add(_twoPresenter);

         A.CallTo(() => _mdiView.Presenter).Returns(_activePresenter);
         A.CallTo(() => _applicationController.OpenedPresenters()).Returns(_openedPresenters);
      }

      protected override void Because()
      {
         sut.For(_mdiView);
         sut.Execute();
      }

      [Observation]
      public void should_close_all_presenter_but_the_one_selected()
      {
         A.CallTo(() => _onePresenter.Close()).MustHaveHappened();
         A.CallTo(() => _twoPresenter.Close()).MustHaveHappened();
         A.CallTo(() => _activePresenter.Close()).MustNotHaveHappened();
      }
   }
}