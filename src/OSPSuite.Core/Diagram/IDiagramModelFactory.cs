using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Diagram
{
   public interface IDiagramModelFactory
   {
      IDiagramModel Create();
   }

   class DiagramModelFactory : DynamicFactory<IDiagramModel>, IDiagramModelFactory
   {
      public DiagramModelFactory(IContainer container) : base(container)
      {
      }
   }
}