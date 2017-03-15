using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface ICommandBrowserStarter : ITestStarter
   {
   }

   public class CommandBrowserStarter : ICommandBrowserStarter
   {
      public void Start()
      {
         var form = new StaticCommandList();
         form.Show();
      }
   }
}