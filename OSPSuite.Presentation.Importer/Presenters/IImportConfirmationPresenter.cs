using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImportConfirmationPresenter : IPresenter<IImportConfirmationView>
   {
      void Show(string fileName, IDataSource dataSource, IEnumerable<string> names, IEnumerable<MetaDataMappingConverter> mappings);

      void ImportDataForConfirmation(string fileName,  IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings);

      void TriggerNamingConventionChanged(string namingConvention);
   }
}
