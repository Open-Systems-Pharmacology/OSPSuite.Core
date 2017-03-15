namespace OSPSuite.Core.Diagram
{
   public interface IBaseLink : IBaseObject
   {
      IBaseNode GetOtherNode(IBaseNode node);
      IBaseNode GetFromNode();
      IBaseNode GetToNode();
      void Initialize(IBaseNode fromNode, IBaseNode toNode);
      bool IsVisible { get; set; }
      void Unlink();
   }
}