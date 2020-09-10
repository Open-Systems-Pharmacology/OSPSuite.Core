using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImporterTiledPresenter : AbstractPresenter<IImporterTiledView, IImporterTiledPresenter>, IImporterTiledPresenter
   {
      private IImporterPresenter _importerPresenter;
      public ImporterTiledPresenter(IImporterTiledView view, IImporterPresenter importerPresenter) : base(view)
      {
         _importerPresenter = importerPresenter;
         _view.AddImporterView(_importerPresenter.View);
      }
   }
}