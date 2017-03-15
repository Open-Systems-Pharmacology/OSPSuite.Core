using System.Drawing;
using OSPSuite.Core.Diagram;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class DiagramColors : IDiagramColors
   {
      public static Color LightBlue = Color.FromArgb(80, 160, 240);
      public static Color LightGreen = Color.FromArgb(160, 240, 80);

      public DiagramColors()
      {
         DiagramBackground = Color.White;
         BorderFixed = Color.Black; 
         BorderUnfixed = SuiteColors.Gray;

         ContainerLogical = Color.FromArgb(64, SuiteColors.Blue);
         ContainerPhysical = Color.FromArgb(64, SuiteColors.Green);

         // 1F = full color, no transparency; 0F = no color, full transparency
         ContainerOpacity = 0.15F; 

         ContainerBorder = SuiteColors.Gray;
         ContainerHandle = Color.FromArgb(128, SuiteColors.Yellow);

         // Opacity difference from NodeSize Large to Middle, Middle to Small 
         NodeSizeOpacity = 0.5F;
         PortOpacity = 0.5F;

         NeighborhoodNode = Color.Pink;
         NeighborhoodLink = Color.FromArgb(160, Color.Pink);
         NeighborhoodPort = Color.Pink;
         TransportLink = Color.FromArgb(192, Color.Pink);

         MoleculeNode = SuiteColors.DarkGreen;

         ObserverNode = SuiteColors.Yellow;
         ObserverLink = SuiteColors.Yellow;

         ReactionNode = Color.FromArgb(128, SuiteColors.Gray);
         ReactionPortEduct = SuiteColors.Blue;
         ReactionLinkEduct = SuiteColors.Blue;
         ReactionPortProduct = SuiteColors.Green;
         ReactionLinkProduct = SuiteColors.GreenDark;
         ReactionPortModifier = SuiteColors.Orange;
         ReactionLinkModifier = SuiteColors.Orange;

         JournalPageNode = LightGreen;
         JournalPageLink = SuiteColors.DarkGreen;
         JournalPagePort = SuiteColors.DarkGreen;
         RelatedItemNode = LightBlue;
         RelatedItemLink = SuiteColors.DarkBlue;
      }

      public Color DiagramBackground { get; set; }
      public Color BorderFixed { get; set; }
      public Color BorderUnfixed { get; set; }
      public Color ContainerLogical { get; set; }
      public Color ContainerPhysical { get; set; }
      public float ContainerOpacity { get; set; }
      public Color ContainerBorder { get; set; }
      public Color ContainerHandle { get; set; }
      public float NodeSizeOpacity { get; set; }
      public float PortOpacity { get; set; }
      public Color NeighborhoodNode { get; set; }
      public Color NeighborhoodLink { get; set; }
      public Color NeighborhoodPort { get; set; }
      public Color TransportLink { get; set; }
      public Color MoleculeNode { get; set; }
      public Color ObserverNode { get; set; }
      public Color ObserverLink { get; set; }
      public Color ReactionNode { get; set; }
      public Color ReactionPortEduct { get; set; }
      public Color ReactionLinkEduct { get; set; }
      public Color ReactionPortProduct { get; set; }
      public Color ReactionLinkProduct { get; set; }
      public Color ReactionPortModifier { get; set; }
      public Color ReactionLinkModifier { get; set; }
      public Color JournalPageNode { get; set; }
      public Color JournalPageLink { get; set; }
      public Color JournalPagePort { get; set; }
      public Color RelatedItemNode { get; set; }
      public Color RelatedItemLink { get; set; }
      public Color RelatedItemPort { get { return DiagramBackground; } }
   }
}
