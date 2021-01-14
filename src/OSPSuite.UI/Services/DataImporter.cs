
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.UI.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.UI.Services
{
   public class DataImporter : IDataImporter
   {
      private readonly Utility.Container.IContainer _container;
      private readonly IDialogCreator _dialogCreator;

      public DataImporter(OSPSuite.Utility.Container.IContainer container, IDialogCreator dialogCreator)
      {
         _container = container;
         _dialogCreator = dialogCreator;
      }

      public IEnumerable<DataRepository> ImportDataSets(IReadOnlyList<MetaDataCategory> metaDataCategories, IReadOnlyList<ColumnInfo> columnInfos, DataImporterSettings dataImporterSettings)
      {
         
         var path = _dialogCreator.AskForFileToOpen(Captions.Importer.PleaseSelectDataFile, Captions.Importer.ImportFileFilter, Constants.DirectoryKey.OBSERVED_DATA);
         
         if (string.IsNullOrEmpty(path))
            return new List<DataRepository>();

         using (var importerPresenter = _container.Resolve<IImporterPresenter>())
         {
            importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
            importerPresenter.SetSourceFile(path);
            using (var importerModalPresenter = _container.Resolve<IModalImporterPresenter>())
            {
               return importerModalPresenter.ImportDataSets(importerPresenter, metaDataCategories, columnInfos, dataImporterSettings);
            }
         }
      }
   }
}
