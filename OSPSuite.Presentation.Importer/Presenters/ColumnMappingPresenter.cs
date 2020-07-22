using Org.BouncyCastle.Asn1.Cms;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      public ColumnMappingPresenter(IColumnMappingControl view) : base(view) { }

      public void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         View.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
      }

      public void SetDataFormat(IDataFormat format)
      {
         View.SetMappingSource(format.Parameters);
      }
   }
}
