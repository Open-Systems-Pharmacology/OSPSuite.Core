using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

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
         _importerPresenter.View.OnImportAllSheets += ImportAllSheets; //should we actually be doing this? subscribing to the view directly?
         _importerPresenter.View.OnImportSingleSheet += ImportSingleSheet; //should we actually be doing this? subscribing to the view directly?
         _view.AddImporterView(_importerPresenter.View);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void AddConfirmationView()
      {
         _view.AddConfirmationView(_confirmationPresenter.View);
      }


      public void ImportAllSheets()
      {
         startImport(_importerPresenter.GetAllSheets());
      }
      public void ImportSingleSheet(string sheetName)
      {
         startImport(new Dictionary<string, IDataSheet>() { { sheetName, _importerPresenter.GetSingleSheet(sheetName) } });
      }

      private void startImport(IReadOnlyDictionary<string, IDataSheet> sheets)
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