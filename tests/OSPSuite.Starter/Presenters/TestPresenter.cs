using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Services;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.UI.Views;

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
      void StartDynamicTest();
      void ShowModalTest();
      void ShowWizardTest();
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
      private readonly IDynamicTestPresenter _dynamicTestPresenter;

      public TestPresenter(ITestView view, IGridTestStarter girdTestStarter,
         IShellPresenter shellPresenter, IOptimizationStarter optimizationStarter, ISensitivityAnalysisStarter sensitivityAnalysisStarter,
         ICommandBrowserStarter commandBrowserStarter, ISimpleUIStarter simpleUIStarter, IImporterConfigurationDataGenerator dataGenerator,
         IOSPSuiteLogger logger, IDialogCreator dialogCreator, IDynamicTestPresenter dynamicTestPresenter) : base(view)
      {
         _girdTestStarter = girdTestStarter;
         _shellPresenter = shellPresenter;
         _optimizationStarter = optimizationStarter;
         _sensitivityAnalysisStarter = sensitivityAnalysisStarter;
         _commandBrowserStarter = commandBrowserStarter;
         _simpleUIStarter = simpleUIStarter;
         _logger = logger;
         _dialogCreator = dialogCreator;
         _dynamicTestPresenter = dynamicTestPresenter;
      }

      public void StartLoggerTest()
      {
         _logger.AddCriticalError("Critical", "Category3");
         _logger.AddInfo("Info");
      }

      public void StartDialogCreatorTest()
      {
         // _dialogCreator.MessageBoxError("This is an error message");
         // _dialogCreator.MessageBoxInfo("this is an info message");
         // _dialogCreator.MessageBoxYesNoCancel("This is a YesNoCancelMessage");
         // _dialogCreator.MessageBoxYesNoCancel(
         //    "And this is a message with a <href =https://docs.open-systems-pharmacology.org> hyperlink </href>");

         _dialogCreator.AskForInput("This is the caption", "Hello","toto", new[] {"titi", "tutu"}, iconName:ApplicationIcons.Rename.IconName);
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

      public void StartImporterReloadTest()
      {
         var presenter = IoC.Resolve<IImporterTestPresenter>();
         presenter.ReloadWithPKSimSettings();
      }

      public void StartImporterLoadTest()
      {
         var presenter = IoC.Resolve<IImporterTestPresenter>();
         presenter.LoadWithPKSimSettings();
      }

      public void StartDynamicTest()
      {
         _dynamicTestPresenter.Start();
      }

      public void ShowModalTest()
      {
         var view = new BaseModalView();
         view.InitializeResources();
         view.Display();
      }

      public void ShowWizardTest()
      {
         var view = new WizardView();
         view.InitializeResources();
         view.Display();
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