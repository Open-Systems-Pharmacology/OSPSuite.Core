using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterDataPresenter _importerDataPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private IDataSource _lastDataSource;

      public ImporterPresenter(IImporterView view, IImporterDataPresenter importerDataPresenter, IImportConfirmationPresenter confirmationPresenter, IColumnMappingPresenter columnMappingPresenter
      ) : base(view)
      {
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _confirmationPresenter.OnImportData += ImportData;
         _importerDataPresenter.OnImportSheets += ImportSheets;
         _view.AddImporterView(_importerDataPresenter.View);
         AddSubPresenters(_importerDataPresenter, _confirmationPresenter, _columnMappingPresenter);
         _importerDataPresenter.OnSourceFileChanged += (s, a) => { view.DisableConfirmationView(); };
         _importerDataPresenter.OnFormatChanged += onFormatChanged;
         _importerDataPresenter.OnTabChanged += onTabChanged;
         _columnMappingPresenter.OnMissingMapping += onMissingMapping;
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _importerDataPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
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
         _confirmationPresenter.SetNamingConventions(_importerDataPresenter.GetNamingConventions());
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
         _importerDataPresenter.onMissingMapping();
      }

      private void onCompletedMapping(object sender, EventArgs e)
      {
         _importerDataPresenter.onCompletedMapping();
      }
      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerDataPresenter.View);
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}