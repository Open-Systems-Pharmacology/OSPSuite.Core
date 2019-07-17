using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Helpers
{
   public class SimulationNode : ObjectWithIdAndNameNode<ClassifiableSimulation>
   {
      public SimulationNode(ClassifiableSimulation tag) : base(tag)
      {
      }
   }
}