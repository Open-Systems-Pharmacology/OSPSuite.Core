using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface ISimpleUIStarter : ITestStarter
   {
   }

   public class SimpleUIStarter : ISimpleUIStarter
   {
      public void Start()
      {
         var form = new SimpleUIStarterView();
         form.Show();
      }
   }
}