using System;
using OSPSuite.UI.Views.Importer;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Core.Importer;
using System.Collections.Generic;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer
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
