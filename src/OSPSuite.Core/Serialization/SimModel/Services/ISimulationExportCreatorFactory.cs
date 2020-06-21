using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public interface ISimulationExportCreatorFactory
   {
      ISimulationExportCreator Create();
   }

   internal class  SimulationExportCreatorFactory : DynamicFactory<ISimulationExportCreator>, ISimulationExportCreatorFactory
   {
      public SimulationExportCreatorFactory(IContainer container) : base(container)
      {
      }
   }
}