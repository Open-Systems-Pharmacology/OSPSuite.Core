using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public class ClassificationNode : ObjectWithIdAndNameNode<IClassification>, IViewItem
   {
      public ClassificationNode(IClassification classification)
         : base(classification)
      {
      }

      public override string ToString()
      {
         return Text;
      }
   }
}