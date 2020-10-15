using System;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportDataEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }
   public interface IImportConfirmationPresenter : IPresenter<IImportConfirmationView>
   {
      void ImportDataForConfirmation(string fileName,  IDataFormat format, Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings);
      void TriggerNamingConventionChanged(string namingConvention);
      void SetMappings(string fileName, IEnumerable<MetaDataMappingConverter> mappings);
      void AddSheets(IDataFormat format, Cache<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos);
      void SetNamingConventions(IEnumerable<string> namingConventions);
      void ImportData();

      event EventHandler<ImportDataEventArgs> OnImportData;
   }
}
