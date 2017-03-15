using System.Collections.Generic;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public interface IBaseForceLayout
   {
      void PerformLayout(IContainerBase containerBase, IList<IHasLayoutInfo> freeNodes);
      IForceLayoutConfiguration Config { get; set; }
   }
}