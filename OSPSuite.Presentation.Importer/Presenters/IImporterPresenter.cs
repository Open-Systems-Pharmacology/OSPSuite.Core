using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Importer.Core;
using System.Collections.Generic;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IImporterPresenter : IPresenter<IImporterView>
   {
      void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats);

      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      event FormatChangedHandler OnFormatChanged;

      event OnTriggerImportHandler OnTriggerImport;
      void SetDataSource(string dataSourceFileName);
      void SelectTab(string tabName);
   }

   public delegate void FormatChangedHandler(string format);

   public delegate void OnTriggerImportHandler( IDataSource dataSource);

}
