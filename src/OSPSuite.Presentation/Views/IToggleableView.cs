using System.Drawing;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IToggleableView : IView<IToogleablePresenter>
   {
      void ToggleVisibility();

      void Display();

      /// <summary>
      /// Moves and resizes the form according to <paramref name="location"/> and <paramref name="size"/>
      /// </summary>
      void SetFormLayout(Point location, Size size);

      void Hide();
   }
}