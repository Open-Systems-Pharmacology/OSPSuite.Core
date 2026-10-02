using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Diagram.Elements
{
   public class JournalPageNode : ElementBaseNode, IJournalPageNode
   {
      public static readonly SizeF NewNodeSize = new SizeF(120, 65);
      public const float RELATED_ITEM_RELATIVE_X = 20F / 120F;
      private const float RELATED_ITEM_SPACING = 10F;
      private static readonly DateTimeFormatter _dateTimeFormatter = new DateTimeFormatter(displayTime: false);
      private bool _isExpanded = true;

      public int UniqueIndex { get; set; }
      public string Text { get; private set; } = string.Empty;

      public JournalPageNode()
      {
         NodeBaseSize = NewNodeSize;
         NodeSize = NodeSize.Middle;
      }

      public bool IsExpanded
      {
         get => _isExpanded;
         set => SetField(ref _isExpanded, value);
      }

      public override bool LabelVisible => false;

      public override Color LabelColor => Color.Black;

      public IReadOnlyList<RelatedItemNode> RelatedItemNodes => Links.OfType<RelatedItemLink>().Select(link => link.ItemNode).Where(node => node != null).ToList();

      public bool HasRelatedItems => RelatedItemNodes.Any();

      public JournalPageNode ParentPageNode => Links.OfType<JournalPageLink>().FirstOrDefault(link => link.ChildNode == this)?.ParentNode;

      public void CollapseRelatedItems() => IsExpanded = false;

      public void ExpandRelatedItems() => IsExpanded = true;

      public void UpdateAttributesFrom(JournalPage page)
      {
         UniqueIndex = page.UniqueIndex;
         Name = page.Title;
         Text = $"{page.Title}{Environment.NewLine}{Environment.NewLine}({page.UniqueIndex})             {_dateTimeFormatter.Format(page.CreatedAt)}{Environment.NewLine}";
         NotifyChanged();
      }

      public void ClearParentLinks()
      {
         Links.OfType<JournalPageLink>().Where(link => link.ChildNode == this).ToList().Each(link => link.Unlink());
      }

      public PointF GetNextRelatedItemLocation(RelatedItemNode lowestRelatedItemNode)
      {
         var bounds = Bounds;
         var belowThis = Math.Max(bounds.Bottom, lowestRelatedItemNode?.Bounds.Bottom ?? bounds.Bottom);
         return new PointF(bounds.Left + RELATED_ITEM_RELATIVE_X * bounds.Width, belowThis + RelatedItemNode.RelatedItemNodeSize.Height + RELATED_ITEM_SPACING);
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         FillColor = diagramColors.JournalPageNode;
         PortColor = diagramColors.JournalPagePort;
         Links.OfType<JournalPageLink>().Each(link => link.SetColorFrom(diagramColors));
      }

      protected override ElementBaseNode CreateInstance() => new JournalPageNode();

      protected override void CopyPropertiesTo(ElementBaseNode target)
      {
         base.CopyPropertiesTo(target);
         if (!(target is JournalPageNode page))
            return;
         page.UniqueIndex = UniqueIndex;
         page.Text = Text;
         page.IsExpanded = IsExpanded;
      }
   }

   public class JournalPageLink : BaseLink, IJournalPageLink
   {
      public JournalPageNode ParentNode => GetFromNode() as JournalPageNode;

      public JournalPageNode ChildNode => GetToNode() as JournalPageNode;

      public override bool IsCurved => true;

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         Color = diagramColors.JournalPageLink;
         NotifyChanged();
      }
   }
}
