using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class MoleculeNode : ReactionDiagramNode, IMoleculeNode
   {
      public MoleculeNode()
      {
         UserFlags = NodeLayoutType.MOLECULE_NODE;
         NodeBaseSize = new SizeF(15F, 15F);
         NodeSize = NodeSize.Large;
      }

      public bool IsConnectedToReactions => Links.Any();

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetFillColorFrom(diagramColors, diagramColors.MoleculeNode);
         foreach (var link in Links.OfType<IWithColor>())
         {
            link.SetColorFrom(diagramColors);
         }
      }

      protected override ElementBaseNode CreateInstance() => new MoleculeNode();
   }
}
