using System.Collections.Generic;
using System.Data;
using OSPSuite.Core.Importer;
using OSPSuite.UI.Views.Importer;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer
{
   public interface IDataViewingPresenter : IPresenter<IDataViewingControl>
   {
      void SetDataSource(IDataSourceFile dataSourceFile);
      void SetTabData(string sheetName);
      List<string> GetSheetNames();
      DataTable GetSheet(string tabName);
      void RemoveTabData(string tabName);
      void RemoveAllButThisTabData(string tabName);
   }
}