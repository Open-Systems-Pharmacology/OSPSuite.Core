using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSPSuite.Presentation.Importer.Core.DataSourceFileReaders;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class DataViewingPresenter : AbstractPresenter<IDataViewingControl, IDataViewingPresenter>, IDataViewingPresenter
   {
      private ExcelDataSourceFile _tempDataSourceFile;
      public DataViewingPresenter(IDataViewingControl view) : base(view)
      {
         _tempDataSourceFile = new ExcelDataSourceFile(null);
      }

      public void SetDataSource( string path )
      {
         _tempDataSourceFile.Path = path;
         View.SetGridSource();
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