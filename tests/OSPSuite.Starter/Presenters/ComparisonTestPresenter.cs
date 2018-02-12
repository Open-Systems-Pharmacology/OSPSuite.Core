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

         var p1C1 = new Parameter().WithName("P1").WithParentContainer(container1);
         p1C1.Value = 5;
         p1C1.ValueOrigin.Method = ValueOriginDeterminationMethods.InVitro;
         p1C1.ValueOrigin.Source = ValueOriginSources.Database;

         var container2 = new Container().WithName("C2");
         var p1C2 = new Parameter().WithName("P1").WithParentContainer(container2);
         p1C2.Value = 4;
         p1C2.ValueOrigin.Method = ValueOriginDeterminationMethods.InVitro;
         p1C2.ValueOrigin.Source = ValueOriginSources.ParameterIdentification;

         _mainComparisonPresenter.CompareObjects(container1, container2);
      }
   }
}