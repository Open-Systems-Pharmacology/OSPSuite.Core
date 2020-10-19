using System;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.UI.Views.Importer;


namespace OSPSuite.Presentation.Importer
{
   public class ImportDataEventArgs : EventArgs
   {
      public IDataSource DataSource { get; set; }
   }
   public interface IImportConfirmationPresenter : IPresenter<IImportConfirmationView>
   {void TriggerNamingConventionChanged(string namingConvention);
      void SetDataSource(IDataSource dataSource);
      void SetNamingConventions(IEnumerable<string> namingConventions);
      void ImportData();

      event EventHandler<ImportDataEventArgs> OnImportData;
   }
}
