using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IImporterStarter : ITestStarter
   {
   }

   public class ImporterStarter : TestStarter<IImporterTestPresenter>, IImporterStarter
   {
   }
}
