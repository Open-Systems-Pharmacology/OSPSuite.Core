using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Core.Domain;
using OSPSuite.Infrastructure.Import.Core.Mappers;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImportConfirmationPresenter : AbstractPresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
   {
      private IImporter _importer;
      private IDataSource _dataSource;
      private IDataSetToDataRepositoryMapper _dataRepositoryMapper;
      private readonly ISimpleChartPresenter _chartPresenter;

      //maybe better seperate concerns here
      public void DataSetToDataRepository(string key, int index)
      {
         var dataRepository = _dataRepositoryMapper.ConvertImportDataSet(_dataSource, index, key);
         View.ShowSelectedDataSet(dataRepository);
         _chartPresenter.PlotObservedData(dataRepository);
      }

      public event EventHandler<ImportDataEventArgs> OnImportData = delegate { };

      public ImportConfirmationPresenter(IImportConfirmationView view, IImporter importer, IDataSetToDataRepositoryMapper dataRepositoryMapper,
         ISimpleChartPresenter chartPresenter) : base(view)
      {
         _importer = importer;
         _dataSource = new DataSource(_importer); //we re just initializing to empty...
         _dataRepositoryMapper = dataRepositoryMapper;
         _chartPresenter = chartPresenter;
         View.AddChartView(_chartPresenter.View);
      }

      public void TriggerNamingConventionChanged(string namingConvention)
      {
         setNames(namingConvention);
      }

      public void SetDataSource(IDataSource dataSource)
      {
         _dataSource = dataSource;
         var keys = new List<string>()
         {
            Constants.FILE,
            Constants.SHEET
         };
         keys.AddRange(_dataSource.GetMappings().Select(m => m.Id));
         View.SetNamingConventionKeys(keys);
      }

      public void SetNamingConventions (IEnumerable<string> namingConventions)
      {
         if (namingConventions == null)
            throw new NullNamingConventionsException();

         var conventions = namingConventions.ToList();

         if (conventions.Count == 0)
            throw new EmptyNamingConventionsException();

         _view.SetNamingConventions(conventions);
         setNames(conventions.First());
      }

      public void ImportData()
      {
         OnImportData.Invoke(this, new ImportDataEventArgs { DataSource = _dataSource });
      }

      private void setNames(string namingConvention)
      {
         _dataSource.SetNamingConvention(namingConvention);
         _view.SetDataSetNames(_dataSource.NamesFromConvention());
      }
   }
}
