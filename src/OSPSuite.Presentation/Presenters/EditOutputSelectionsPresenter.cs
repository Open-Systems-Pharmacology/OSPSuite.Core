using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IEditOutputSelectionsPresenter : ISimulationSettingsItemPresenter
   {
   }

   public class EditOutputSelectionsPresenter : AbstractSubPresenter<IEditOutputSelectionsView, IEditOutputSelectionsPresenter>, IEditOutputSelectionsPresenter
   {
      private OutputSelections _outputSelection;

      public EditOutputSelectionsPresenter(IEditOutputSelectionsView view)
         : base(view)
      {
      }

      public void Edit(ISimulationSettings simulationSettings)
      {
         _outputSelection = simulationSettings.OutputSelections;
         _view.BindTo(_outputSelection.AllOutputs);
      }
   }
}