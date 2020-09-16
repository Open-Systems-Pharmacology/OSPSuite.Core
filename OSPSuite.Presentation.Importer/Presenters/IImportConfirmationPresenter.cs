using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Views;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class MetaDataMappingConverter
   {
      public string Id { get; set; }

      public Func<string, int> Index { get; set; }
   }
   public interface IImportConfirmationPresenter : IDisposablePresenter//<IImportConfirmationView>
   {
      void Show(string fileName, IDataSource dataSource, IEnumerable<string> names, IEnumerable<MetaDataMappingConverter> mappings);
      bool Canceled { get; }
   }
}
