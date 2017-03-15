namespace OSPSuite.Core.Diagram
{
   public interface IMoleculeNode : IElementBaseNode
   {
      bool IsConnectedToReactions { get; }
   }
}