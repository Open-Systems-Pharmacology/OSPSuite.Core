using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Services;
using System.Linq;

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
      void StartImporterReloadTest();
      void StartImporterLoadTest();
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
      void StartEmptyFormTest();
      void StartLoggerTest();
      void StartDialogCreatorTest();
   }

   public class TestPresenter : AbstractPresenter<ITestView, ITestPresenter>, ITestPresenter
   {
      private readonly IGridTestStarter _girdTestStarter;
      private readonly IShellPresenter _shellPresenter;
      private readonly IOptimizationStarter _optimizationStarter;
      private readonly ISensitivityAnalysisStarter _sensitivityAnalysisStarter;
      private readonly ICommandBrowserStarter _commandBrowserStarter;
      private readonly ISimpleUIStarter _simpleUIStarter;
      private readonly IOSPSuiteLogger _logger;
      private readonly IDialogCreator _dialogCreator;

      public TestPresenter(ITestView view, IGridTestStarter girdTestStarter,
         IShellPresenter shellPresenter, IOptimizationStarter optimizationStarter, ISensitivityAnalysisStarter sensitivityAnalysisStarter,
         ICommandBrowserStarter commandBrowserStarter, ISimpleUIStarter simpleUIStarter, IImporterConfigurationDataGenerator dataGenerator,
         IOSPSuiteLogger logger, IDialogCreator dialogCreator) : base(view)
      {
         _girdTestStarter = girdTestStarter;
         _shellPresenter = shellPresenter;
         _optimizationStarter = optimizationStarter;
         _sensitivityAnalysisStarter = sensitivityAnalysisStarter;
         _commandBrowserStarter = commandBrowserStarter;
         _simpleUIStarter = simpleUIStarter;
         _logger = logger;
         _dialogCreator = dialogCreator;
      }

      public void StartLoggerTest()
      {
         _logger.AddCriticalError("Critical", "Category3");
         _logger.AddInfo("Info");
      }

      public void StartDialogCreatorTest()
      {
         _dialogCreator.MessageBoxError("This is an error message");
         _dialogCreator.MessageBoxInfo("this is an info message");
         _dialogCreator.MessageBoxYesNoCancel("This is a YesNoCancelMessage");
         _dialogCreator.MessageBoxYesNoCancel(
            "And this is a message with a <href =https://docs.open-systems-pharmacology.org> hyperlink </href>");
      }

      private void start<T>(int width = 0, int height = 0) where T : IPresenter
      {
         var starter = new TestStarter<T>();
         starter.Start(width, height);
      }

      private void startLarge<T>() where T : IPresenter => start<T>(1200, 800);

      public void StartHistogramTest() => startLarge<IHistogramTestPresenter>();

      public void StartMatrixTest() => startLarge<IMatrixTestPresenter>();

      public void StartEmptyFormTest() => startLarge<IEmptyTestFormTestPresenter>();

      public void StartChartTest() => startLarge<IChartTestPresenter>();

      public void StartJournalTest() => startLarge<IJournalTestPresenter>();

      public void StartGridTest() => _girdTestStarter.Start();

      public void StartComparisonTest() => start<IComparisonTestPresenter>();

      public void StartExplorerTest() => start<IExplorerTestPresenter>();

      public void StartImporterTest() => start<IImporterTestPresenter>();

      public void StartImporterReloadTest() {
         var presenter = IoC.Resolve<IImporterTestPresenter>();
         presenter.ReloadWithPKSimSettings();
      }

      public void StartImporterLoadTest()
      {
         var presenter = IoC.Resolve<IImporterTestPresenter>();
         presenter.LoadWithPKSimSettings();
      }

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