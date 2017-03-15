namespace OSPSuite.Presentation.Nodes
{
   public class TextNode : AbstractNode
   {
      private readonly string _id;

      public TextNode(string text, string id)
      {
         Text = text;
         _id = id;
      }

      public override string Id
      {
         get { return _id; }
      }

      public override object TagAsObject
      {
         get { return Text; }
      }
   }
}