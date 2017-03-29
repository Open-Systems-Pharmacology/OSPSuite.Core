using OSPSuite.Core.Domain;

namespace OSPSuite.Helpers
{
   public class ClassifiableSimulation : Classifiable<ISimulation>
   {
      public ClassifiableSimulation() : base(ClassificationType.Simulation)
      {
      }
   }
}