using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraDiagram;
using OSPSuite.Core.Diagram;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Views.Journal;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views.Diagram;

namespace OSPSuite.UI.Views.Journal
{
   public class JournalDiagramView : DevExpressDiagramView, IJournalDiagramView
   {
      private const float PORT_DOT_SIZE = 6F;
      private const float HANDLE_SIZE = 12F;
      private const float HANDLE_HIT_SIZE = 8F;
      private const float HANDLE_SIGN_INSET = 2F;
      private const float TEXT_PADDING = 4F;
      private const float TEXT_TOP = 5F;
      private static readonly SizeF _gridSize = new SizeF(10, 10);
      private IJournalDiagramPresenter _journalDiagramPresenter;

      public JournalDiagramView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
         _diagramControl.OptionsView.GridSize = _gridSize;
      }

      public void AttachPresenter(IJournalDiagramPresenter presenter)
      {
         base.AttachPresenter(presenter);
         _journalDiagramPresenter = presenter;
      }

      public List<IBaseObject> GetSelection() => GetSelectedNodes<IBaseObject>().ToList();

      public void RemoveSelectionHandles() => ClearSelection();

      protected override ShapeDescription ShapeFor(ElementBaseNode node) => node is JournalPageNode ? BasicShapes.Rectangle : base.ShapeFor(node);

      protected override PointCollection ConnectionPointsFor(ElementBaseNode node)
      {
         switch (node)
         {
            case JournalPageNode _:
               return JournalConnectionPoints.PagePoints;
            case RelatedItemNode _:
               return JournalConnectionPoints.RelatedItemPoints;
            default:
               return base.ConnectionPointsFor(node);
         }
      }

      protected override int PointIndexFor(IBaseLink link, IBaseNode node)
      {
         switch (link)
         {
            case JournalPageLink pageLink:
               return node == pageLink.ParentNode ? JournalConnectionPoints.CHILD_INDEX : JournalConnectionPoints.PARENT_INDEX;
            case RelatedItemLink itemLink:
               return node == itemLink.PageNode ? JournalConnectionPoints.RELATED_ITEM_INDEX : 0;
            default:
               return base.PointIndexFor(link, node);
         }
      }

      protected override object PortFor(ElementBaseNode node, int pointIndex, PointFloat point)
      {
         if (!(node is JournalPageNode))
            return base.PortFor(node, pointIndex, point);
         return JournalConnectionPoints.PortFor(pointIndex) ?? (point.X < node.Location.X ? JournalPort.Parent : JournalPort.Child);
      }

      protected override bool IsOverPortOf(ElementBaseNode node, PointF point)
      {
         if (!(node is JournalPageNode))
            return base.IsOverPortOf(node, point);
         return new[] {JournalConnectionPoints.PagePoints[JournalConnectionPoints.PARENT_INDEX], JournalConnectionPoints.PagePoints[JournalConnectionPoints.CHILD_INDEX]}
            .Any(relativePoint => Distance(point, AbsolutePoint(node, relativePoint)) <= PORT_DOT_SIZE);
      }

      protected override bool CanConnect(ElementBaseNode node, ElementBaseNode oppositeNode)
      {
         return node is JournalPageNode && (oppositeNode == null || (oppositeNode is JournalPageNode && oppositeNode != node));
      }

      protected override bool IsValidConnection(ElementBaseNode node, int pointIndex, ElementBaseNode oppositeNode, int oppositePointIndex)
      {
         if (!(node is JournalPageNode page))
            return false;
         if (oppositeNode == null)
            return true;
         if (!(oppositeNode is JournalPageNode otherPage) || otherPage == page)
            return false;

         var port = JournalConnectionPoints.PortFor(pointIndex);
         var oppositePort = JournalConnectionPoints.PortFor(oppositePointIndex);
         if (port == null || oppositePort == null || port == oppositePort || port == JournalPort.RelatedItem || oppositePort == JournalPort.RelatedItem)
            return false;

         var child = port == JournalPort.Parent ? page : otherPage;
         return child.ParentPageNode == null;
      }

      protected override void OnLinkCreated(IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort)
      {
         var parent = Equals(fromPort, JournalPort.Child) ? fromNode : toNode;
         var child = parent == fromNode ? toNode : fromNode;
         _journalDiagramPresenter.AddParentLink(child, parent);
      }

