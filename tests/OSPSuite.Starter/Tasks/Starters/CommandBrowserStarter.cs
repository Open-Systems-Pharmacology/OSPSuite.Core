using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface ICommandBrowserStarter: ITestStarter
   {
   }

   public class CommandBrowserStarter : ICommandBrowserStarter
   {
      public void Start(int width = 0, int height = 0)
      {
         var form = new StaticCommandList();
         form.Show();
      }
   }
}