using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Importer;


namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportConfirmationPresenter : AbstractPresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
   {
      private IImporter _importer;
      private string _namingConvention { get; set; }
      private string _fileName;
      private IDataSource _dataSource;
      private IEnumerable<Dictionary<Column, IList<ValueAndLloq>>> _plainData;
      private IEnumerable<MetaDataMappingConverter> _mappings;

      public ImportConfirmationPresenter(IImportConfirmationView view, IImporter importer) : base(view)
      {
         _importer = importer;
         _dataSource = new DataSource();
      }

      public void TriggerNamingConventionChanged(string namingConvention)
      {
         this.DoWithinExceptionHandler(() => setNames(namingConvention));
      }

      public void Show(string fileName, IDataSource dataSource, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings)
      {
         _fileName = fileName;
         _mappings = mappings;
         _dataSource = dataSource;
         _view.SetNamingConventions(namingConventions);
         setNames(namingConventions.First());
         _plainData = _dataSource.DataSets.SelectMany(ds => ds.Value.Data.Values);
      }

      public void ImportDataForConfirmation(string fileName, IDataFormat format, IReadOnlyDictionary<string, IDataSheet> dataSheets, IReadOnlyList<ColumnInfo> columnInfos, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings)
      {
         _fileName = fileName; //should we set this every time?
         _mappings = mappings;
         _importer.AddFromFile(format, dataSheets, columnInfos, _dataSource);

         _view.SetNamingConventions(namingConventions);
         setNames(namingConventions.First());
         _plainData = _dataSource.DataSets.SelectMany(ds => ds.Value.Data.Values);
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
