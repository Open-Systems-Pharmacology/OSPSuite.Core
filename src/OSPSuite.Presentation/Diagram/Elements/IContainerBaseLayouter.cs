using System.Collections.Generic;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface IContainerBaseLayouter
   {
      void PlaceFreeNodes(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes);
   }
}