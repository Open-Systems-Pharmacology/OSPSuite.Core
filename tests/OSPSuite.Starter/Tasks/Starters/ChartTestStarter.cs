using OSPSuite.Core.Commands;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IChartTestStarter : ITestStarter
   {
   }

   public class ChartTestStarter : TestStarter<IChartTestPresenter>, IChartTestStarter
   {

      public ChartTestStarter(IChartTestPresenter chartTestPresenter) : base(chartTestPresenter)

      {
      }

      public override void Start()
      {

         base.Start(1200, 800);
         _presenter.InitializeWith(new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>());
      }
   }
}