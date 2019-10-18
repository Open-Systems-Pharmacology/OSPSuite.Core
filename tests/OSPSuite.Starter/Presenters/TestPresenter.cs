using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Starter.Presenters
{
   public interface ITestPresenter : IPresenter<ITestView>
   {
      void StartChartTest();
      void StartJournalTest();
      void StartGridTest();
      void StartComparisonTest();
      void StartExplorerTest();
      void StartImporterTest();
      void StartShellTest();
      void StartDataRepositoryTest();
      void StartPivotGridTest();
      void StartParameterIdentificationTest();
      void StartSensitivityAnalysisTest();
      void StartCommandBrowserTest();
      void StartSimpleUITest();
      void StartExceptionView();
      void StartHistogramTest();
      void StartMatrixTest();
   }

   public class TestPresenter : AbstractPresenter<ITestView, ITestPresenter>, ITestPresenter
   {
      private readonly IGridTestStarter _girdTestStarter;
      private readonly IShellPresenter _shellPresenter;
      private readonly IOptimizationStarter _optimizationStarter;
      private readonly ISensitivityAnalysisStarter _sensitivityAnalysisStarter;
      private readonly ICommandBrowserStarter _commandBrowserStarter;
      private readonly ISimpleUIStarter _simpleUIStarter;

      public TestPresenter(ITestView view, IGridTestStarter girdTestStarter,
         IShellPresenter shellPresenter, IOptimizationStarter optimizationStarter, ISensitivityAnalysisStarter sensitivityAnalysisStarter,
         ICommandBrowserStarter commandBrowserStarter, ISimpleUIStarter simpleUIStarter) : base(view)
      {
         _girdTestStarter = girdTestStarter;
         _shellPresenter = shellPresenter;
         _optimizationStarter = optimizationStarter;
         _sensitivityAnalysisStarter = sensitivityAnalysisStarter;
         _commandBrowserStarter = commandBrowserStarter;
         _simpleUIStarter = simpleUIStarter;
      }

      private void start<T>(int width = 0, int height = 0) where T : IPresenter
      {
         var starter = new TestStarter<T>();
         starter.Start(width, height);
      }

      private void startLarge<T>() where T : IPresenter => start<T>(1200, 800);

      public void StartHistogramTest() => startLarge<IHistogramTestPresenter>();

      public void StartMatrixTest() => startLarge<IMatrixTestPresenter>();

      public void StartChartTest() => startLarge<IChartTestPresenter>();

      public void StartJournalTest() => startLarge<IJournalTestPresenter>();

      public void StartGridTest() => _girdTestStarter.Start();

      public void StartComparisonTest() => start<IComparisonTestPresenter>();

      public void StartExplorerTest() => start<IExplorerTestPresenter>();

      public void StartImporterTest() => start<IImporterTestPresenter>();

      public void StartShellTest() => _shellPresenter.Start();

      public void StartDataRepositoryTest() => startLarge<IDataRepositoryTestPresenter>();

      public void StartPivotGridTest() => start<IPivotGridTestPresenter>();

      public void StartParameterIdentificationTest() => _optimizationStarter.Start();

      public void StartSensitivityAnalysisTest() => _sensitivityAnalysisStarter.Start();

      public void StartCommandBrowserTest() => _commandBrowserStarter.Start();

      public void StartSimpleUITest() => _simpleUIStarter.Start();

      public void StartExceptionView()
      {
         Parameter parameter = null;
         this.DoWithinExceptionHandler(() => parameter.Value = 10);
      }
   }
}