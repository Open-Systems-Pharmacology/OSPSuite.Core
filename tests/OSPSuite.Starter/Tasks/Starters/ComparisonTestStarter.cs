using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface IComparisonTestStarter : ITestStarter
   {
      
   }
   public class ComparisonTestStarter : TestStarter<IComparisonTestPresenter>, IComparisonTestStarter
   {
      public ComparisonTestStarter(IComparisonTestPresenter presenter) : base(presenter)
      {
      }
   }
}