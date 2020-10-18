using System;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Presenters
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
