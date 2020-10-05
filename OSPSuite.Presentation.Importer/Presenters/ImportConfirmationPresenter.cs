using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using DevExpress.DataProcessing;
using OSPSuite.Core.Importer;


namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportConfirmationPresenter : AbstractPresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
   {
      private IImporter _importer;
      private string _namingConvention { get; set; }
      private string _fileName;
      private IDataSource _dataSource;
      private IEnumerable<IReadOnlyDictionary<Column, IList<ValueAndLloq>>> _plainData;
      private IEnumerable<MetaDataMappingConverter> _mappings;

      public ImportConfirmationPresenter(IImportConfirmationView view, IImporter importer) : base(view)
      {
         _importer = importer;
         _dataSource = new DataSource();
      }

      public void TriggerNamingConventionChanged(string namingConvention)
      {
         setNames(namingConvention);
      }

      public void ImportDataForConfirmation(string fileName, IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings)
      {
         _fileName = fileName; //should we set this every time?
         _mappings = mappings;
         _importer.AddFromFile(format, dataSheets, columnInfos, _dataSource);

         if (namingConventions == null)
            throw new NullNamingConventionsException();

         var conventions = namingConventions.ToList();

         if (conventions.Count == 0)
            throw new EmptyNamingConventionsException();

         _view.SetNamingConventions(conventions);
         setNames(conventions.First());
         _plainData = _dataSource.DataSets.SelectMany(ds => ds.Data.Select(p => p.Data));
      }

      private void setNames(string namingConvention)
      {
         _namingConvention = namingConvention;
         _view.SetDataSetNames
         (
            _importer.NamesFromConvention(_namingConvention, _fileName, _dataSource.DataSets, _mappings)
         );
      }
   }
}
