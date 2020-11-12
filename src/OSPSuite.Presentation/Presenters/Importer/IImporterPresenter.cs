using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImportTriggeredEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }

   public interface IImporterPresenter : IPresenter<IImporterView>
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );
      void AddConfirmationView();
      void AddDataMappingView();
      void SetSourceFile(string path);

      event EventHandler<ImportTriggeredEventArgs> OnTriggerImport;
   }
}
