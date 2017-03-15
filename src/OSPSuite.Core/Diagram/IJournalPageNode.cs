namespace OSPSuite.Core.Diagram
{
   public interface IJournalPageNode : IBaseNode
   {
      void CollapseRelatedItems();
      void ExpandRelatedItems();
   }
}
