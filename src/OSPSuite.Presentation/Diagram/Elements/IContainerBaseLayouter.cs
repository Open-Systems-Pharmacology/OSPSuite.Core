using System.Collections.Generic;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface IContainerBaseLayouter
   {
      void DoForceLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes, int levelDepth);
      IForceLayoutConfiguration ForceLayoutConfiguration { get; set; }
   }
}