using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Utility.Extensions;

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
         Links.OfType<IWithColor>().Each(link => link.SetColorFrom(diagramColors));
      }

      protected override ElementBaseNode CreateInstance() => new MoleculeNode();
   }
}
