﻿using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         ColumnInfoCache columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         string configurationId = null
      );

      IReadOnlyList<DataRepository> ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         ColumnInfoCache columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         ImporterConfiguration configuration
      );

      void SetCaption(string caption);
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IImporterPresenter _importerPresenter;
      private readonly IDialogCreator _dialogCreator;
      private IReadOnlyList<DataRepository> _results;
      private ImporterConfiguration _configuration;

      public ModalImporterPresenter(IModalImporterView view, IImporterPresenter importerPresenter, IDialogCreator dialogCreator) : base(view)
      {
         _dialogCreator = dialogCreator;
         _importerPresenter = importerPresenter;
         AddSubPresenters(importerPresenter);

         _importerPresenter.OnTriggerImport += (s, d) =>
         {
            _results = d.DataRepositories;
            _configuration = _importerPresenter.UpdateAndGetConfiguration();
            _view.CloseView();
         };
      }

      public override bool ShouldClose => _dialogCreator.MessageBoxYesNo(Captions.ReallyCancel) == ViewResult.Yes;

      public IReadOnlyList<DataRepository> ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         ColumnInfoCache columnInfos,
         DataImporterSettings dataImporterSettings,
         string path,
         ImporterConfiguration configuration
      )
      {
         _importerPresenter.SetSettings(metaDataCategories, columnInfos, dataImporterSettings);
         _importerPresenter.LoadConfiguration(configuration, path);
         return importDataSets(configuration.Id).DataRepositories;
      }

      public void SetCaption(string caption)
      {
         _view.Caption = caption;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         ColumnInfoCache columnInfos,
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
         catch (AbstractImporterException e)
         {
            _dialogCreator.MessageBoxError(e.Message);
            return emptyImport;
         }

         return importDataSets(configurationId);
      }

      private (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) importDataSets(string configurationId)
      {
         _view.FillImporterPanel(_importerPresenter.BaseView);

         _results = Array.Empty<DataRepository>();
         _configuration = null;

         _view.Display();

         //in case we are reloading from configuration, reset the Id
         if (!string.IsNullOrEmpty(configurationId) && _configuration != null)
         {
            _configuration.Id = configurationId;
            _results.Each(r => r.ConfigurationId = configurationId);
         }

         return (_results, _configuration);
      }
   }
}