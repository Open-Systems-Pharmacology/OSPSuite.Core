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
      private IDataSourceFile _tempDataSourceFile;
      public DataViewingPresenter(IDataViewingControl view) : base(view)
      {
         _tempDataSourceFile = new ExcelDataSourceFile(null);
      }

      public void SetDataSource(IDataSourceFile dataSourceFile) //should be deleted actually
      {
         _tempDataSourceFile = dataSourceFile;
         View.SetGridSource();
      }

      public void SetTabData(string sheetName) //should be deleted actually
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
   }
}