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
      public ImporterTiledPresenter(IImporterTiledView view) : base(view)
      {
      }
   }
}