using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.Nodes
{
   public class GroupNode : AbstractNode<IGroup>
   {
      public GroupNode(IGroup group)
         : base(group)
      {
         Text = group.DisplayName;
      }

      public override string Id
      {
         get { return Tag.Name; }
      }
   }
}