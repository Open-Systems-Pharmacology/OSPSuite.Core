using System.Drawing.Drawing2D;
using OSPSuite.Utility.Exceptions;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Diagram.Elements
{
   public class ReactionLink : BaseLink
   {
      public ReactionLinkType Type { get; private set; }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         switch (Type)
         {
            case ReactionLinkType.Educt:
               PenColor = diagramColors.ReactionLinkEduct;
               break;
            case ReactionLinkType.Product:
               PenColor = diagramColors.ReactionLinkProduct;
               break;
            case ReactionLinkType.Modifier:
               PenColor = diagramColors.ReactionLinkModifier;
               Pen.DashStyle = DashStyle.Dash;
               break;
            default:
               throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
         }
      }

      public ReactionNode ReactionNode
      {
         get
         {
            switch (Type)
            {
               case ReactionLinkType.Educt:
                  return GetToNode() as ReactionNode;
               case ReactionLinkType.Product:
                  return GetFromNode() as ReactionNode;
               case ReactionLinkType.Modifier:
                  return GetToNode() as ReactionNode;
               default:
                  throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
            }
         }
      }

      public MoleculeNode MoleculeNode
      {
         get
         {
            switch (Type)
            {
               case ReactionLinkType.Educt:
                  return GetFromNode() as MoleculeNode;
               case ReactionLinkType.Product:
                  return GetToNode() as MoleculeNode;
               case ReactionLinkType.Modifier:
                  return GetFromNode() as MoleculeNode;
               default:
                  throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
            }
         }
         set
         {
            var moleculeNode = value;
            if (moleculeNode == null) return;
            switch (Type)
            {
               case ReactionLinkType.Educt:
                  FromPort = moleculeNode.Port;
                  break;
               case ReactionLinkType.Product:
                  ToPort = moleculeNode.Port;
                  break;
               case ReactionLinkType.Modifier:
                  FromPort = moleculeNode.Port;
                  break;
               default:
                  throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
            }
         }
      }

      public ReactionLink()
      {
         Style = GoStrokeStyle.Bezier;
      }

      public void Initialize(ReactionLinkType reactionLinkType, ReactionNode reactionNode, IMoleculeNode moleculeNode)
      {
         Type = reactionLinkType;

         switch (Type)
         {
            case ReactionLinkType.Educt:
               base.Initialize(moleculeNode, reactionNode);
               break;
            case ReactionLinkType.Product:
               base.Initialize(reactionNode, moleculeNode);
               break;
            case ReactionLinkType.Modifier:
               base.Initialize(moleculeNode, reactionNode);
               break;
            default:
               throw new OSPSuiteException("No valid ReactionLinkType = " + Type);
         }

         this.ToBack();
      }
   }
}