using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImporterTiledPresenter : AbstractPresenter<IImporterTiledView, IImporterTiledPresenter>, IImporterTiledPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private IImportConfirmationPresenter _confirmationPresenter;
      public ImporterTiledPresenter(IImporterTiledView view, IImporterPresenter importerPresenter, IImportConfirmationPresenter confirmationPresenter) : base(view)
      {
         _importerPresenter = importerPresenter;
         _confirmationPresenter = confirmationPresenter;
         _view.AddImporterView(_importerPresenter.View);
      }

      public void SetSettings(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void AddConfirmationView()
      {
         _importerPresenter.FillConfirmationView(ref _confirmationPresenter); //not sure about this method
         _view.AddConfirmationView(_confirmationPresenter.View);
      }

      public void AddDataMappingView()
      {
         _view.AddImporterView(_importerPresenter.View);
      }
   }
}