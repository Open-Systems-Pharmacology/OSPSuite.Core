using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public abstract class RootNodeBase<T> : AbstractNode<T> where T : RootNodeType
   {
      protected RootNodeBase(T rootNodeType)
         : base(rootNodeType)
      {
         Text = rootNodeType.Name;
         Icon = rootNodeType.Icon;
      }

      public override string Id
      {
         get { return Tag.Id; }
      }
   }

   public class RootNode : RootNodeBase<RootNodeType>
   {
      public RootNode(RootNodeType rootNode)
         : base(rootNode)
      {
      }
   }
}