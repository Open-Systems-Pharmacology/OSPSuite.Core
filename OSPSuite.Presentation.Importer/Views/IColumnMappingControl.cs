using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Views
{
   public interface IColumnMappingControl : IView<IColumnMappingPresenter>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      void SetMappingSource(IEnumerable<DataFormatParameter> mappings);
   }
}
