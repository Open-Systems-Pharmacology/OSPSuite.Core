using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Importer.Services;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImporterTiledPresenter : AbstractPresenter<IImporterTiledView, IImporterTiledPresenter>, IImporterTiledPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private readonly IImporter _importer;
      private IImportConfirmationPresenter _confirmationPresenter;
      
      public ImporterTiledPresenter(IImporter importer, IImporterTiledView view, IImporterPresenter importerPresenter, IImportConfirmationPresenter confirmationPresenter) : base(view)
      {
         _importer = importer;
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
         IEnumerable<string> namingConventions;
         IEnumerable<MetaDataMappingConverter> mappings;
         IDataFormat format;
         string fileName;
         IReadOnlyList<ColumnInfo> columnInfos;
         _importerPresenter.GetDataForImport(out fileName, out format, out columnInfos, out namingConventions, out mappings);

         _confirmationPresenter.ImportDataForConfirmation(fileName, format, sheets, columnInfos, namingConventions, mappings);

         //_importerPresenter.FillConfirmationView(ref _confirmationPresenter); //not sure about this method
         AddConfirmationView();
      }

      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerPresenter.View);
      }
   }
}