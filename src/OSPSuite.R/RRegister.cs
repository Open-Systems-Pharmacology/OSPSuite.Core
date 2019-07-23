using OSPSuite.R.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.R
{
   public class RRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.Register<ISimulationLoader, SimulationLoader>();
         container.Register<IContainerTask, ContainerTask>();
      }
   }
}