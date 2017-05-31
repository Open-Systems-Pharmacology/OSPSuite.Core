using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IPivotGridStarter : ITestStarter
   {

   }

   public class PivotGridStarter : TestStarter<IPivotGridTestPresenter>, IPivotGridStarter
   {
      public override void Start()
      {
         base.Start();
         _presenter.Edit();
      }
   }
}