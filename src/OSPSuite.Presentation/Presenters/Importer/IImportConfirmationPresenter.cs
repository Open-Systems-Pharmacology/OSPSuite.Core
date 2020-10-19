using System;
using System.Collections.Generic;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
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
