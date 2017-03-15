using System.Drawing;
using System.Windows.Forms;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using NUnit.Framework;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI
{
   public class When_resizing_a_view_according_to_the_screen_size_forcing_a_resize : StaticContextSpecification
   {
      private Form _view;
      private Rectangle _boundaries;

      protected override void Context()
      {
         _view = new Form();
         _boundaries = new Rectangle {Width = 1000, Height = 1000};
      }

      protected override void Because()
      {
         FormExtensions.ReziseForCurrentScreen(_view, _boundaries, fractionWidth: 0.5, fractionHeight: 0.5, resizeOnlyIfOutOfBound: false);
      }

      [Observation]
      public void should_have_resized_the_view()
      {
         _view.Height.ShouldBeEqualTo(500);
         _view.Width.ShouldBeEqualTo(500);
      }
   }

   public class When_resizing_a_view_whose_width_is_smaller_than_the_target_width_but_height_is_bigger_that_the_target_height : StaticContextSpecification
   {
      private Form _view;
      private Rectangle _boundaries;

      protected override void Context()
      {
         base.Context();
         _view = new Form {Width = 800, Height = 300};
         _boundaries = new Rectangle {Width = 1000, Height = 500};
      }

      protected override void Because()
      {
         FormExtensions.ReziseForCurrentScreen(_view, _boundaries, fractionWidth: 1, fractionHeight: 0.5, resizeOnlyIfOutOfBound: true);
      }

      [Observation]
      public void should_have_resized_the_height_to_the_target_height_only()
      {
         _view.Height.ShouldBeEqualTo(250);
         _view.Width.ShouldBeEqualTo(800);
      }
   }

   public class When_showing_a_form : StaticContextSpecification
   {
      private Rectangle _workingArea;
      private Form _view;

      protected override void Context()
      {
         base.Context();
         _workingArea = new Rectangle(0, 0, 500, 500);
      }

      [TestCase(1, 1, 499, 499, 1, 1, 499, 499, Description = "fits inside working area")]
      [TestCase(-1000, -1000, 100, 200, 0, 0, 100, 200, Description = "completely outside working area")]
      [TestCase(-10, -10, 100, 200, 0, 0, 100, 200, Description = "shifted into the frame right and down")]
      [TestCase(490, 490, 100, 200, 300, 400, 100, 200, Description = "shifted into the frame left and up")]
      [TestCase(-10, -10, 2000, 2000, 0, 0, 500, 500, Description = "clipped to the view area")]
      public void the_layout_should_be_set_so_the_form_is_completely_visible_in_the_view(int x, int y, int height, int width, int expectedX, int expectedY, int expectedHeight, int expectedWidth)
      {
         _view = new Form { Width = 800, Height = 300 };
         _view.FitToScreen(new Rectangle(x,y,width,height), _workingArea);

         _view.Location.X.ShouldBeEqualTo(expectedX);
         _view.Location.Y.ShouldBeEqualTo(expectedY);
         _view.Size.Width.ShouldBeEqualTo(expectedWidth);
         _view.Size.Height.ShouldBeEqualTo(expectedHeight);
      }
   }
}