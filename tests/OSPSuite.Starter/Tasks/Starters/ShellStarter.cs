using OSPSuite.Utility.Container;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IShellStarter : ITestStarter
   {
   }

   public class ShellStarter : IShellStarter
   {
      private readonly IContainer _container;

      public ShellStarter(IContainer container)
      {
         _container = container;
      }

      public void Start()
      {
         var presenter = _container.Resolve<IShellPresenter>();
         presenter.Start();
      }
   }
}