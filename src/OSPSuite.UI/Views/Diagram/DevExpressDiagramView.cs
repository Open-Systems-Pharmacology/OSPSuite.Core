using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Diagram.Core;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraDiagram;
using OSPSuite.Assets;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Presenters.Diagram;
using OSPSuite.Presentation.Views.Diagram;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Diagram
{
   public partial class DevExpressDiagramView : BaseUserControl, IBaseDiagramView, IViewWithPopup
   {
      private const float PORT_SIZE = 8F;
      private const float LABEL_OFFSET = 2F;
      private const float CONTAINER_LABEL_OFFSET = 3F;
      private const float CONTAINER_HANDLE_SIZE = 8F;
      private const int CLICK_TOLERANCE = 2;
      private const float EXPORT_MARGIN = 10F;
      private const int CONNECTOR_BORDER_SIZE = 1;
      private const float ORIGIN_MARGIN = 10F;
      private const int MOLECULE_CONNECTION_POINT_COUNT = 8;
      private const int NO_CONNECTION_POINT_INDEX = -1;
      private const int MAX_COLLAPSE_DEPTH = 100;
      private const float MIN_EFFECTIVE_ZOOM = 0.01F;
      private static readonly PointCollection _moleculeConnectionPoints = createMoleculeConnectionPoints();
      private static readonly DiagramDoubleCollection _dashPattern = new DiagramDoubleCollection(new double[] {3, 3});

      protected IBaseDiagramPresenter _presenter;
      private DiagramModel _model;
      private readonly Dictionary<IBaseNode, DiagramItem> _items = new Dictionary<IBaseNode, DiagramItem>();
      private readonly Dictionary<IBaseLink, DiagramConnector> _connectors = new Dictionary<IBaseLink, DiagramConnector>();
      private bool _syncing;
      private bool _syncPending;
      private bool _readOnly;
      private bool _connectorToolActive;
      private bool _switchingTool;
      private bool _queryingConnectionPoints;
      private bool _originPending;
      private Point _mouseDownLocation;
      private int _mouseDownClicks;

      public DevExpressDiagramView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         PopupBarManager.Images = imageListRetriever.AllImages16x16;
         initializeDiagramControl();
      }

      private void initializeDiagramControl()
      {
         var optionsView = _diagramControl.OptionsView;
         optionsView.ShowGrid = false;
         optionsView.GridSize = Assets.Diagram.Base.GridCellSize;
         optionsView.ShowPageBreaks = false;
         optionsView.ShowRulers = false;
         optionsView.ShowPanAndZoomPanel = false;
         optionsView.CanvasSizeMode = CanvasSizeMode.Fill;
         optionsView.PropertiesPanelVisibility = PropertiesPanelVisibility.Closed;
         optionsView.ToolboxVisibility = ToolboxVisibility.Closed;
         optionsView.MinZoomFactor = Assets.Diagram.Base.MinLimitDocScale;
         optionsView.MaxZoomFactor = Assets.Diagram.Base.MaxLimitDocScale;

         var optionsBehavior = _diagramControl.OptionsBehavior;
         optionsBehavior.ShowQuickShapes = false;
         optionsBehavior.SnapToGrid = false;

         var optionsProtection = _diagramControl.OptionsProtection;
         optionsProtection.AllowUndoRedo = false;
         optionsProtection.AllowCopyItems = false;
         optionsProtection.AllowEditItems = false;
         optionsProtection.AllowResizeItems = false;
         optionsProtection.AllowRotateItems = false;
         optionsProtection.AllowChangeConnectorsRoute = false;

         _diagramControl.CustomDrawItem += (o, e) => onCustomDrawItem(e);
         _toolTipController.GetActiveObjectInfo += (o, e) => OnEvent(onGetActiveObjectInfo, e);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _diagramControl.ItemsMoving += (o, e) => OnEvent(onItemsMoving, e);
         _diagramControl.ItemsDeleting += (o, e) => OnEvent(onItemsDeleting, e);
         _diagramControl.ItemsChanged += (o, e) => OnEvent(onItemsChanged, e);
         _diagramControl.QueryConnectionPoints += (o, e) => OnEvent(onQueryConnectionPoints, e);
         _diagramControl.ConnectionChanging += (o, e) => OnEvent(onConnectionChanging, e);
         _diagramControl.ConnectionChanged += (o, e) => OnEvent(onConnectionChanged, e);
         _diagramControl.MouseDown += (o, e) => OnEvent(onMouseDown, e);
         _diagramControl.MouseUp += (o, e) => OnEvent(onMouseUp, e);
         _diagramControl.MouseMove += (o, e) => OnEvent(onMouseMove, e);
         _diagramControl.MouseLeave += (o, e) => OnEvent(activatePointerTool);
         _diagramControl.MouseDoubleClick += (o, e) => OnEvent(onMouseDoubleClick, e);
      }

      protected void SetReadOnly()
      {
         _readOnly = true;
         var optionsProtection = _diagramControl.OptionsProtection;
         optionsProtection.IsReadOnly = true;
         optionsProtection.AllowMoveItems = true;
         optionsProtection.AllowZoom = true;
      }

      public IDiagramModel Model
      {
         set
         {
            var diagramModel = value as DiagramModel;
            if (diagramModel == null)
               throw new InvalidTypeException(value, typeof(DiagramModel));

            detachModel();
            _model = diagramModel;
            _model.Changed += onModelChanged;
            synchronize();
            _originPending = true;
            tryScrollToOrigin();
         }
      }

      protected override void OnHandleCreated(EventArgs e)
      {
         base.OnHandleCreated(e);
         tryScrollToOrigin();
      }

      protected override void OnSizeChanged(EventArgs e)
      {
         base.OnSizeChanged(e);
         if (_originPending)
         {
            tryScrollToOrigin();
            return;
         }

         clampToContent();
      }

      private void clampToContent()
      {
         if (!IsHandleCreated)
            return;

         var content = _model?.Bounds ?? RectangleF.Empty;
         if (content.IsEmpty)
            return;

         var minX = content.X - ORIGIN_MARGIN;
         var minY = content.Y - ORIGIN_MARGIN;
         var visibleTopLeft = _diagramControl.PointToDocument(new PointFloat(0, 0));
         if (visibleTopLeft.X >= minX && visibleTopLeft.Y >= minY)
            return;

         _diagramControl.ScrollToPoint(new PointFloat(Math.Max(minX, visibleTopLeft.X), Math.Max(minY, visibleTopLeft.Y)), HorzAlignment.Near, VertAlignment.Top);
      }

      private void tryScrollToOrigin()
      {
         if (!_originPending || !IsHandleCreated)
            return;

         var viewport = _diagramControl.ClientSize;
         if (viewport.Width <= 0 || viewport.Height <= 0)
            return;

         _originPending = false;
         BeginInvoke(new Action(scrollToDiagramOrigin));
      }

      private void scrollToDiagramOrigin()
      {
         var bounds = _model?.Bounds ?? RectangleF.Empty;
         if (bounds.IsEmpty)
            return;

         _diagramControl.ScrollToPoint(new PointFloat(bounds.X - ORIGIN_MARGIN, bounds.Y - ORIGIN_MARGIN), HorzAlignment.Near, VertAlignment.Top);
      }

      private void detachModel()
      {
         if (_model == null)
            return;
         _model.Changed -= onModelChanged;
         _model = null;
      }

      public virtual void AttachPresenter(IBaseDiagramPresenter presenter)
      {
         _presenter = presenter;
      }

      public IBaseDiagramPresenter Presenter => _presenter;

      public BarManager PopupBarManager { get; private set; }

      private void onModelChanged()
      {
         if (_syncing || IsDisposed)
            return;

         if (!IsHandleCreated)
         {
            synchronize();
            return;
         }

         if (_syncPending)
            return;
         _syncPending = true;
         BeginInvoke(new Action(() =>
         {
            _syncPending = false;
            synchronize();
         }));
      }

      private void synchronize()
      {
         if (_model == null || _syncing)
            return;

         _syncing = true;
         try
         {
            _diagramControl.BeginUpdate();
            var nodes = _model.GetAllChildren<DiagramNode>().Where(isShown).OrderBy(depthOf).ToList();
            removeStaleItems(nodes);
            nodes.OfType<ContainerNode>().Each(node => node.CollapsedSize = collapsedSizeFor(node));
            nodes.Each(updateItem);

            var links = _model.GetAllChildren<BaseLink>()
               .Where(link => link.IsVisible)
               .Select(link => (link, begin: shownItemFor(link.FromNode), end: shownItemFor(link.ToNode)))
               .Where(x => x.begin != null && x.end != null && x.begin != x.end)
               .ToList();
            removeStaleConnectors(links.Select(x => x.link).ToList());
            links.Each(x => updateConnector(x.link, x.begin, x.end));

            updateZOrder(nodes);
         }
         finally
         {
            _diagramControl.EndUpdate();
            _syncing = false;
         }
      }

      private static bool isShown(DiagramNode node)
      {
         if (!node.Visible)
            return false;
         if (!(node is NeighborhoodNode neighborhood))
            return true;

         var first = shownNodeFor(neighborhood.FirstNeighbor);
         var second = shownNodeFor(neighborhood.SecondNeighbor);
         return first == null || second == null || first != second;
      }

      private static IBaseNode shownNodeFor(IBaseNode node)
      {
         var current = node;
         while (current != null && !current.Visible)
         {
            if (current.Hidden || !current.IsVisible)
               return null;

            current = current.GetParent() as IBaseNode;
         }

         return current;
      }

      private DiagramItem shownItemFor(IBaseNode node)
      {
         var shown = shownNodeFor(node);
         return shown != null && _items.TryGetValue(shown, out var item) ? item : null;
      }

      private static int depthOf(IBaseNode node)
      {
         var depth = 0;
         for (var parent = node.GetParent() as IBaseNode; parent != null; parent = parent.GetParent() as IBaseNode)
            depth++;
         return depth;
      }

      private void removeStaleItems(IReadOnlyList<DiagramNode> nodes)
      {
         _items.Where(x => !nodes.Contains(x.Key)).ToList().Each(stale =>
         {
            _connectors.Where(x => x.Value.BeginItem == stale.Value || x.Value.EndItem == stale.Value).ToList().Each(removeConnector);
            collectionOf(stale.Value).Remove(stale.Value);
            _items.Remove(stale.Key);
         });
      }

      private DiagramItemCollection collectionOf(DiagramItem item) => (item.ParentItem as DiagramContainer)?.Items ?? _diagramControl.Items;

      private void removeStaleConnectors(IReadOnlyList<BaseLink> links)
      {
         _connectors.Where(x => !links.Contains(x.Key)).ToList().Each(removeConnector);
      }

      private void removeConnector(KeyValuePair<IBaseLink, DiagramConnector> connector)
      {
         _diagramControl.Items.Remove(connector.Value);
         _connectors.Remove(connector.Key);
      }

      private void updateItem(DiagramNode node)
      {
         switch (node)
         {
            case ContainerNode containerNode:
               updateContainer(containerNode);
               break;
            case ElementBaseNode elementNode:
               updateShape(elementNode);
               break;
         }
      }

      private void updateContainer(ContainerNode node)
      {
         if (!_items.TryGetValue(node, out var item))
         {
            item = new DiagramContainer
            {
               Tag = node,
               CanResize = false,
               CanRotate = false,
               CanEdit = false,
               CanSelect = true,
               CanAddItems = false,
               CanChangeParent = false,
               ItemsCanChangeParent = false,
               ClipItemsToBounds = false,
               ShowHeader = false
            };
            _items.Add(node, item);
         }

         placeInParent(item, node);
         var container = (DiagramContainer) item;
         container.Position = new PointFloat(relativeTo(node, node.Location));
         container.Size = node.DrawnBounds.Size;
         container.Appearance.BackColor = node.BackgroundColor;
         container.Appearance.BorderColor = node.BorderColor;
         container.Appearance.BorderSize = (int) node.BorderWidth;
      }

      private void updateShape(ElementBaseNode node)
      {
         if (!_items.TryGetValue(node, out var item))
         {
            item = new DiagramShape
            {
               Tag = node,
               CanResize = false,
               CanRotate = false,
               CanEdit = false,
               CanSelect = true,
               CanChangeParent = false
            };
            _items.Add(node, item);
         }

         placeInParent(item, node);
         var shape = (DiagramShape) item;
         shape.Shape = ShapeFor(node);
         shape.ConnectionPoints = ConnectionPointsFor(node);
         shape.MinSize = SizeF.Empty;
         shape.Size = node.Size;
         shape.Position = new PointFloat(relativeTo(node, node.Bounds.Location));
         shape.CanAttachConnectorBeginPoint = node.CanLink;
         shape.CanAttachConnectorEndPoint = node.CanLink;
         shape.Appearance.BackColor = node.FillColor;
         shape.Appearance.BorderColor = node.BorderColor;
         shape.Appearance.BorderSize = (int) node.BorderWidth;
      }

      protected virtual ShapeDescription ShapeFor(ElementBaseNode node) => node is ReactionNode ? BasicShapes.Triangle : BasicShapes.Ellipse;

      protected virtual PointCollection ConnectionPointsFor(ElementBaseNode node)
      {
         return node is ReactionNode reactionNode ? ReactionConnectionPoints.For(reactionNode.DisplayEductsRight) : _moleculeConnectionPoints;
      }

      private void placeInParent(DiagramItem item, DiagramNode node)
      {
         var collection = parentContainerOf(node) is ContainerNode parent ? ((DiagramContainer) _items[parent]).Items : _diagramControl.Items;
         if (collection.Contains(item))
            return;

         if (item.ParentItem != null)
            collectionOf(item).Remove(item);
         else
            _diagramControl.Items.Remove(item);
         collection.Add(item);
      }

      private ContainerNode parentContainerOf(IBaseNode node)
      {
         return node.GetParent() is ContainerNode parent && _items.ContainsKey(parent) ? parent : null;
      }

      private PointF relativeTo(IBaseNode node, PointF absoluteLocation)
      {
         var parent = parentContainerOf(node);
         return parent == null ? absoluteLocation : new PointF(absoluteLocation.X - parent.Location.X, absoluteLocation.Y - parent.Location.Y);
      }

      private void updateConnector(BaseLink link, DiagramItem beginItem, DiagramItem endItem)
      {
         if (!_connectors.TryGetValue(link, out var connector))
         {
            connector = new DiagramConnector(beginItem, endItem)
            {
               Tag = link,
               Type = ConnectorType.Straight,
               BeginArrow = null,
               EndArrow = null,
               CanChangeRoute = false,
               CanEdit = false,
               CanSelect = true,
               CanMove = false,
               CanDragBeginPoint = false,
               CanDragEndPoint = false
            };
            _connectors.Add(link, connector);
            _diagramControl.Items.Add(connector);
         }

         connector.BeginItem = beginItem;
         connector.EndItem = endItem;
         connector.Type = link.IsCurved ? ConnectorType.Curved : ConnectorType.Straight;
         connector.BeginItemPointIndex = PointIndexFor(link, link.FromNode);
         connector.EndItemPointIndex = PointIndexFor(link, link.ToNode);
         connector.Appearance.BorderColor = link.Color;
         connector.Appearance.BorderSize = CONNECTOR_BORDER_SIZE;
         connector.Appearance.BorderDashPattern = link.IsDashed ? _dashPattern : DiagramDoubleCollection.EmptyCollection;
      }

      protected virtual int PointIndexFor(IBaseLink link, IBaseNode node)
      {
         if (node is ReactionNode && link is ReactionLink reactionLink)
            return ReactionConnectionPoints.IndexFor(reactionLink.Type);

         return NO_CONNECTION_POINT_INDEX;
      }

      private void updateZOrder(IReadOnlyList<DiagramNode> nodes)
      {
         var rootItems = _connectors.Values.Cast<DiagramItem>().Concat(nodes.Where(node => parentContainerOf(node) == null).Select(node => _items[node])).ToList();
         reorder(_diagramControl.Items, rootItems);

         nodes.OfType<ContainerNode>().Each(container =>
         {
            var childItems = container.GetDirectChildren<DiagramNode>().Where(_items.ContainsKey).Select(node => _items[node]).ToList();
            reorder(((DiagramContainer) _items[container]).Items, childItems);
         });
      }

      private void reorder(DiagramItemCollection collection, IReadOnlyList<DiagramItem> orderedItems)
      {
         if (collection.SequenceEqual(orderedItems))
            return;
         orderedItems.Each(item => _diagramControl.BringItemsToFront(new[] {item}));
      }

      private static PointCollection createMoleculeConnectionPoints()
      {
         return new PointCollection(Enumerable.Range(0, MOLECULE_CONNECTION_POINT_COUNT).Select(i =>
         {
            var angle = i * 2 * Math.PI / MOLECULE_CONNECTION_POINT_COUNT;
            return new PointFloat((float) (0.5 + 0.5 * Math.Cos(angle)), (float) (0.5 + 0.5 * Math.Sin(angle)));
         }).ToList());
      }

      private void onCustomDrawItem(CustomDrawItemEventArgs e)
      {
         if (e.Item.Tag is ContainerNode containerNode)
         {
            e.DefaultDraw(CustomDrawItemMode.All);
            drawContainerLabel(e.Graphics, containerNode);
            e.Handled = true;
            return;
         }

         if (!(e.Item.Tag is ElementBaseNode node))
            return;

         e.DefaultDraw(CustomDrawItemMode.All);
         DrawNode(e.Graphics, node, e.Size);
         e.Handled = true;
      }

      protected virtual void DrawNode(Graphics graphics, ElementBaseNode node, SizeF size)
      {
         var reactionNode = node as ReactionNode;
         if (reactionNode != null)
            drawReactionPorts(graphics, reactionNode, size);
         else
            drawInnerEllipse(graphics, node, size);

         drawLabel(graphics, node, size, labelBelow: reactionNode != null);
      }

      private void drawContainerLabel(Graphics graphics, ContainerNode node)
      {
         using (var handleBrush = new SolidBrush(node.HandleColor))
         using (var textBrush = new SolidBrush(ForeColor))
         {
            graphics.FillRectangle(handleBrush, CONTAINER_LABEL_OFFSET, CONTAINER_LABEL_OFFSET, CONTAINER_HANDLE_SIZE, CONTAINER_HANDLE_SIZE);
            if (!string.IsNullOrEmpty(node.Name))
               graphics.DrawString(node.Name, Font, textBrush, containerHandleBounds(node).Width, CONTAINER_LABEL_OFFSET / 2);
         }
      }

      private static RectangleF containerHandleBounds(ContainerNode node)
      {
         var size = CONTAINER_HANDLE_SIZE + 2 * CONTAINER_LABEL_OFFSET;
         return new RectangleF(node.Location.X, node.Location.Y, size, size);
      }

      private SizeF collapsedSizeFor(ContainerNode node)
      {
         var handle = containerHandleBounds(node);
         var text = TextRenderer.MeasureText(node.Name ?? string.Empty, Font);
         return new SizeF(handle.Width + text.Width + CONTAINER_LABEL_OFFSET, Math.Max(handle.Height, text.Height + CONTAINER_LABEL_OFFSET));
      }

      private static void drawInnerEllipse(Graphics graphics, ElementBaseNode node, SizeF size)
      {
         using (var brush = new SolidBrush(node.PortColor))
         {
            graphics.FillEllipse(brush, size.Width / 4, size.Height / 4, size.Width / 2, size.Height / 2);
         }
      }

      private static void drawReactionPorts(Graphics graphics, ReactionNode node, SizeF size)
      {
         var points = ReactionConnectionPoints.For(node.DisplayEductsRight);
         drawPort(graphics, node.EductPortColor, points[ReactionConnectionPoints.EDUCT_INDEX], size);
         drawPort(graphics, node.ProductPortColor, points[ReactionConnectionPoints.PRODUCT_INDEX], size);
         drawPort(graphics, node.ModifierPortColor, points[ReactionConnectionPoints.MODIFIER_INDEX], size);
      }

      private static void drawPort(Graphics graphics, Color color, PointFloat relativePoint, SizeF size)
      {
         using (var brush = new SolidBrush(color))
         {
            graphics.FillEllipse(brush, relativePoint.X * size.Width - PORT_SIZE / 2, relativePoint.Y * size.Height - PORT_SIZE / 2, PORT_SIZE, PORT_SIZE);
         }
      }

      private void drawLabel(Graphics graphics, ElementBaseNode node, SizeF size, bool labelBelow)
      {
         if (!hasLabel(node))
            return;

         using (var font = labelFont(node))
         using (var brush = new SolidBrush(node.LabelColor))
         {
            var textSize = graphics.MeasureString(node.Name, font);
            graphics.DrawString(node.Name, font, brush, labelLocation(size, textSize, labelBelow));
         }
      }

      private static bool hasLabel(ElementBaseNode node) => node.LabelVisible && !string.IsNullOrEmpty(node.Name);

      private static Font labelFont(ElementBaseNode node) => new Font(SystemFonts.DefaultFont.FontFamily, node.LabelFontSize);

      private static PointF labelLocation(SizeF size, SizeF textSize, bool labelBelow)
      {
         return labelBelow
            ? new PointF((size.Width - textSize.Width) / 2, size.Height + LABEL_OFFSET)
            : new PointF(size.Width + LABEL_OFFSET, (size.Height - textSize.Height) / 2);
      }

      private RectangleF visualBounds(Graphics graphics, DiagramNode node)
      {
         switch (node)
         {
            case ContainerNode containerNode when !containerNode.IsExpanded:
               return containerNode.DrawnBounds;
            case ContainerNode containerNode:
               var labelSize = graphics.MeasureString(containerNode.Name ?? string.Empty, Font);
               return RectangleF.Union(containerNode.DrawnBounds, new RectangleF(containerHandleBounds(containerNode).Right, node.Location.Y, labelSize.Width, labelSize.Height));
            case ElementBaseNode elementNode when hasLabel(elementNode):
               using (var font = labelFont(elementNode))
               {
                  var textSize = graphics.MeasureString(node.Name, font);
                  var location = labelLocation(node.Size, textSize, labelBelow: node is ReactionNode);
                  return RectangleF.Union(node.Bounds, new RectangleF(node.Bounds.X + location.X, node.Bounds.Y + location.Y, textSize.Width, textSize.Height));
               }
            default:
               return node.Bounds;
         }
      }

      private RectangleF drawingBounds() => drawingBounds(_items.Keys.OfType<DiagramNode>().ToList());

      private RectangleF drawingBounds(IReadOnlyList<DiagramNode> nodes)
      {
         using (var bitmap = new Bitmap(1, 1))
         using (var graphics = Graphics.FromImage(bitmap))
         {
            var bounds = nodes.Select(node => visualBounds(graphics, node)).Aggregate(RectangleF.Union);
            bounds.Inflate(EXPORT_MARGIN, EXPORT_MARGIN);
            return bounds;
         }
      }

      private IReadOnlyList<DiagramNode> nodesToExport(IContainerBase containerBase)
      {
         var allNodes = _items.Keys.OfType<DiagramNode>().ToList();
         if (containerBase == null || containerBase is IDiagramModel)
            return allNodes;

         var exported = containerBase.GetAllChildren<IBaseNode>().ToList();
         return allNodes.Where(node => Equals(node, containerBase) || exported.Contains(node)).ToList();
      }

      private void onGetActiveObjectInfo(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != _diagramControl)
            return;

         var node = itemAt(e.ControlMousePosition)?.Tag as IBaseNode;
         if (node == null)
            return;

         var lines = new List<string> {node.Description};
         if (node is MoleculeNode moleculeNode && moleculeNode.CanLink)
            lines.Add(ToolTips.BuildingBlockReaction.HowToCreateReactionLink);

         var text = lines.Where(line => !string.IsNullOrEmpty(line)).ToString(Environment.NewLine);
         if (!string.IsNullOrEmpty(text))
            e.Info = new ToolTipControlInfo(node, text);
      }

      private DiagramItem itemAt(Point controlPoint) => _diagramControl.CalcHitItem(controlPoint);

      private void onItemsMoving(DiagramItemsMovingEventArgs e)
      {
         if (_syncing || e.Stage != DiagramActionStage.Finished)
            return;

         var movedItems = e.Items.Where(x => x.Item.Tag is DiagramNode).OrderBy(x => depthOf((DiagramNode) x.Item.Tag)).ToList();
         if (!movedItems.Any())
            return;

         var movedNodes = movedItems.Select(x => (IBaseNode) x.Item.Tag).ToList();
         _syncing = true;
         try
         {
            _model.StartTransaction();
            movedItems.Each(x =>
            {
               var node = (DiagramNode) x.Item.Tag;
               var before = node.Location;
               moveNode(node, new PointF(x.NewDiagramPosition.X, x.NewDiagramPosition.Y), movedNodes);
               var delta = new PointF(node.Location.X - before.X, node.Location.Y - before.Y);
               var dependents = NodesMovingWith(node).Where(dependent => !movedNodes.Contains(dependent)).ToList();
               dependents.Each(dependent => dependent.Location = new PointF(dependent.Location.X + delta.X, dependent.Location.Y + delta.Y));
            });
            _model.FinishTransaction("Move");
         }
         finally
         {
            _syncing = false;
         }

         defer(synchronize);

         _presenter.SelectionMoved(this, EventArgs.Empty);
      }

      protected virtual IEnumerable<IBaseNode> NodesMovingWith(IBaseNode node) => Enumerable.Empty<IBaseNode>();

      private static void moveNode(DiagramNode node, PointF topLeft, IReadOnlyList<IBaseNode> movedNodes)
      {
         switch (node)
         {
            case ContainerNode containerNode:
               var offset = new SizeF(topLeft.X - containerNode.Location.X, topLeft.Y - containerNode.Location.Y);
               containerNode.Location = topLeft;
               containerNode.GetLinkedNodes<INeighborhoodNode>(true)
                  .Where(neighborhoodNode => !movedNodes.Contains(neighborhoodNode) && !containerNode.ContainsChildNode(neighborhoodNode, true))
                  .Each(neighborhoodNode => neighborhoodNode.AdjustPositionForContainerInMove(containerNode, offset));
               break;
            case ElementBaseNode elementNode:
               elementNode.Location = new PointF(topLeft.X + elementNode.Size.Width / 2, topLeft.Y + elementNode.Size.Height / 2);
               break;
         }
      }

      private void onItemsDeleting(DiagramItemsDeletingEventArgs e)
      {
         e.Cancel = true;
         if (_syncing || _readOnly)
            return;

         var tags = e.Items.Select(item => item.Tag).ToList();
         OnSelectionDeleting(tags.OfType<IBaseNode>().ToList(), tags.OfType<IBaseLink>().ToList());
      }

      private void onItemsChanged(DiagramItemsChangedEventArgs e)
      {
         if (_syncing || e.Action != ItemsChangedAction.Added || e.Item.Tag != null)
            return;

         if (e.Item is DiagramConnector connector)
            finalizeUserConnector(connector);
         else
            defer(() => collectionOf(e.Item).Remove(e.Item));
      }

      private void onConnectionChanged(DiagramConnectionChangedEventArgs e)
      {
         if (_syncing || e.Connector.Tag != null)
            return;
         finalizeUserConnector(e.Connector);
      }

      private void finalizeUserConnector(DiagramConnector connector)
      {
         if (connector.BeginItem == null || connector.EndItem == null)
            return;
         defer(() => createLinkFrom(connector));
      }

      private void createLinkFrom(DiagramConnector connector)
      {
         if (!_diagramControl.Items.Contains(connector))
            return;

         var fromNode = nodeOf(connector.BeginItem);
         var toNode = nodeOf(connector.EndItem);
         var fromPort = PortFor(fromNode, connector.BeginItemPointIndex, connector.BeginPoint);
         var toPort = PortFor(toNode, connector.EndItemPointIndex, connector.EndPoint);
         _diagramControl.Items.Remove(connector);
         if (_readOnly || fromNode == null || toNode == null)
            return;

         OnLinkCreated(fromNode, toNode, fromPort, toPort);
      }

      private static ElementBaseNode nodeOf(IDiagramItem item) => (item as DiagramItem)?.Tag as ElementBaseNode;

      protected virtual object PortFor(ElementBaseNode node, int pointIndex, PointFloat point)
      {
         if (!(node is ReactionNode reactionNode))
            return null;
         return ReactionConnectionPoints.LinkTypeFor(pointIndex) ?? nearestLinkType(reactionNode, point);
      }

      private static ReactionLinkType nearestLinkType(ReactionNode node, PointFloat point)
      {
         var connectionPoints = ReactionConnectionPoints.For(node.DisplayEductsRight);
         var nearestIndex = Enumerable.Range(0, connectionPoints.Count)
            .OrderBy(i => Distance(new PointF(point.X, point.Y), AbsolutePoint(node, connectionPoints[i])))
            .First();
         return ReactionConnectionPoints.LinkTypeFor(nearestIndex).Value;
      }

      protected static PointF AbsolutePoint(ElementBaseNode node, PointFloat relativePoint)
      {
         var bounds = node.Bounds;
         return new PointF(bounds.X + relativePoint.X * bounds.Width, bounds.Y + relativePoint.Y * bounds.Height);
      }

      private void finalizeForeignItems()
      {
         _diagramControl.Items.Where(item => item.Tag == null).ToList().Each(item =>
         {
            if (item is DiagramConnector connector && connector.BeginItem != null && connector.EndItem != null)
               createLinkFrom(connector);
            else
               _diagramControl.Items.Remove(item);
         });
      }

      private void defer(Action action)
      {
         if (IsHandleCreated)
            BeginInvoke(action);
         else
            action();
      }

      private void onQueryConnectionPoints(DiagramQueryConnectionPointsEventArgs e)
      {
         if (_syncing || _queryingConnectionPoints)
            return;

         _queryingConnectionPoints = true;
         try
         {
            var node = e.HoveredItem?.Tag as ElementBaseNode;
            if (node == null)
               return;

            var oppositeNode = nodeOf(e.ConnectorPointType == ConnectorPointType.End ? e.Connector?.BeginItem : e.Connector?.EndItem);
            if (!_readOnly && CanConnect(node, oppositeNode))
               return;

            e.ItemConnectionBorderState = ConnectionElementState.Disabled;
            e.ItemConnectionPointStates.Each(point => point.State = ConnectionElementState.Disabled);
         }
         finally
         {
            _queryingConnectionPoints = false;
         }
      }

      private void onConnectionChanging(DiagramConnectionChangingEventArgs e)
      {
         if (_syncing || e.Connector.Tag != null)
            return;

         if (_readOnly)
         {
            e.Cancel = true;
            return;
         }

         var newNode = e.NewItem?.Tag as ElementBaseNode;
         if (newNode == null)
            return;

         var oppositeIsBegin = e.ConnectorPointType == ConnectorPointType.End;
         var oppositeNode = nodeOf(oppositeIsBegin ? e.Connector.BeginItem : e.Connector.EndItem);
         var oppositePointIndex = oppositeIsBegin ? e.Connector.BeginItemPointIndex : e.Connector.EndItemPointIndex;
         e.Cancel = !IsValidConnection(newNode, e.NewIndex, oppositeNode, oppositePointIndex);
      }

      protected virtual bool CanConnect(ElementBaseNode node, ElementBaseNode oppositeNode)
      {
         return node.CanLink && (oppositeNode == null || (oppositeNode.CanLink && !sameKind(node, oppositeNode)));
      }

      protected virtual bool IsValidConnection(ElementBaseNode node, int pointIndex, ElementBaseNode oppositeNode, int oppositePointIndex)
      {
         if (!node.CanLink)
            return false;
         if (oppositeNode == null)
            return true;
         if (!oppositeNode.CanLink || sameKind(node, oppositeNode))
            return false;

         var linkType = ReactionConnectionPoints.LinkTypeFor(node is ReactionNode ? pointIndex : oppositePointIndex);
         return !linkType.HasValue || !linkExists(node, oppositeNode, linkType.Value);
      }

      private static bool sameKind(ElementBaseNode node, ElementBaseNode otherNode) => node is ReactionNode == otherNode is ReactionNode;

      private static bool linkExists(ElementBaseNode node, ElementBaseNode otherNode, ReactionLinkType linkType)
      {
         return node.Links.OfType<ReactionLink>().Any(link => link.Type == linkType && link.GetOtherNode(node) == otherNode);
      }

      private void onMouseMove(MouseEventArgs e)
      {
         if (_readOnly || _switchingTool || e.Button != MouseButtons.None)
            return;
         setConnectorToolActive(isOverPort(e.Location));
      }

      private void activatePointerTool() => setConnectorToolActive(false);

      private void setConnectorToolActive(bool active)
      {
         if (_switchingTool || _connectorToolActive == active)
            return;

         _switchingTool = true;
         try
         {
            _connectorToolActive = active;
            var optionsBehavior = _diagramControl.OptionsBehavior;
            var tool = active ? optionsBehavior.ConnectorTool : optionsBehavior.PointerTool;
            if (!ReferenceEquals(optionsBehavior.ActiveTool, tool))
               optionsBehavior.ActiveTool = tool;
         }
         finally
         {
            _switchingTool = false;
         }
      }

      private bool isOverPort(Point controlPoint)
      {
         var documentPoint = _diagramControl.PointToDocument(new PointFloat(controlPoint));
         var point = new PointF(documentPoint.X, documentPoint.Y);
         return _items.Keys.OfType<ElementBaseNode>().Any(node => node.CanLink && IsOverPortOf(node, point));
      }

      protected virtual bool IsOverPortOf(ElementBaseNode node, PointF point)
      {
         if (node is ReactionNode reactionNode)
            return ReactionConnectionPoints.For(reactionNode.DisplayEductsRight).Any(relativePoint => Distance(point, AbsolutePoint(node, relativePoint)) <= PORT_SIZE);

         var bounds = node.Bounds;
         var center = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
         var radius = Math.Min(bounds.Width, bounds.Height) / 2;
         var distanceToCenter = Distance(point, center);
         return distanceToCenter <= radius + PORT_SIZE / 2 && distanceToCenter >= radius / 2;
      }

      protected static float Distance(PointF first, PointF second)
      {
         return (float) Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
      }

      private void onMouseDown(MouseEventArgs e)
      {
         _mouseDownLocation = e.Location;
         _mouseDownClicks = e.Clicks;
      }

      private bool isClick(Point location) => Math.Abs(location.X - _mouseDownLocation.X) <= CLICK_TOLERANCE && Math.Abs(location.Y - _mouseDownLocation.Y) <= CLICK_TOLERANCE;

      protected PointF ToDocumentPoint(Point controlPoint)
      {
         var documentPoint = _diagramControl.PointToDocument(new PointFloat(controlPoint));
         return new PointF(documentPoint.X, documentPoint.Y);
      }

      protected virtual void OnNodeClicked(IBaseNode node, PointF documentPoint, Keys modifiers)
      {
         if (node is ContainerNode containerNode && containerHandleBounds(containerNode).Contains(documentPoint))
            onHandleClicked(containerNode, modifiers);
      }

      private void onHandleClicked(ContainerNode node, Keys modifiers)
      {
         if (modifiers.HasFlag(Keys.Shift))
         {
            ChangeLayout(() =>
            {
               node.IsExpanded = !node.IsExpanded;
               node.PostLayoutStep();
            }, node.IsExpanded ? "Collapse" : "Expand");
            return;
         }

         if (modifiers.HasFlag(Keys.Control))
         {
            if (node.IsExpanded)
               ChangeLayout(() =>
               {
                  node.Collapse(MAX_COLLAPSE_DEPTH);
                  node.PostLayoutStep();
               }, "Collapse");
            return;
         }

         if (node.IsExpanded)
         {
            _presenter.Unfocus(node);
            return;
         }

         if (!node.IsExpandedByDefault)
            _presenter.HideAll();
         _presenter.Focus(node);
      }

      protected void ChangeLayout(Action change, string description)
      {
         _model.StartTransaction();
         change();
         _model.FinishTransaction(description);
      }

      private void onMouseUp(MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
            if (_mouseDownClicks == 1 && isClick(e.Location) && itemAt(e.Location)?.Tag is IBaseNode clickedNode)
               OnNodeClicked(clickedNode, ToDocumentPoint(e.Location), ModifierKeys);

            defer(() =>
            {
               finalizeForeignItems();
               activatePointerTool();
            });
            return;
         }

         if (e.Button != MouseButtons.Right)
            return;

         var item = itemAt(e.Location);
         if (item is DiagramConnector)
            return;

         ShowContextMenu(item?.Tag as IBaseNode, e.Location, ToDocumentPoint(e.Location));
      }

      protected virtual void ShowContextMenu(IBaseNode node, Point location, PointF documentPoint)
      {
         _presenter.ShowContextMenu(node, location, documentPoint);
      }

      private void onMouseDoubleClick(MouseEventArgs e)
      {
         if (itemAt(e.Location)?.Tag is IBaseNode node)
            OnNodeDoubleClicked(node);
      }

      protected virtual void OnNodeDoubleClicked(IBaseNode node)
      {
      }

      protected virtual void OnSelectionDeleting(IReadOnlyList<IBaseNode> nodes, IReadOnlyList<IBaseLink> links)
      {
      }

      protected virtual void OnLinkCreated(IBaseNode fromNode, IBaseNode toNode, object fromPort, object toPort)
      {
      }

      public void BeginUpdate() => _diagramControl.BeginUpdate();

      public void EndUpdate() => _diagramControl.EndUpdate();

      public void ClearSelection() => _diagramControl.ClearSelection();

      public void Select<T>(T node)
      {
         var item = itemFor(node);
         if (item != null)
            _diagramControl.SelectItem(item, ModifySelectionMode.AddToSelection);
      }

      private DiagramItem itemFor(object obj)
      {
         switch (obj)
         {
            case IBaseNode node when _items.TryGetValue(node, out var item):
               return item;
            case IBaseLink link when _connectors.TryGetValue(link, out var connector):
               return connector;
            default:
               return null;
         }
      }

      public IEnumerable<T> GetSelectedNodes<T>() where T : class
      {
         return _diagramControl.SelectedItems.Select(item => item.Tag).OfType<T>().ToList();
      }

      public bool SelectionContains<T>(T obj)
      {
         return _diagramControl.SelectedItems.Any(item => ReferenceEquals(item.Tag, obj));
      }

      public void CenterAt<T>(T node)
      {
         var item = itemFor(node);
         if (item == null)
            return;

         var bounds = node is DiagramNode diagramNode ? diagramNode.DrawnBounds : item.Bounds;
         var centre = new PointF(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);

         var content = _model?.Bounds ?? RectangleF.Empty;
         if (content.IsEmpty)
         {
            _diagramControl.ScrollToPoint(new PointFloat(centre), HorzAlignment.Center, VertAlignment.Center);
            return;
         }

         var zoom = Math.Max(MIN_EFFECTIVE_ZOOM, (float) _diagramControl.OptionsView.ZoomFactor);
         var halfWidth = _diagramControl.ClientSize.Width / zoom / 2;
         var halfHeight = _diagramControl.ClientSize.Height / zoom / 2;

         var topLeft = new PointFloat(
            Math.Max(content.X - ORIGIN_MARGIN, centre.X - halfWidth),
            Math.Max(content.Y - ORIGIN_MARGIN, centre.Y - halfHeight));

         _diagramControl.ScrollToPoint(topLeft, HorzAlignment.Near, VertAlignment.Top);
      }

      public override void Refresh()
      {
         synchronize();
         base.Refresh();
      }

      public bool GridVisible
      {
         get => _diagramControl.OptionsView.ShowGrid;
         set
         {
            _diagramControl.OptionsView.ShowGrid = value;
            _diagramControl.OptionsBehavior.SnapToGrid = value;
         }
      }

      public void Zoom(PointF currentLocation, float factor)
      {
         if (factor <= 0F)
         {
            _diagramControl.FitToDrawing();
            return;
         }

         var optionsView = _diagramControl.OptionsView;
         optionsView.ZoomFactor = Math.Max(optionsView.MinZoomFactor, Math.Min(optionsView.MaxZoomFactor, optionsView.ZoomFactor * factor));
         _diagramControl.ScrollToPoint(new PointFloat(currentLocation), HorzAlignment.Center, VertAlignment.Center);
      }

      public void SetBackColor(Color color)
      {
         _diagramControl.BackColor = color;
      }

      public Bitmap GetBitmap(IContainerBase containerBase)
      {
         if (_items.Count == 0)
            return new Bitmap(1, 1);

         var exportBounds = drawingBounds(nodesToExport(containerBase));
         using (var stream = new MemoryStream())
         {
            _diagramControl.ExportToImage(stream, DiagramImageExportFormat.PNG, exportBounds, null, null);
            stream.Position = 0;
            using (var image = new Bitmap(stream))
            {
               return new Bitmap(image);
            }
         }
      }

      public void PrintPreview()
      {
         _diagramControl.ShowPrintPreview();
      }

      public void CopyToClipboard(Image image)
      {
         Clipboard.SetImage(image);
      }
   }
}
