using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IComparisonTestPresenter : IPresenter<IComparisonTestView>
   {
      
   }
   public class ComparisonTestPresenter : AbstractPresenter<IComparisonTestView, IComparisonTestPresenter>, IComparisonTestPresenter
   {
      private readonly IMainComparisonPresenter _mainComparisonPresenter;

      public ComparisonTestPresenter(IComparisonTestView view, IMainComparisonPresenter mainComparisonPresenter) : base(view)
      {
         _mainComparisonPresenter = mainComparisonPresenter;
         _mainComparisonPresenter.Initialize();

         var container1 = new Container().WithName("C1");
         var container2 = new Container().WithName("C2");
         _mainComparisonPresenter.CompareObjects(container1, container2);
      }
   }
}