using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IHistogramTestStarter : ITestStarter
   {
   }

   public class HistogramTestStarter : TestStarter<IHistogramTestPresenter>, IHistogramTestStarter
   {
      public override void Start()
      {
         base.Start(1200, 800);
      }
   }
}