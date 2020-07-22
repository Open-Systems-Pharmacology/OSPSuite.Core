using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IColumnMappingPresenter : IPresenter<IColumnMappingControl>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      void SetDataFormat(IDataFormat format);
   }
}
