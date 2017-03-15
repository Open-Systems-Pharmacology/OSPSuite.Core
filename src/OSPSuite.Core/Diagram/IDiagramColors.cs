using System.Drawing;

namespace OSPSuite.Core.Diagram
{
   public interface IDiagramColors
   {
      Color DiagramBackground { get; set; } 

      Color BorderFixed { get; set; } 
      Color BorderUnfixed { get; set; } 

      Color ContainerLogical { get; set; } 
      Color ContainerPhysical { get; set; } 
      float ContainerOpacity { get; set; }
      Color ContainerBorder { get; set; } 
      Color ContainerHandle { get; set; } 

      // Opacity difference from NodeSize Large to Middle, Middle to Small 
      float NodeSizeOpacity { get; set; }
      float PortOpacity { get; set; }

      Color NeighborhoodNode { get; set; } 
      Color NeighborhoodLink { get; set; } 
      Color NeighborhoodPort { get; set; } 
      Color TransportLink { get; set; } 

      Color MoleculeNode { get; set; } 

      Color ObserverNode { get; set; } 
      Color ObserverLink { get; set; } 

      Color ReactionNode { get; set; } 
      Color ReactionPortEduct { get; set; } 
      Color ReactionLinkEduct { get; set; } 
      Color ReactionPortProduct { get; set; } 
      Color ReactionLinkProduct { get; set; } 
      Color ReactionPortModifier { get; set; } 
      Color ReactionLinkModifier { get; set; }

      Color JournalPageNode { get; set; }
      Color JournalPageLink { get; set; }
      Color JournalPagePort { get; set; }
      Color RelatedItemNode { get; set; }
      Color RelatedItemLink { get; set; }
      Color RelatedItemPort { get; }
   }
}