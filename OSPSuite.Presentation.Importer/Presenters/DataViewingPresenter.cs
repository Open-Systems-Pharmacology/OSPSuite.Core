using System;
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

      public DataTable GetFirstSheet()
      {
         var keys = _tempDataSourceFile.DataSheets.Keys;
         return _tempDataSourceFile.DataSheets[keys.ElementAt(0)].RawData.AsDataTable();
      }
   }
}