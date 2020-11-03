using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterTiledPresenter : AbstractPresenter<IImporterTiledView, IImporterTiledPresenter>, IImporterTiledPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private IDataSource _lastDataSource;

      public ImporterTiledPresenter(IImporterTiledView view, IImporterPresenter importerPresenter, IImportConfirmationPresenter confirmationPresenter, IColumnMappingPresenter columnMappingPresenter
      ) : base(view)
      {
         _importerPresenter = importerPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _confirmationPresenter.OnImportData += ImportData;
         _importerPresenter.OnImportSheets += ImportSheets;
         _view.AddImporterView(_importerPresenter.View);
         AddSubPresenters(_importerPresenter, _confirmationPresenter, _columnMappingPresenter);
         _importerPresenter.OnSourceFileChanged += (s, a) => { view.DisableConfirmationView(); };
         _importerPresenter.OnFormatChanged += onFormatChanged;
         _importerPresenter.OnTabChanged += onTabChanged;
         _columnMappingPresenter.OnMissingMapping += onMissingMapping;
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void AddConfirmationView()
      {
         _view.AddConfirmationView(_confirmationPresenter.View);
         if (_lastDataSource != null)
         {
            _confirmationPresenter.Refresh();
         }
      }

      public void ImportData(object sender, ImportDataEventArgs e)
      {
         _lastDataSource = e.DataSource;
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs { DataSource = _lastDataSource });
      }


      public void ImportSheets(object sender, ImportSheetsEventArgs args)
      {
         _lastDataSource = args.DataSource;
         _confirmationPresenter.SetDataSource(args.DataSource);
         _confirmationPresenter.SetNamingConventions(_importerPresenter.GetNamingConventions());
         AddConfirmationView();
         View.EnableConfirmationView();
      }

      private void onFormatChanged(object sender, FormatChangedEventArgs e)
      {
         _columnMappingPresenter.SetDataFormat(e.Format);
      }

      private void onTabChanged(object sender, TabChangedEventArgs e)
      {
         _columnMappingPresenter.SetRawData(e.TabData);
      }

      private void onMissingMapping(object sender, MissingMappingEventArgs missingMappingEventArgs)
      {
         _importerPresenter.onMissingMapping();
      }

      private void onCompletedMapping(object sender, EventArgs e)
      {
         _importerPresenter.onCompletedMapping();
      }
      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerPresenter.View);
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}