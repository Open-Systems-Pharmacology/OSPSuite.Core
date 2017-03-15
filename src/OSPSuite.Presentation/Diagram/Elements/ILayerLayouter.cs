using System.Collections.Generic;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface ILayerLayouter
   {
      void PerformLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes);
   }
}