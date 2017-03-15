using System.Collections.Generic;

namespace OSPSuite.Core.Importer
{
   public class ImportTableConfiguration
   {
      public IReadOnlyList<MetaDataCategory> MetaDataCategories { set; get; }
      public IReadOnlyList<ColumnInfo> ColumnInfos { set; get; }
   }
}
