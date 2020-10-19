using System;
using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Core.Importer;
using OSPSuite.UI.Views.Importer;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer
{
   public class ImportTriggeredEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }

   public interface IImporterTiledPresenter : IPresenter<IImporterTiledView>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );
      void AddConfirmationView();
      void AddDataMappingView();

      event EventHandler<ImportTriggeredEventArgs> OnTriggerImport;
   }
}
