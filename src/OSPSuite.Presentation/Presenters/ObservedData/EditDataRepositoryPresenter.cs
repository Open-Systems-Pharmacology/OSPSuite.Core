using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IEditDataRepositoryPresenter : ISingleStartPresenter<DataRepository>
   {
   }

   public class EditDataRepositoryPresenter : SingleStartContainerPresenter<IEditDataRepositoryView, IEditDataRepositoryPresenter, DataRepository, IDataRepositoryItemPresenter>, IEditDataRepositoryPresenter
   {
      private DataRepository repository { get; set; }

      public EditDataRepositoryPresenter(IEditDataRepositoryView view, ISubPresenterItemManager<IDataRepositoryItemPresenter> subPresenterItemManager)
         : base(view, subPresenterItemManager, ObservedDataItems.All)
      {
      }

      protected override void UpdateCaption()
      {
         _view.Caption = repository.Name;
      }

      public override object Subject
      {
         get { return repository; }
      }

      public override void Edit(DataRepository repositoryToEdit)
      {
         repository = repositoryToEdit;
         _subPresenterItemManager.AllSubPresenters.Each(presenter => presenter.EditObservedData(repositoryToEdit));

         UpdateCaption();
         _view.Display();
      }
   }
}