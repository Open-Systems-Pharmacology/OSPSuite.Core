using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImportConfirmationPresenter : IDisposablePresenter
   {
      void Show(string fileName, IDataSource dataSource, IEnumerable<string> names, IEnumerable<MetaDataMappingConverter> mappings);
      bool Canceled { get; }
   }
}