      protected override void OnSelectionDeleting(IReadOnlyList<IBaseNode> nodes, IReadOnlyList<IBaseLink> links)
      {
         _journalDiagramPresenter.DeleteSelection();
      }

      protected override void OnNodeDoubleClicked(IBaseNode node)
      {
         if (node is IJournalPageNode journalPageNode)
            _journalDiagramPresenter.EditJournalPage(journalPageNode);
      }

      protected override void ShowContextMenu(IBaseNode node, Point location, PointF documentPoint)
      {
         if (node == null)
         {
            _journalDiagramPresenter.ShowContextMenu(new JournalDiagramBackground(), location);
            return;
         }

         var selectedNodes = GetSelectedNodes<IBaseNode>().ToList();
         _journalDiagramPresenter.ShowContextMenu(selectedNodes.Contains(node) ? selectedNodes : new List<IBaseNode> {node}, location);
      }

      protected override IEnumerable<IBaseNode> NodesMovingWith(IBaseNode node) => node is JournalPageNode page ? page.RelatedItemNodes : Enumerable.Empty<IBaseNode>();

      protected override void OnNodeClicked(IBaseNode node, PointF documentPoint, Keys modifiers)
      {
         if (node is JournalPageNode page && page.HasRelatedItems && handleBounds(page).Contains(documentPoint))
         {
            ChangeLayout(() => page.IsExpanded = !page.IsExpanded, page.IsExpanded ? "Collapse" : "Expand");
            return;
         }

         base.OnNodeClicked(node, documentPoint, modifiers);
      }

      protected override void DrawNode(Graphics graphics, ElementBaseNode node, SizeF size)
      {
         if (!(node is JournalPageNode page))
         {
            base.DrawNode(graphics, node, size);
            return;
         }

         using (var font = new Font(SystemFonts.DefaultFont.FontFamily, page.LabelFontSize))
         using (var textBrush = new SolidBrush(page.LabelColor))
         using (var portBrush = new SolidBrush(page.PortColor))
         using (var format = new StringFormat(StringFormat.GenericTypographic) {Trimming = StringTrimming.EllipsisWord, Alignment = StringAlignment.Center})
         {
            graphics.DrawString(page.Text, font, textBrush, new RectangleF(TEXT_PADDING, TEXT_TOP, size.Width - 2 * TEXT_PADDING, size.Height - TEXT_TOP), format);
            drawDot(graphics, portBrush, JournalConnectionPoints.PagePoints[JournalConnectionPoints.PARENT_INDEX], size);
            drawDot(graphics, portBrush, JournalConnectionPoints.PagePoints[JournalConnectionPoints.CHILD_INDEX], size);
         }

         if (page.HasRelatedItems)
            drawHandle(graphics, page, size);
      }

      private static void drawDot(Graphics graphics, Brush brush, PointFloat relativePoint, SizeF size)
      {
         graphics.FillEllipse(brush, relativePoint.X * size.Width - PORT_DOT_SIZE / 2, relativePoint.Y * size.Height - PORT_DOT_SIZE / 2, PORT_DOT_SIZE, PORT_DOT_SIZE);
      }

      private void drawHandle(Graphics graphics, JournalPageNode page, SizeF size)
      {
         var center = new PointF(JournalConnectionPoints.RELATED_ITEM_RELATIVE_X * size.Width, size.Height);
         var rectangle = new RectangleF(center.X - HANDLE_SIZE / 2, center.Y - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
         using (var background = new SolidBrush(_diagramControl.BackColor))
         using (var pen = new Pen(page.LabelColor))
         {
            graphics.FillEllipse(background, rectangle);
            graphics.DrawEllipse(pen, rectangle);
            graphics.DrawLine(pen, rectangle.X + HANDLE_SIGN_INSET, center.Y, rectangle.Right - HANDLE_SIGN_INSET, center.Y);
            if (!page.IsExpanded)
               graphics.DrawLine(pen, center.X, rectangle.Y + HANDLE_SIGN_INSET, center.X, rectangle.Bottom - HANDLE_SIGN_INSET);
         }
      }

      private static RectangleF handleBounds(JournalPageNode page)
      {
         var center = AbsolutePoint(page, JournalConnectionPoints.PagePoints[JournalConnectionPoints.RELATED_ITEM_INDEX]);
         return new RectangleF(center.X - HANDLE_HIT_SIZE, center.Y - HANDLE_HIT_SIZE, 2 * HANDLE_HIT_SIZE, 2 * HANDLE_HIT_SIZE);
      }
   }
}
