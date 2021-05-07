using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;
using System.Collections.Generic;
using ImporterConfiguration = OSPSuite.Core.Import.ImporterConfiguration;

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
      public ModalImporterPresenter(IModalImporterView view) : base(view)
      {
      }

      public (IReadOnlyList<DataRepository> DataRepositories, ImporterConfiguration Configuration) ImportDataSets(
         IImporterPresenter presenter, 
         string configurationId = null
      )
      {
         IReadOnlyList<DataRepository> result = null;
         _view.FillImporterPanel(presenter.BaseView);

         presenter.OnTriggerImport += (s, d) =>
         {
            result = d.DataRepositories;
         };
         var configuration = presenter.UpdateAndGetConfiguration();
         if (!string.IsNullOrEmpty(configurationId))
         {
            configuration.Id = configurationId;
            result.Each(r => r.ConfigurationId = configurationId);
         }
         _view.AttachImporterPresenter(presenter);
         _view.Display();
         return (result, configuration);
      }
   }
}
