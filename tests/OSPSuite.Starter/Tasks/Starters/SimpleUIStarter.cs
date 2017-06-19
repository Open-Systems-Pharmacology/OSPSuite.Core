using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface ISimpleUIStarter : ITestStarter
   {
   }

   public class SimpleUIStarter : ISimpleUIStarter
   {
      public void Start(int width = 0, int height = 0)
      {
         var form = new SimpleUIStarterView();
         form.Show();
      }
   }
}