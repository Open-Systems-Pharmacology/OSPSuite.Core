using System.Drawing;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Diagram;

namespace OSPSuite.UI.Diagram.Elements
{
   public class MoleculeNode : ReactionDiagramNode, IMoleculeNode
   {
      public MoleculeNode()
      {
         Port = new MoleculePort(this, true);
         UserFlags = NodeLayoutType.MOLECULE_NODE;
         LabelSpot = MiddleRight;
         NodeBaseSize = new SizeF(15F, 15F);
         NodeSize = NodeSize.Large;
         Text = string.Empty;
         ((BasePort) Port).SetLinkValidator(isValidLinkFromMoleculeNode);
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         SetBrushColor(diagramColors, diagramColors.MoleculeNode);
         foreach (var transportLink in GetLinks<TransportLink>())
         {
            transportLink.SetColorFrom(diagramColors);
         }
      }

      private bool isValidLinkFromMoleculeNode(IBaseNode node1, IBaseNode node2, object obj1, object obj2)
      {
         return node2.IsAnImplementationOf<ReactionNode>();
      }

      public void AddLink(TransportLink link)
      {
         Port.AddSourceLink(link);
      }

      public bool IsConnectedToReactions => Nodes.Count > 0;
   }
}