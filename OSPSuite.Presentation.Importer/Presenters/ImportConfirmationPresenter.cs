using Org.BouncyCastle.Asn1.Cms;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ImportConfirmationPresenter : AbstractDisposablePresenter<IImportConfirmationView, IImportConfirmationPresenter>, IImportConfirmationPresenter
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
      }

      public void Show(string fileName, IDataSource dataSource, IEnumerable<string> namingConventions, IEnumerable<MetaDataMappingConverter> mappings)
      {
         _fileName = fileName;
         _mappings = mappings;
         _dataSource = dataSource;
         _view.SetNamingConventions(namingConventions);
         _view.OnNamingConventionChanged += (namingConvention) => this.DoWithinExceptionHandler(() => setNames(namingConvention));
         setNames(namingConventions.First());
         _plainData = _dataSource.DataSets.SelectMany(ds => ds.Value.Data.Values);
         //_plainData.SelectMany(d => d.Select(vlloq => { vlloq.Key.Name, vlloq.Key.Unit, vlloq.Value. }))
         //_view.OnSelectedDataSetChanged += (index) => this.DoWithinExceptionHandler(() => );
         _view.Display();
      }

      private void setNames(string namingConvention)
      {
         _namingConvention = namingConvention;
         _view.SetDataSetNames
         (
            _importer.NamesFromConvention(_namingConvention, _fileName, _dataSource.DataSets, _mappings)
         );
      }

      public bool Canceled => _view.Canceled;
   }
}
