using OSPSuite.Core.Services;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public interface ISimulationExportCreatorFactory
   {
      ISimulationExportCreator Create();
   }

   internal class  SimulationExportCreatorFactory : DynamicFactory<ISimulationExportCreator>, ISimulationExportCreatorFactory
   {
   }
}