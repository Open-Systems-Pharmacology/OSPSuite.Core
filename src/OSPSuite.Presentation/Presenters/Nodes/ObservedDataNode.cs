using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public class ObservedDataNode : ObjectWithIdAndNameNode<ClassifiableObservedData>, IViewItem
   {
      public ObservedDataNode(ClassifiableObservedData observedData)
         : base(observedData)
      {
      }
   }
}