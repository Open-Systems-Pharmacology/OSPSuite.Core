using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class MetaDataMappingConverter
   {
      public string Id { get; set; }

      public Func<string, int> Index { get; set; }
   }
   public interface IImportConfirmationPresenter : IDisposablePresenter
   {
      void Show(string fileName, IDataSource dataSource, IEnumerable<string> names, IEnumerable<MetaDataMappingConverter> mappings);
      bool Canceled { get; }
   }
}
