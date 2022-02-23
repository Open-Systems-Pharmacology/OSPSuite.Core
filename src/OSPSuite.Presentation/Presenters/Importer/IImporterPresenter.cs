using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Utility.Collections;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ImportTriggeredEventArgs : EventArgs
   {
      public IReadOnlyList<DataRepository> DataRepositories { get; set; }
   }

   public interface IImporterPresenter : IDisposablePresenter
   {
      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         Cache<string, ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      bool SetSourceFile(string path);

      event EventHandler<ImportTriggeredEventArgs> OnTriggerImport;

      void SaveConfiguration();

      void LoadConfiguration(ImporterConfiguration configuration, string fileName);

      ImporterConfiguration UpdateAndGetConfiguration();
      void LoadConfigurationWithoutImporting();
   }
}
