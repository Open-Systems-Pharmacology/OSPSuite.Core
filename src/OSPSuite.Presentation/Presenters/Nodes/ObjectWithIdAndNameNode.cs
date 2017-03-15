using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Presenters.Nodes
{
   public class ObjectWithIdAndNameNode<TObject> : AbstractNode<TObject> where TObject : class, IWithId, IWithName
   {
      public ObjectWithIdAndNameNode(TObject objectBase)
         : base(objectBase)
      {
         UpdateText();
      }

      public override string Id
      {
         get { return Tag.Id; }
      }

      protected override void UpdateText()
      {
         Text = Tag.Name;
      }
   }
}