using System.Collections.Generic;
using System.Data;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public interface IDataViewingPresenter : IPresenter<IDataViewingControl>
   {
      void SetDataSource(string path);

      List<string> GetSheetNames();
      DataTable GetSheet(string tabName);
   }
}