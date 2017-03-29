using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Helpers
{
   public class SimulationNode : ObjectWithIdAndNameNode<ClassifiableSimulation>
   {
      public SimulationNode(ClassifiableSimulation tag) : base(tag)
      {
      }
   }
}