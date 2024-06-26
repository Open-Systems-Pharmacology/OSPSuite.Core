﻿using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Views;

namespace OSPSuite.Starter.Views
{
   public partial class TestView : BaseView, ITestView
   {
      private ITestPresenter _presenter;

      public TestView()
      {
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         startChartTestButton.Click += (sender, args) => OnEvent(_presenter.StartChartTest);
         startJournalTestButton.Click += (sender, args) => OnEvent(_presenter.StartJournalTest);
         startGridTestButton.Click += (sender, args) => OnEvent(_presenter.StartGridTest);
         startComparisonTestButton.Click += (sender, args) => OnEvent(_presenter.StartComparisonTest);
         startExplorerTestButton.Click += (sender, args) => OnEvent(_presenter.StartExplorerTest);
         startImporterTestButton.Click += (sender, args) => OnEvent(_presenter.StartImporterTest);
         startImporterLoadTestButton.Click += (sender, args) => OnEvent(_presenter.StartImporterLoadTest);
         startImporterReloadTestButton.Click += (sender, args) => OnEvent(_presenter.StartImporterReloadTest);
         startShellTestButton.Click += (sender, args) => OnEvent(_presenter.StartShellTest);
         startDataRepositoryTestButton.Click += (sender, args) => OnEvent(_presenter.StartDataRepositoryTest);
         startPivotGridTestButton.Click += (sender, args) => OnEvent(_presenter.StartPivotGridTest);
         startParameterIdentificationTestButton.Click += (sender, args) => OnEvent(_presenter.StartParameterIdentificationTest);
         startCommandBrowserTestButton.Click += (sender, args) => OnEvent(_presenter.StartCommandBrowserTest);
         startSimpleUITestButton.Click += (sender, args) => OnEvent(_presenter.StartSimpleUITest);
         startExceptionViewButton.Click += (sender, args) => OnEvent(_presenter.StartExceptionView);
         startSensitivityAnalysisTestButton.Click += (sender, args) => OnEvent(_presenter.StartSensitivityAnalysisTest);
         startHistogramTestButton.Click += (sender, args) => OnEvent(_presenter.StartHistogramTest);
         startMatrixTestButton.Click += (sender, args) => OnEvent(_presenter.StartMatrixTest);
         startEmptyFormButton.Click += (sender, args) => OnEvent(_presenter.StartEmptyFormTest);
         startLoggerButton.Click += (sender, args) => OnEvent(_presenter.StartLoggerTest);
         startDialogCreatorButton.Click += (sender, args) => OnEvent(_presenter.StartDialogCreatorTest);
         startDynamicTestButton.Click += (sender, args) => OnEvent(_presenter.StartDynamicTest);
         showModalButton.Click += (sender, args) => OnEvent(_presenter.ShowModalTest);
         showWizardButton.Click += (sender, args) => OnEvent(_presenter.ShowWizardTest);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         startChartTestButton.Text = "Start Chart Test";
         startJournalTestButton.Text = "Start Journal Test";
         startGridTestButton.Text = "Start Grid Test";
         startComparisonTestButton.Text = "Start Comparison Test";
         startExplorerTestButton.Text = "Start Explorer Test";
         startImporterTestButton.Text = "Start Importer Test";
         startImporterLoadTestButton.Text = "Start Importer Load Test";
         startImporterReloadTestButton.Text = "Start Importer Reload Test";
         startShellTestButton.Text = "Start Shell Test";
         startDataRepositoryTestButton.Text = "Start Data Repository Test";
         startPivotGridTestButton.Text = "Start Pivot Grid Test";
         startParameterIdentificationTestButton.Text = "Start Parameter Identification Test";
         startCommandBrowserTestButton.Text = "Start Command Browser Test";
         startSimpleUITestButton.Text = "Simple UI Test";
         startExceptionViewButton.Text = "Start Exception View";
         startSensitivityAnalysisTestButton.Text = "Start Sensitivity Analysis Test";
         startHistogramTestButton.Text = "Start Histogram Test";
         startMatrixTestButton.Text = "Start Matrix Test";
         startEmptyFormButton.Text = "Start Empty Form";
         startLoggerButton.Text = "Start Logger Test";
         startDialogCreatorButton.Text = "Start Dialog Creator Test";
         startDynamicTestButton.Text = "Start Dynamic Test";
         showWizardButton.Text = "Start Wizard Test";
         showModalButton.Text = "Start Modal Test";
      }

      public void AttachPresenter(ITestPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}
