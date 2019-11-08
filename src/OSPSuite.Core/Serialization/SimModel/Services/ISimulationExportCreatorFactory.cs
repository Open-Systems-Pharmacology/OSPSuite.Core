using OSPSuite.Core.Services;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public interface ISimulationExportCreatorFactory
   {
      ISimulationExportCreator Create();
   }

   //TODO
   public class  SimulationExportCreatorFactory : DynamicFactory<ISimulationExportCreator>, ISimulationExportCreatorFactory
   {
   }
}