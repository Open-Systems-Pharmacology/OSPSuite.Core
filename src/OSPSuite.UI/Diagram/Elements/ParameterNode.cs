using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ParameterNode : ElementBaseNode
   {
      public ParameterNode()
      {
         UserFlags = 3;
         
         LabelSpot = MiddleRight;

         NodeBaseSize = new SizeF(10F, 10F);
         NodeSize = NodeSize.Middle;

         Port.IsValidFrom = false;
         Port.IsValidTo = false;

         Text = "";
      }
   }
}
