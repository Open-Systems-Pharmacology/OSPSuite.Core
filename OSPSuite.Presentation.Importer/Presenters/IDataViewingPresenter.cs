using System.Collections.Generic;
using System.Data;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IDataViewingPresenter : IPresenter<IDataViewingControl>
   {
      void SetDataSource(string path);
      void SetTabData(string sheetName);
      List<string> GetSheetNames();
      DataTable GetSheet(string tabName);
   }
}