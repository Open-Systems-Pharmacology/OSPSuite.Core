using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         string configurationId = null
      );

      IReadOnlyList<DataRepository> ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         ImporterConfiguration configuration
      );
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private readonly IDialogCreator _dialogCreator;

      public ModalImporterPresenter(IModalImporterView view, IImporterPresenter importerPresenter, IDialogCreator dialogCreator) : base(view)
      {
         _dialogCreator = dialogCreator;
         _importerPresenter = importerPresenter;
         AddSubPresenters(importerPresenter);
      }

      public IReadOnlyList<DataRepository> ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         ImporterConfiguration configuration
      )
      {
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
         _importerPresenter.LoadConfiguration(configuration, path);
         return importDataSets(configuration.Id).DataRepositories;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         string configurationId = null
      )
      {         
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
         (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) emptyImport = (Array.Empty<DataRepository>(), null);

         try
         {
            if (!_importerPresenter.SetSourceFile(path))
               return emptyImport;
         }
         catch (Exception e) when (e is UnsupportedFormatException || e is UnsupportedFileTypeException)
         {
            _dialogCreator.MessageBoxError(e.Message);
            return emptyImport;
         }

         return importDataSets(configurationId);
      }

      private (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) importDataSets(string configurationId)
      {
         IReadOnlyList<DataRepository> results = Array.Empty<DataRepository>();
         _view.FillImporterPanel(_importerPresenter.BaseView);
         var configuration = _importerPresenter.UpdateAndGetConfiguration();

         _importerPresenter.OnTriggerImport += (s, d) =>
         {
            results = d.DataRepositories;
            configuration = _importerPresenter.UpdateAndGetConfiguration();
         };

         if (!string.IsNullOrEmpty(configurationId))
         {
            configuration.Id = configurationId;
            results.Each(r => r.ConfigurationId = configurationId);
         }

         _view.AttachImporterPresenter(_importerPresenter);
         _view.Display();
         return (results, configuration);
      }
   }
}