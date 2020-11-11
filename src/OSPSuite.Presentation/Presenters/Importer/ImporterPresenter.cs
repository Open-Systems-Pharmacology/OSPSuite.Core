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
      private readonly INanPresenter _nanPresenter;
      private IDataSource _lastDataSource;

      public ImporterPresenter(IImporterView view, INanPresenter nanPresenter, IImporterDataPresenter importerDataPresenter, IImportConfirmationPresenter confirmationPresenter, IColumnMappingPresenter columnMappingPresenter
      ) : base(view)
      {
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _nanPresenter = nanPresenter;
         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddNanView(nanPresenter.View);
         _confirmationPresenter.OnImportData += ImportData;
         _importerDataPresenter.OnImportSheets += ImportSheets;
         _view.AddImporterView(_importerDataPresenter.View);
         AddSubPresenters(_importerDataPresenter, _confirmationPresenter, _columnMappingPresenter);
         _importerDataPresenter.OnSourceFileChanged += (s, a) => { view.DisableConfirmationView(); };
         _importerDataPresenter.OnFormatChanged += onFormatChanged;
         _nanPresenter.OnNanSettingsChanged += onNaNSettingsChanged;
         _importerDataPresenter.OnTabChanged += onTabChanged;
         _columnMappingPresenter.OnMissingMapping += onMissingMapping;
         _columnMappingPresenter.OnMappingCompleted += onCompletedMapping;
      }

      private void onNaNSettingsChanged(object sender, NanSettingsChangedEventArgs e)
      {
         throw new NotImplementedException();
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
         args.DataSource.NanSettings = _nanPresenter.Settings;
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