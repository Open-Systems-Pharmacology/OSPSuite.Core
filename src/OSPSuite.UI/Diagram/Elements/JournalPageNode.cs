using System;
using System.Drawing;
using System.Linq;
using Northwoods.Go;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;

namespace OSPSuite.UI.Diagram.Elements
{
   public class JournalPageNode : ElementBaseNode, IGoCollapsible, IJournalPageNode
   {
      private const string PARENT_PORT = "parentPort";
      private const string CHILD_PORT = "childPort";
      private const string RELATED_ITEM_PORT = "relatedItemPort";

      public static readonly SizeF NewNodeSize = new SizeF(120, 65);
      private DateTimeFormatter _dateTimeFormatter;
      public const int CHANGED_COLLAPSIBLE = 1235;
      public const int CHANGED_EXPANDED = 1236;

      public int UniqueIndex { get; set; }

      public JournalPageNode()
      {
         IsExpanded = true;
         Collapsible = true;
         ConstructShape();

         relatedItemPort().AddObserver(this);
         var goCollapsibleHandle = new GoCollapsibleHandle
         {
            BrushColor = Color.Transparent,
            Bordered = false,
            Size = new SizeF(6, 6)
         };

         relatedItemPort().Size = new SizeF(goCollapsibleHandle.Size.Width * 2, goCollapsibleHandle.Size.Width * 2);
         relatedItemPort().SetSpotLocation(Middle, getRelatedItemPortLocation());
         goCollapsibleHandle.Center = calculateHandlePosition();
         Add(goCollapsibleHandle);
         UpdateHandle();
         configureLabel();
      }

      private JournalItemPort parentPort()
      {
         return FindChild<JournalItemPort>(PARENT_PORT);
      }

      private JournalItemPort childPort()
      {
         return FindChild<JournalItemPort>(CHILD_PORT);
      }

      private JournalItemPort relatedItemPort()
      {
         return FindChild<JournalItemPort>(RELATED_ITEM_PORT);
      }

      private void configureLabel()
      {
         LabelSpot = MiddleTop;
         Label.TextColor = Color.Black;
         Label.Multiline = true;
         Label.Wrapping = true;
         Label.AutoResizes = false;
         Label.Width = Shape.Width - 8;
         //must be after addPorts, otherwise WrappingWidth is reset
         Label.WrappingWidth = Label.Width;
      }

      public override void SetColorFrom(IDiagramColors diagramColors)
      {
         base.SetColorFrom(diagramColors);
         Shape.BrushColor = diagramColors.JournalPageNode;

         relatedItemPort().BrushColor = diagramColors.DiagramBackground;

         parentPort().BrushColor = diagramColors.JournalPagePort;
         childPort().BrushColor = diagramColors.JournalPagePort;

         foreach (var link in GetLinks<IJournalPageLink>()) link.SetColorFrom(diagramColors);
      }

      public override void LayoutChildren(GoObject childchanged)
      {
         if (Initializing) return;
         base.LayoutChildren(childchanged);

         if (Label == null || Shape == null) return;
         var p = Shape.GetSpotLocation(LabelSpot);
         p.Y += 5;
         Label.SetSpotLocation(LabelSpot, p);
      }

      private PointF calculateHandlePosition()
      {
         return relatedItemPort().Center;
      }

      protected virtual void ConstructShape()
      {
         var goRectangle = new GoRoundedRectangle { Corner = new SizeF(4, 4) };
         _dateTimeFormatter = new DateTimeFormatter(displayTime: false);
         Shape = goRectangle;

         NodeBaseSize = NewNodeSize;
         NodeSize = NodeSize.Middle;
         addPorts();
         movePorts();
      }

      private void addPorts()
      {
         Port = null;

         var journalParentPort = new JournalParentPort();
         Add(journalParentPort);
         AddChildName(PARENT_PORT, journalParentPort);


         var journalChildPort = new JournalChildPort();
         Add(journalChildPort);
         AddChildName(CHILD_PORT, journalChildPort);

         var itemPort = new RelatedItemPort();
         Add(itemPort);
         AddChildName(RELATED_ITEM_PORT, itemPort);
      }

      private void movePorts()
      {
         parentPort().SetSpotLocation(Middle, getParentPortLocation());
         childPort().SetSpotLocation(Middle, getChildPortLocation());
      }

      public PointF GetNextRelatedItemLocation(RelatedItemNode lowestRelatedItemNode)
      {
         float belowThis;
         if (lowestRelatedItemNode != null)
            belowThis = Shape.Bottom > lowestRelatedItemNode.Bottom? Shape.Bottom : lowestRelatedItemNode.Bottom;
         else
            belowThis = Shape.Bottom;
         return new PointF(nearLeft(), belowThis + (RelatedItemNode.RelatedItemNodeSize.Height + 10));
      }

