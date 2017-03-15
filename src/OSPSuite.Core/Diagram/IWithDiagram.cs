namespace OSPSuite.Core.Diagram
{
   public interface IWithDiagramModel
   {
      IDiagramModel DiagramModel { get; set; }
   }

   public interface IWithDiagramFor<TModel> : IWithDiagramModel where TModel : IWithDiagramFor<TModel>
   {
      IDiagramManager<TModel> DiagramManager { get; set; }
   }
}