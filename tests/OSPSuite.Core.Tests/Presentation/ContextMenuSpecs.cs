using System.Drawing;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ContextMenus;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ContextMenu : ContextSpecification<IContextMenu>
   {
      protected IContextMenuView _view;
      protected Point _position;
      protected IView _parentView;

      protected override void Context()
      {
         _view = A.Fake<IContextMenuView>();
         _parentView = A.Fake<IView>();
         _position = new Point();
         sut = new TestContextMenu(_view);
      }
   }

   public class When_showing_a_new_context_menu_for_a_type : concern_for_ContextMenu
   {
      protected override void Because()
      {
         sut.Show(_parentView, _position);
      }

      [Observation]
      public void should_display_the_view_at_the_given_position()
      {
         A.CallTo(() => _view.Display(_parentView, _position)).MustHaveHappened();
      }
   }

   public class TestContextMenu : ContextMenu
   {
      public TestContextMenu(IContextMenuView view)
         : base(view)
      {
      }
   }
}