      private PointF getRelatedItemPortLocation()
      {
         return new PointF(nearLeft(), Shape.Bottom);
      }

      private PointF getChildPortLocation()
      {
         return new PointF(Shape.Right, midHeight());
      }

      private PointF getParentPortLocation()
      {
         return new PointF(Shape.Left, midHeight());
      }

      private float nearLeft()
      {
         return roundValueUpToNearest((9 * Shape.Left + Shape.Right) / 10, 10);
      }

      private float roundValueUpToNearest(float value, float nearest)
      {
         return (float)(Math.Round(value / nearest + 0.5) * nearest);
      }

      private float midHeight()
      {
         return roundValueUpToNearest((Shape.Top + Shape.Bottom) / 2, 10);
      }

      public override GoObject CopyObject(GoCopyDictionary env)
      {
         var copy = (JournalPageNode)base.CopyObject(env);
         return copy;
      }

      public override void AddLinkFrom(IBaseLink link)
      {
         var relatedItemLink = link as RelatedItemLink;
         if (relatedItemLink == null)
            childPort().AddSourceLink((IGoLink)link);
         else
            relatedItemPort().AddDestinationLink((IGoLink)link);
      }

      public override void AddLinkTo(IBaseLink link)
      {
         parentPort().AddDestinationLink((IGoLink)link);
      }

      public void UpdateAttributesFrom(JournalPage page)
      {
         UniqueIndex = page.UniqueIndex;
         Label.Text = string.Format("{0}{1}{1}({2})             {3}{1}", page.Title, Environment.NewLine, page.UniqueIndex, _dateTimeFormatter.Format(page.CreatedAt));
         configureLabel();
      }

      public void ClearParentLinks()
      {
         parentPort().ClearLinks();
      }

      public virtual bool HasChildren()
      {
         return Collapsible && relatedItemPort().DestinationLinksCount > 0;
      }

      protected void UpdateHandle()
      {
         foreach (var h in this.OfType<GoCollapsibleHandle>())
         {
            h.Visible = HasChildren();
            h.Printable = h.Visible;
            relatedItemPort().Visible = h.Visible;
            break;
         }
      }

      private void setExpanded(bool e)
      {
         var old = IsExpanded;
         if (old == e) return;
         IsExpanded = e;
         Changed(CHANGED_EXPANDED, 0, old, NullRect, 0, e, NullRect);
         UpdateHandle();
      }

      public virtual bool HasAnyChildrenUnseen()
      {
         return Collapsible && Destinations.Any(n => !n.GoObject.CanView());
      }

      protected override void OnObservedChanged(GoObject observed, int subhint,
                                                int oldI, Object oldVal, RectangleF oldRect,
                                                int newI, Object newVal, RectangleF newRect)
      {
         base.OnObservedChanged(observed, subhint, oldI, oldVal, oldRect, newI, newVal, newRect);
         if (observed != relatedItemPort() || (subhint != GoPort.ChangedAddedLink && subhint != GoPort.ChangedRemovedLink)) return;
         if (!HasAnyChildrenUnseen() && IsExpanded)
            Expand();
         else
            Collapse();
         UpdateHandle();
      }

      public void CollapseRelatedItems()
      {
         Collapse();
      }

      public void ExpandRelatedItems()
      {
         Expand();
      }

      public virtual void Collapse()
      {
         if (!Collapsible) return;
         setExpanded(false);

         foreach (var relatedItemNode in Destinations.Cast<GoObject>().Where(obj => obj != this).OfType<RelatedItemNode>())
         {
            setVisibility(relatedItemNode, visible: false);
            setSourceLinkVisibility(relatedItemNode, visible: false);
         }
      }

      public virtual void Expand()
      {
         if (!Collapsible) return;
         setExpanded(true);

         foreach (var relatedItemNode in Destinations.Cast<GoObject>().Where(obj => obj != this).OfType<RelatedItemNode>())
         {
            setVisibility(relatedItemNode, visible: true);
            setSourceLinkVisibility(relatedItemNode, visible: true);
         }
      }

      private static void setVisibility(RelatedItemNode relatedItemNode, bool visible)
      {
         relatedItemNode.Visible = visible;
         relatedItemNode.Printable = visible;
      }

      private static void setSourceLinkVisibility(RelatedItemNode relatedItemNode, bool visible)
      {
         foreach (var inlink in relatedItemNode.SourceLinks)
         {
            inlink.GoObject.Visible = visible;
            inlink.GoObject.Printable = visible;
         }
      }

      public bool Collapsible { get; set; }

      private bool _isExpanded;
      public bool IsExpanded
      {
         get { return _isExpanded; }
         private set
         {
            _isExpanded = value;
            if (_isExpanded)
               Expand();
            else
               Collapse();
         }
      }
   }
}