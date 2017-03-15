using Northwoods.Go;
using OSPSuite.Assets;

namespace OSPSuite.UI.Diagram.Elements
{
   internal class MoleculePort : BasePort
   {
      public MoleculePort(MoleculeNode node, bool inverseShape) : base(node, inverseShape)
      {
      }

      public override string GetToolTip(GoView view)
      {
         var moleculeNode = Node as MoleculeNode;
         if (moleculeNode != null && moleculeNode.CanLink)
            return ToolTips.BuildingBlockReaction.HowToCreateReactionLink;

         return base.GetToolTip(view);
      }
   }
}