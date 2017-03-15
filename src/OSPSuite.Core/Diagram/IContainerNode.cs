using System.Collections.Generic;

namespace OSPSuite.Core.Diagram
{
   public interface IContainerNode : IBaseNode, IContainerBase
   {
      bool IsLogical { get; set; }
      bool IsExpanded { get; set; }
      bool IsExpandedByDefault { get; set; }
      void ShowChildrenAndLinkedNodes();
      IEnumerable<T> GetLinkedNodes<T>(bool recursive) where T : class, IBaseNode;
   }
}