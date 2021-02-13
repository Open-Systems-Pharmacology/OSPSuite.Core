using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImportTriggeredEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }

   public interface IImporterPresenter : IDisposablePresenter
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      void SetSourceFile(string path);

      event EventHandler<ImportTriggeredEventArgs> OnTriggerImport;

      void SaveConfiguration(string fileName);
      void LoadConfigurationFromFilePath(string fileName);
      void LoadConfiguration(ImporterConfiguration configuration);
   }
}
