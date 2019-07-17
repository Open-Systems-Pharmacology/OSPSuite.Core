using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CloseMdiViewCommand : ContextSpecification<CloseMdiViewCommand>
   {
      protected IApplicationController _applicationController;

      protected override void Context()
      {
         sut = new CloseMdiViewCommand();
      }
   }

   public class When_executing_the_close_view_command : concern_for_CloseMdiViewCommand
   {
      private IMdiChildView _mdiView;
      private ISingleStartPresenter _presenter;

      protected override void Context()
      {
         base.Context();
         _mdiView = A.Fake<IMdiChildView>();
         _presenter = A.Fake<ISingleStartPresenter>();
         A.CallTo(() => _mdiView.Presenter).Returns(_presenter);
      }

      protected override void Because()
      {
         sut.For(_mdiView);
         sut.Execute();
      }

      [Observation]
      public void should_close_the_view_that_was_selected()
      {
         A.CallTo(() => _presenter.Close()).MustHaveHappened();
      }
   }
}