using System;
using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
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
         _importerPresenter.OnImportAllSheets += ImportAllSheets;
         _importerPresenter.OnImportSingleSheet += ImportSingleSheet;
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


      public void ImportAllSheets(object sender, EventArgs e)
      {
         startImport(_importerPresenter.GetAllSheets());
      }
      public void ImportSingleSheet(object sender, ImportSingleSheetEventArgs args)
      {
         startImport(new Cache<string, IDataSheet>() { { args.SheetName, _importerPresenter.GetSingleSheet(args.SheetName) } });
      }

      private void startImport(Cache<string, IDataSheet> sheets)
      {
         _importerPresenter.GetDataForImport(out var fileName, out var format, out var columnInfos, out var namingConventions, out var mappings);
         _confirmationPresenter.ImportDataForConfirmation(fileName, format, sheets, columnInfos, namingConventions, mappings);

         AddConfirmationView();
         View.EnableConfirmationView();
      }

      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerPresenter.View);
      }
   }
}