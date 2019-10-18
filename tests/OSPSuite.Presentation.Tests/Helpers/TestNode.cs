using System;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Helpers
{
   public class TestNode : AbstractNode<string>
   {
      private readonly string _name;

      public TestNode() : this(string.Empty)
      {
      }

      public TestNode(string name)
         : this(Guid.NewGuid().ToString(), name)
      {
      }

      public TestNode(string id, string name) : base(id)
      {
         Id = id;
         _name = name;
         Text = _name;
      }

      public override string Id { get; }
   }
}