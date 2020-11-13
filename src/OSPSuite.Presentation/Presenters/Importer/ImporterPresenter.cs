using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterPresenter : AbstractPresenter<IImporterView, IImporterPresenter>, IImporterPresenter
   {
      private readonly IImporterDataPresenter _importerDataPresenter;
      private readonly IColumnMappingPresenter _columnMappingPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private readonly INanPresenter _nanPresenter;
      private readonly IImporter _importer;
      private IDataSource _dataSource;

      public ImporterPresenter(IImporterView view, IImporter importer, INanPresenter nanPresenter, IImporterDataPresenter importerDataPresenter, IImportConfirmationPresenter confirmationPresenter, IColumnMappingPresenter columnMappingPresenter
      ) : base(view)
      {
         _importerDataPresenter = importerDataPresenter;
         _confirmationPresenter = confirmationPresenter;
         _columnMappingPresenter = columnMappingPresenter;
         _nanPresenter = nanPresenter;
         _importer = importer;
         _dataSource = new DataSource(_importer);

         _view.AddColumnMappingControl(columnMappingPresenter.View);
         _view.AddNanView(nanPresenter.View);
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
         _columnInfos = columnInfos;
         _columnMappingPresenter.SetSettings(metaDataCategories, columnInfos);
         _importerDataPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void AddConfirmationView()
      {
         _view.AddConfirmationView(_confirmationPresenter.View);
         if (_dataSource != null)  //NOT SURE ABOUT THIS REFERENCE HERE
         {
            _confirmationPresenter.Refresh();
         }
      }

      public void ImportData(object sender, ImportDataEventArgs e)
      {
         _dataSource = e.DataSource;
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs { DataSource = _dataSource });
      }


      public void ImportSheets(object sender, ImportSheetsEventArgs args)
      {
         var mappings = args.DataSourceFile.Format.Parameters.OfType<MetaDataFormatParameter>().Select(md => new MetaDataMappingConverter()
         {
            Id = md.MetaDataId,
            Index = sheetName => args.DataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
         }).Union
         (
            args.DataSourceFile.Format.Parameters.OfType<GroupByDataFormatParameter>().Select(md => new MetaDataMappingConverter()
            {
               Id = md.ColumnName,
               Index = sheetName => args.DataSourceFile.DataSheets[sheetName].RawData.GetColumnDescription(md.ColumnName).Index
            })
         );

         _dataSource.SetMappings(args.DataSourceFile.Path, mappings);
         _dataSource.NanSettings = _nanPresenter.Settings;
         _dataSource.SetDataFormat(_columnMappingPresenter.GetDataFormat());
         _dataSource.AddSheets(args.Sheets, _columnInfos);

         _confirmationPresenter.SetDataSource(_dataSource);
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

      public void SetSourceFile(string path)
      {
         _importerDataPresenter.SetDataSource(path);
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}