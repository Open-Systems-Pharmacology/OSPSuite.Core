using OSPSuite.Core.Domain;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Importer.Core;
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
      void StartImporterExcelView();
   }

   public class TestPresenter : AbstractPresenter<ITestView, ITestPresenter>, ITestPresenter
   {
      private readonly IGridTestStarter _girdTestStarter;
      private readonly IShellPresenter _shellPresenter;
      private readonly IOptimizationStarter _optimizationStarter;
      private readonly ISensitivityAnalysisStarter _sensitivityAnalysisStarter;
      private readonly ICommandBrowserStarter _commandBrowserStarter;
      private readonly ISimpleUIStarter _simpleUIStarter;
      private readonly IImporterConfigurationDataGenerator _dataGenerator;
      private readonly IDialogCreator _dialogCreator;


      public TestPresenter(ITestView view, IGridTestStarter girdTestStarter,
         IShellPresenter shellPresenter, IOptimizationStarter optimizationStarter, ISensitivityAnalysisStarter sensitivityAnalysisStarter,
         ICommandBrowserStarter commandBrowserStarter, ISimpleUIStarter simpleUIStarter, IImporterConfigurationDataGenerator dataGenerator,
         IDialogCreator dialogCreator) : base(view)
      {
         _girdTestStarter = girdTestStarter;
         _shellPresenter = shellPresenter;
         _optimizationStarter = optimizationStarter;
         _sensitivityAnalysisStarter = sensitivityAnalysisStarter;
         _commandBrowserStarter = commandBrowserStarter;
         _simpleUIStarter = simpleUIStarter;
         _dataGenerator = dataGenerator;
         _dialogCreator = dialogCreator;

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

      public void StartImporterExcelView()
      {
         var starter = new TestStarter<IImporterTiledPresenter>();
         starter.Start(660, 400);

         var dataImporterSettings = new DataImporterSettings();
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET);
         dataImporterSettings.AddNamingPatternMetaData(Constants.FILE, Constants.SHEET, "Species");
         starter.Presenter.SetSettings(
            _dataGenerator.DefaultPKSimMetaDataCategories(),
            _dataGenerator.DefaultPKSimConcentrationImportConfiguration(),
            dataImporterSettings);
      }

      private void startImportSuccessDialog(IDataSource dataSource)
      {
         _dialogCreator.MessageBoxInfo( dataSource.DataSets.Values.Select(d => d.Data.Count).Sum() + " data sets successfully imported");
      }
   }
}