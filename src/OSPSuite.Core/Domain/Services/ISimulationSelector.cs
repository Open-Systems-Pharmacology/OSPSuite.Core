namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationSelector
   {
      bool SimulationCanBeUsedForIdentification(ISimulation simulation);
      bool SimulationCanBeUsedForSensitivityAnalysis(ISimulation simulation);
   }
}