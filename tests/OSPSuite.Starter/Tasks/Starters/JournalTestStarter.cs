using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IJournalTestStarter : ITestStarter
   {
   }

   public class JournalTestStarter : TestStarter<IJournalTestPresenter>, IJournalTestStarter
   {
      public JournalTestStarter(IJournalTestPresenter presenter) : base(presenter)
      {
      }
   }
}