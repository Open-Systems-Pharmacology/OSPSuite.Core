using OSPSuite.Core.Diagram;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class ReactionLink : BaseLink
   {
      public ReactionLinkType Type { get; private set; }

      public ReactionNode ReactionNode => Type == ReactionLinkType.Product ? GetFromNode() as ReactionNode : GetToNode() as ReactionNode;

      public MoleculeNode MoleculeNode => Type == ReactionLinkType.Product ? GetToNode() as MoleculeNode : GetFromNode() as MoleculeNode;

      public void Initialize(ReactionLinkType reactionLinkType, ReactionNode reactionNode, IMoleculeNode moleculeNode)
      {
         Type = reactionLinkType;

         switch (Type)
         {
            case ReactionLinkType.Educt:
            case ReactionLinkType.Modifier:
               Initialize(moleculeNode, reactionNode);
               break;
            case ReactionLinkType.Product:
               Initialize(reactionNode, moleculeNode);
               break;
            default:
               throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
         }
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         switch (Type)
         {
            case ReactionLinkType.Educt:
               Color = diagramColors.ReactionLinkEduct;
               IsDashed = false;
               break;
            case ReactionLinkType.Product:
               Color = diagramColors.ReactionLinkProduct;
               IsDashed = false;
               break;
            case ReactionLinkType.Modifier:
               Color = diagramColors.ReactionLinkModifier;
               IsDashed = true;
               break;
            default:
               throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
         }

         NotifyChanged();
      }
   }
}
