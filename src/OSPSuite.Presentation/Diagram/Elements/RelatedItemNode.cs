using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class RelatedItemNode : ElementBaseNode, IRelatedItemNode
   {
      public static readonly SizeF RelatedItemNodeSize = new SizeF(20, 20);
      private const int MAXIMUM_TITLE_LENGTH = 32;
      private const int ELLIPSIS_LENGTH = 3;

      public RelatedItemNode()
      {
         NodeBaseSize = RelatedItemNodeSize;
         NodeSize = NodeSize.Middle;
         CanLink = false;
      }

      public override Color LabelColor => Color.Black;

      public JournalPageNode PageNode => Links.OfType<RelatedItemLink>().FirstOrDefault()?.PageNode;

      public override bool Visible
      {
         get => base.Visible && (PageNode?.IsExpanded ?? true);
         set => base.Visible = value;
      }

      public void UpdateAttributesFromItem(RelatedItem item)
      {
         var maxNameLength = MAXIMUM_TITLE_LENGTH - item.ItemType.Length;
         var name = item.Name.Length > maxNameLength ? shortenedName(item.Name, maxNameLength) : item.Name;
         Name = $"  {name} ({item.ItemType})";
      }

      private static string shortenedName(string name, int maxNameLength)
      {
         return maxNameLength < ELLIPSIS_LENGTH ? string.Empty : name.Substring(0, maxNameLength - ELLIPSIS_LENGTH) + "...";
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         FillColor = diagramColors.RelatedItemNode;
         PortColor = diagramColors.RelatedItemNode;
         Links.OfType<RelatedItemLink>().Each(link => link.SetColorFrom(diagramColors));
      }

      protected override ElementBaseNode CreateInstance() => new RelatedItemNode();
   }

   public class RelatedItemLink : BaseLink
   {
      public JournalPageNode PageNode => GetFromNode() as JournalPageNode;

      public RelatedItemNode ItemNode => GetToNode() as RelatedItemNode;

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         Color = diagramColors.RelatedItemLink;
         NotifyChanged();
      }
   }
}
