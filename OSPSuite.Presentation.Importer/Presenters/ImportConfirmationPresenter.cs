using System;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Importer;


namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportConfirmationPresenter : AbstractPresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
   {
      private IImporter _importer;
      private IDataSource _dataSource;
      

      public event EventHandler<ImportDataEventArgs> OnImportData = delegate { };

      public ImportConfirmationPresenter(IImportConfirmationView view, IImporter importer) : base(view)
      {
         _importer = importer;
         _dataSource = new DataSource(_importer); //we re just initializing to empty...
      }

      public void TriggerNamingConventionChanged(string namingConvention)
      {
         setNames(namingConvention);
      }

      public void SetDataSource(IDataSource dataSource)
      {
         _dataSource = dataSource;
      }

      public void SetNamingConventions (IEnumerable<string> namingConventions)
      {
         if (namingConventions == null)
            throw new NullNamingConventionsException();

         var conventions = namingConventions.ToList();

         if (conventions.Count == 0)
            throw new EmptyNamingConventionsException();

         _view.SetNamingConventions(conventions);
         setNames(conventions.First());
      }

      public void ImportData()
      {
         OnImportData.Invoke(this, new ImportDataEventArgs { DataSource = _dataSource });
      }

      private void setNames(string namingConvention)
      {
         _dataSource.SetNamingConvention(namingConvention);
         _view.SetDataSetNames(_dataSource.NamesFromConvention());
      }
   }
}
