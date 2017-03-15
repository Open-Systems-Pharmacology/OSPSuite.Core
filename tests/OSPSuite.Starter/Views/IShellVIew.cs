using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IShellView : IShell, IView<IShellPresenter>
   {
      void Show();
   }
}