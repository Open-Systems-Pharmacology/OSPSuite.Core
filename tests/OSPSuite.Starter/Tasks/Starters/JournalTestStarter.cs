using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IJournalTestStarter : ITestStarter
   {
   }

   public class JournalTestStarter : TestStarter<IJournalTestPresenter>, IJournalTestStarter
   {
      public override void Start()
      {
         base.Start(1200, 800);
      }
   }
}