using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IShellPresenter : IPresenter<IShellView>
   {
      void Start();
   }
   public class ShellPresenter : AbstractPresenter<IShellView, IShellPresenter>, IShellPresenter
   {
      private readonly IMenuAndToolBarPresenter _menuAndToolBarPresenter;

      public ShellPresenter(IShellView view, IMenuAndToolBarPresenter menuAndToolBarPresenter) : base(view)
      {
         _menuAndToolBarPresenter = menuAndToolBarPresenter;
      }

      public void Start()
      {
         _menuAndToolBarPresenter.Initialize();
         View.Show();
      }
   }
}