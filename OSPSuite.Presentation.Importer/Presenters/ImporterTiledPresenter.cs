using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImporterTiledPresenter : AbstractPresenter<IImporterTiledView, IImporterTiledPresenter>, IImporterTiledPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private readonly IImportConfirmationPresenter _confirmationPresenter;
      
      public ImporterTiledPresenter(IImporterTiledView view, IImporterPresenter importerPresenter, IImportConfirmationPresenter confirmationPresenter) : base(view)
      {
         _importerPresenter = importerPresenter;
         _confirmationPresenter = confirmationPresenter;
         _confirmationPresenter.OnImportData += ImportData;
         _importerPresenter.OnImportSheets += ImportSheets;
         _view.AddImporterView(_importerPresenter.View);
         AddSubPresenters(_importerPresenter, _confirmationPresenter); 
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void AddConfirmationView()
      {
         _view.AddConfirmationView(_confirmationPresenter.View);
      }

      public void ImportData(object sender, ImportDataEventArgs e)
      {
         OnTriggerImport.Invoke(this, new ImportTriggeredEventArgs { DataSource = e.DataSource });
      }


      public void ImportSheets(object sender, ImportSheetsEventArgs args)
      {
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