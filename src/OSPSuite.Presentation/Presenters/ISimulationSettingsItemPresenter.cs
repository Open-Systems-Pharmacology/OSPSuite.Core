using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Presentation.Presenters
{
   public interface ISimulationSettingsItemPresenter : ISubPresenter
   {
      void Edit(ISimulationSettings simulationSettings);
   }
}