using System;
using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface IModalImporterPresenter : IDisposablePresenter
   {
      (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IImporterPresenter presenter,
         string configurationId = null
      );
   }

   public class ModalImporterPresenter : AbstractDisposablePresenter<IModalImporterView, IModalImporterPresenter>, IModalImporterPresenter
   {
      private readonly IImporterPresenter _importerPresenter;

      public ModalImporterPresenter(IModalImporterView view, IImporterPresenter importerPresenter) : base(view)
      {
         _importerPresenter = importerPresenter;
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IImporterPresenter presenter,
         string configurationId = null
      )
      {
         IReadOnlyList<DataRepository> results = Array.Empty<DataRepository>();
         _view.FillImporterPanel(presenter.BaseView);
         var configuration = presenter.UpdateAndGetConfiguration();

         presenter.OnTriggerImport += (s, d) =>
         {
            results = d.DataRepositories;
            configuration = presenter.UpdateAndGetConfiguration();
         };

         if (!string.IsNullOrEmpty(configurationId))
         {
            configuration.Id = configurationId;
            results.Each(r => r.ConfigurationId = configurationId);
         }

         _view.AttachImporterPresenter(presenter);
         _view.Display();
         return (results, configuration);
      }
   }
}