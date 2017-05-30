using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IExplorerTestStarter : ITestStarter
   {

   }
   public class ExplorerTestStarter : TestStarter<IExplorerTestPresenter>, IExplorerTestStarter
   {
   }
}