namespace OSPSuite.Core.Diagram.Extensions
{
   public static class WithDiagramModelExtensions
   {
      public static void InitializeDiagramManager<T>(this T withDiagramModel, IDiagramOptions diagramOptions) where T : IWithDiagramFor<T>
      {
         withDiagramModel.DiagramManager.InitializeWith(withDiagramModel, diagramOptions);
      }
   }
}