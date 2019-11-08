using OSPSuite.Core.Services;

namespace OSPSuite.Core.Diagram
{
   public interface IDiagramModelFactory
   {
      IDiagramModel Create();
   }

   class DiagramModelFactory : DynamicFactory<IDiagramModel>, IDiagramModelFactory
   {
   }
}