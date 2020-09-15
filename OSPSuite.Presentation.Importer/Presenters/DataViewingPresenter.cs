using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class DataViewingPresenter : AbstractPresenter<IDataViewingControl, IDataViewingPresenter>, IDataViewingPresenter
   {
      private IDataSourceFile _tempDataSourceFile; //this should not really be called temp...
      public DataViewingPresenter(IDataViewingControl view) : base(view)
      {
         _tempDataSourceFile = new ExcelDataSourceFile(null);
      }

      public void SetDataSource(IDataSourceFile dataSourceFile)
      {
         _tempDataSourceFile = dataSourceFile;
         View.SetGridSource();
      }

      public void SetTabData(string sheetName)
      {
         View.SetGridSource(sheetName);
      }

      public List<string> GetSheetNames()
      {
         return _tempDataSourceFile.DataSheets.Keys.ToList();
      }
      public DataTable GetSheet(string tabName)
      {
         return _tempDataSourceFile.DataSheets[tabName].RawData.AsDataTable();
      }

      public void RemoveTabData(string tabName)
      {
         _tempDataSourceFile.DataSheets.Remove(tabName);
      }
   }
}