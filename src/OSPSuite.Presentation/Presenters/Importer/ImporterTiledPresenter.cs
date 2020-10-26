using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImporterTiledPresenter : AbstractPresenter<IImporterTiledView, IImporterTiledPresenter>, IImporterTiledPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      private IDataSource _lastDataSource;

      public ImporterTiledPresenter(IImporterTiledView view, IImporterPresenter importerPresenter, IImportConfirmationPresenter confirmationPresenter) : base(view)
      {
         _importerPresenter = importerPresenter;
         _confirmationPresenter = confirmationPresenter;
         _confirmationPresenter.OnImportData += ImportData;
         _importerPresenter.OnImportSheets += ImportSheets;
         _view.AddImporterView(_importerPresenter.View);
         AddSubPresenters(_importerPresenter, _confirmationPresenter);
         _importerPresenter.OnSourceFileChanged += (s, a) => { view.DisableConfirmationView(); };
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
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

      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerPresenter.View);
      }

      public event EventHandler<ImportTriggeredEventArgs> OnTriggerImport = delegate { };
   }
}