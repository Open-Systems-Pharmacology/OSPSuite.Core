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
      void StartSentitivityAnalysisTest();
      void StartCommandBrowserTest();
      void StartSimpleUITest();
      void StartExceptionView();

   }

   public class TestPresenter : AbstractPresenter<ITestView, ITestPresenter>, ITestPresenter
   {
      private readonly IChartTestStarter _chartTestStarter;
      private readonly IGridTestStarter _girdTestStarter;
      private readonly IJournalTestStarter _journalTestStarter;
      private readonly IComparisonTestStarter _comparisonTestStarter;
      private readonly IExplorerTestStarter _explorerTestStarter;
      private readonly IImporterStarter _importerStarter;
      private readonly IShellStarter _shellStarter;
      private readonly IDataRepositoryStarter _dataRepositoryStarter;
      private readonly IPivotGridStarter _pivotGridStarter;
      private readonly IOptimizationStarter _optimzationStarter;
      private readonly ICommandBrowserStarter _commandBrowserStarter;
      private readonly ISimpleUIStarter _simpleUIStarter;
      private readonly ISensitivityAnalysisStarter _sensitivityAnalysisStarter;

      public TestPresenter(ITestView view, IChartTestStarter chartTestStarter, IGridTestStarter girdTestStarter, 
         IJournalTestStarter journalTestStarter, IComparisonTestStarter comparisonTestStarter, IExplorerTestStarter explorerTestStarter,
         IImporterStarter importerStarter, IShellStarter shellStarter, IDataRepositoryStarter dataRepositoryStarter, IPivotGridStarter pivotGridStarter,
         IOptimizationStarter optimzationStarter,ICommandBrowserStarter commandBrowserStarter, ISimpleUIStarter simpleUIStarter, ISensitivityAnalysisStarter sensitivityAnalysisStarter) : base(view)
      {
         _chartTestStarter = chartTestStarter;
         _girdTestStarter = girdTestStarter;
         _journalTestStarter = journalTestStarter;
         _comparisonTestStarter = comparisonTestStarter;
         _explorerTestStarter = explorerTestStarter;
         _importerStarter = importerStarter;
         _shellStarter = shellStarter;
         _dataRepositoryStarter = dataRepositoryStarter;
         _pivotGridStarter = pivotGridStarter;
         _optimzationStarter = optimzationStarter;
         _commandBrowserStarter = commandBrowserStarter;
         _simpleUIStarter = simpleUIStarter;
         _sensitivityAnalysisStarter = sensitivityAnalysisStarter;
      }

      public void StartChartTest()
      {
         _chartTestStarter.Start();
      }

      public void StartJournalTest()
      {
         _journalTestStarter.Start();
      }

      public void StartGridTest()
      {
         _girdTestStarter.Start();
      }

      public void StartComparisonTest()
      {
         _comparisonTestStarter.Start();
      }

      public void StartExplorerTest()
      {
         _explorerTestStarter.Start();
      }

      public void StartImporterTest()
      {
         _importerStarter.Start();
      }

      public void StartShellTest()
      {
         _shellStarter.Start();
      }

      public void StartDataRepositoryTest()
      {
         _dataRepositoryStarter.Start();
      }

      public void StartPivotGridTest()
      {
         _pivotGridStarter.Start();
      }

      public void StartParameterIdentificationTest()
      {
         _optimzationStarter.Start();
      }

      public void StartSentitivityAnalysisTest()
      {
         _sensitivityAnalysisStarter.Start();
      }

      public void StartCommandBrowserTest()
      {
         _commandBrowserStarter.Start();
      }

      public void StartSimpleUITest()
      {
         _simpleUIStarter.Start();
      }

      public void StartExceptionView()
      {
         Parameter parameter = null;
         this.DoWithinExceptionHandler(() => parameter.Value = 10);
      }
   }
}