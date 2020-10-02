using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImportConfirmationPresenter : IPresenter<IImportConfirmationView>
   {
      void ImportDataForConfirmation(string fileName,  IDataFormat format, Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings);

      void TriggerNamingConventionChanged(string namingConvention);
   }
}
