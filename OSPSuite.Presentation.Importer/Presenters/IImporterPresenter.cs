using System;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportSheetsEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }
   public class FormatChangedEventArgs : EventArgs
   {
      public string Format { get; set; }
   }
   public interface IImporterPresenter : IPresenter<IImporterView>
   {
      void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats);

      void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      );

      event EventHandler<FormatChangedEventArgs> OnFormatChanged;

      event EventHandler<ImportSheetsEventArgs> OnImportSheets;

      void SetDataSource(string dataSourceFileName);
      void SelectTab(string tabName);
      void RemoveTab(string tabName);
      void RemoveAllButThisTab(string tabName);
      IEnumerable<string> GetNamingConventions();
      void ImportDataForConfirmation();
      void ImportDataForConfirmation(string sheetName);
      void SetNewFormat(string formatName);
      void RefreshTabs();//should this be here actually, or in the view? - then the view should only get the list of the sheet names from the _dataviewingpresenter
   }
}
