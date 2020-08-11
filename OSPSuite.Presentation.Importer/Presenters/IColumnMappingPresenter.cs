using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingOption
   {
      public string Description { get; set; }
      public int IconIndex { get; set; }
   }

   public interface IColumnMappingPresenter : IPresenter<IColumnMappingControl>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      void SetDataFormat(IDataFormat format);

      IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(int rowHandle);
   }
}