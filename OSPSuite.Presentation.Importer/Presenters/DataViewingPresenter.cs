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
      private IDataSourceFile _dataSourceFile;
      public DataViewingPresenter(IDataViewingControl view) : base(view)
      {
         _dataSourceFile = new ExcelDataSourceFile(null);
      }

      public void SetDataSource(IDataSourceFile dataSourceFile)
      {
         _dataSourceFile = dataSourceFile;
         View.SetGridSource();
      }

      public void SetTabData(string sheetName)
      {
         View.SetGridSource(sheetName);
      }

      public List<string> GetSheetNames()
      {
         return _dataSourceFile.DataSheets.Keys.ToList();
      }
      public DataTable GetSheet(string tabName)
      {
         return _dataSourceFile.DataSheets.TryGetValue(tabName, out var value) ? value.RawData.AsDataTable() : new DataTable();
      }

      public void RemoveTabData(string tabName)
      {
         _dataSourceFile.DataSheets.Remove(tabName);
      }

      public void RemoveAllButThisTabData(string tabName)
      {
         var remainingSheet = _dataSourceFile.DataSheets[tabName];
         _dataSourceFile.DataSheets.Clear();
         _dataSourceFile.DataSheets.Add(tabName, remainingSheet);
      }
   }
}