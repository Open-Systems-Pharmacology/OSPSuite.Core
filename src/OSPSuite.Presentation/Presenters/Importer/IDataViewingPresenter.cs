using System.Collections.Generic;
using System.Data;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
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