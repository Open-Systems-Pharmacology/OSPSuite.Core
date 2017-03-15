using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IEditDisplayUnitsPresenter : IPresenter<IEditDisplayUnitsView>, IDisposablePresenter
   {
      void Edit(DisplayUnitsManager displayUnitsManager, string displayUnitsType);
   }

   public class EditDisplayUnitsPresenter : AbstractDisposableContainerPresenter<IEditDisplayUnitsView, IEditDisplayUnitsPresenter>, IEditDisplayUnitsPresenter
   {
      private readonly ICloneManagerForModel _cloner;
      private readonly IDisplayUnitsPresenter _displayUnitsPresenter;
      private DisplayUnitsManager _displayUnitsManagerToEdit;
      private DisplayUnitsManager _clonedUnitManager;

      public EditDisplayUnitsPresenter(IEditDisplayUnitsView view, ICloneManagerForModel cloner, IDisplayUnitsPresenter displayUnitsPresenter)
         : base(view)
      {
         _cloner = cloner;
         _displayUnitsPresenter = displayUnitsPresenter;
         view.AddUnitsView(_displayUnitsPresenter.View);
         AddSubPresenters(_displayUnitsPresenter);
      }

      public void Edit(DisplayUnitsManager displayUnitsManager, string displayUnitsType)
      {
         _displayUnitsManagerToEdit = displayUnitsManager;
         _clonedUnitManager = _cloner.Clone(_displayUnitsManagerToEdit);
         _displayUnitsPresenter.Edit(_clonedUnitManager);
         _view.Caption = Captions.ManageDisplayUnits(displayUnitsType);
         _view.Display();
         if (_view.Canceled) return;
         // User confirms changes=> Update
         _displayUnitsManagerToEdit.UpdatePropertiesFrom(_clonedUnitManager, _cloner);
      }

      public override void ViewChanged()
      {
         _view.OkEnabled = CanClose;
      }
   }
}