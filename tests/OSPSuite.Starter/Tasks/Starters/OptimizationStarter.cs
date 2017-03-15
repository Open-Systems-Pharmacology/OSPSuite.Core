using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IOptimizationStarter : ITestStarter
   {
      
   }

   public class OptimizationStarter : TestStarter<IOptimizationStarterPresenter>, IOptimizationStarter
   {
      public OptimizationStarter(IOptimizationStarterPresenter presenter) : base(presenter)
      {
      }
   }
}
