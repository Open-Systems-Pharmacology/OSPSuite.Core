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
      private const float EXPORT_MARGIN = 10F;
      private const int CONNECTOR_BORDER_SIZE = 1;
      private const int NO_CONNECTION_POINT_INDEX = -1;
      private static readonly PointCollection _moleculeConnectionPoints = createMoleculeConnectionPoints();
      private static readonly DiagramDoubleCollection _dashPattern = new DiagramDoubleCollection(new double[] {3, 3});

      protected IBaseDiagramPresenter _presenter;
      private DiagramModel _model;
      private readonly Dictionary<IBaseNode, DiagramShape> _shapes = new Dictionary<IBaseNode, DiagramShape>();
      private readonly Dictionary<IBaseLink, DiagramConnector> _connectors = new Dictionary<IBaseLink, DiagramConnector>();
      private bool _syncing;
      private bool _syncPending;
      private bool _readOnly;
      private bool _connectorToolActive;
      private bool _switchingTool;
      private bool _queryingConnectionPoints;

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
         optionsView.CanvasSizeMode = CanvasSizeMode.AutoSize;
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
         }
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
            var nodes = _model.GetAllChildren<ElementBaseNode>().Where(node => node.Visible).ToList();
            removeStaleShapes(nodes);
            nodes.Each(updateShape);

            var links = _model.GetAllChildren<BaseLink>().Where(link => link.Visible && _shapes.ContainsKey(link.FromNode) && _shapes.ContainsKey(link.ToNode)).ToList();
            removeStaleConnectors(links);
            links.Each(updateConnector);

            updateZOrder(nodes);
         }
         finally
         {
            _diagramControl.EndUpdate();
            _syncing = false;
         }
      }

      private void removeStaleShapes(IReadOnlyList<ElementBaseNode> nodes)
      {
         _shapes.Where(x => !nodes.Contains(x.Key)).ToList().Each(stale =>
         {
            _connectors.Where(x => x.Value.BeginItem == stale.Value || x.Value.EndItem == stale.Value).ToList().Each(removeConnector);
            _diagramControl.Items.Remove(stale.Value);
            _shapes.Remove(stale.Key);
         });
      }

      private void removeStaleConnectors(IReadOnlyList<BaseLink> links)
      {
         _connectors.Where(x => !links.Contains(x.Key)).ToList().Each(removeConnector);
      }

      private void removeConnector(KeyValuePair<IBaseLink, DiagramConnector> connector)
      {
         _diagramControl.Items.Remove(connector.Value);
         _connectors.Remove(connector.Key);
      }

      private void updateShape(ElementBaseNode node)
      {
         if (!_shapes.TryGetValue(node, out var shape))
         {
            shape = new DiagramShape
            {
               Tag = node,
               CanResize = false,
               CanRotate = false,
               CanEdit = false,
               CanSelect = true
            };
            _shapes.Add(node, shape);
            _diagramControl.Items.Add(shape);
         }

         var reactionNode = node as ReactionNode;
         shape.Shape = reactionNode != null ? BasicShapes.Triangle : BasicShapes.Ellipse;
         shape.ConnectionPoints = reactionNode != null ? ReactionConnectionPoints.For(reactionNode.DisplayEductsRight) : _moleculeConnectionPoints;
         shape.MinSize = SizeF.Empty;
         shape.Size = node.Size;
         shape.Position = new PointFloat(node.Bounds.Location);
         shape.CanAttachConnectorBeginPoint = node.CanLink;
         shape.CanAttachConnectorEndPoint = node.CanLink;
         shape.Appearance.BackColor = node.FillColor;
         shape.Appearance.BorderColor = node.BorderColor;
         shape.Appearance.BorderSize = (int) node.BorderWidth;
      }

      private void updateConnector(BaseLink link)
      {
         var beginShape = _shapes[link.FromNode];
         var endShape = _shapes[link.ToNode];
         if (!_connectors.TryGetValue(link, out var connector))
         {
            connector = new DiagramConnector(beginShape, endShape)
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

         connector.BeginItem = beginShape;
         connector.EndItem = endShape;
         connector.BeginItemPointIndex = pointIndexFor(link, link.FromNode);
         connector.EndItemPointIndex = pointIndexFor(link, link.ToNode);
         connector.Appearance.BorderColor = link.Color;
         connector.Appearance.BorderSize = CONNECTOR_BORDER_SIZE;
         connector.Appearance.BorderDashPattern = link.IsDashed ? _dashPattern : DiagramDoubleCollection.EmptyCollection;
      }

      private static int pointIndexFor(IBaseLink link, IBaseNode node)
      {
         if (node is ReactionNode && link is ReactionLink reactionLink)
            return ReactionConnectionPoints.IndexFor(reactionLink.Type);

         return NO_CONNECTION_POINT_INDEX;
      }

      private void updateZOrder(IEnumerable<ElementBaseNode> nodes)
      {
         var orderedItems = _connectors.Values.Cast<DiagramItem>().Concat(nodes.Select(node => _shapes[node])).ToList();
         if (_diagramControl.Items.SequenceEqual(orderedItems))
            return;
         orderedItems.Each(item => _diagramControl.BringItemsToFront(new[] {item}));
      }

      private static PointCollection createMoleculeConnectionPoints()
      {
         return new PointCollection(Enumerable.Range(0, 8).Select(i =>
         {
            var angle = i * Math.PI / 4;
            return new PointFloat((float) (0.5 + 0.5 * Math.Cos(angle)), (float) (0.5 + 0.5 * Math.Sin(angle)));
         }).ToList());
      }

      private void onCustomDrawItem(CustomDrawItemEventArgs e)
      {
         if (!(e.Item.Tag is ElementBaseNode node))
            return;

         e.DefaultDraw(CustomDrawItemMode.All);
         var reactionNode = node as ReactionNode;
         if (reactionNode != null)
            drawReactionPorts(e.Graphics, reactionNode, e.Size);
         else
            drawInnerEllipse(e.Graphics, node, e.Size);

         drawLabel(e.Graphics, node, e.Size, labelBelow: reactionNode != null);
         e.Handled = true;
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

      private RectangleF labelBounds(Graphics graphics, ElementBaseNode node, DiagramShape shape)
      {
         if (!hasLabel(node))
            return shape.Bounds;

         using (var font = labelFont(node))
         {
            var textSize = graphics.MeasureString(node.Name, font);
            var location = labelLocation(shape.Size, textSize, labelBelow: node is ReactionNode);
            return new RectangleF(shape.X + location.X, shape.Y + location.Y, textSize.Width, textSize.Height);
         }
      }

      private RectangleF drawingBounds() => drawingBounds(_shapes.ToList());

      private RectangleF drawingBounds(IReadOnlyList<KeyValuePair<IBaseNode, DiagramShape>> shapes)
      {
         using (var bitmap = new Bitmap(1, 1))
         using (var graphics = Graphics.FromImage(bitmap))
         {
            var labels = shapes.Select(x => labelBounds(graphics, (ElementBaseNode) x.Key, x.Value));
            var bounds = shapes.Select(x => (RectangleF) x.Value.Bounds).Concat(labels).Aggregate(RectangleF.Union);
            bounds.Inflate(EXPORT_MARGIN, EXPORT_MARGIN);
            return bounds;
         }
      }

      private IReadOnlyList<KeyValuePair<IBaseNode, DiagramShape>> shapesToExport(IContainerBase containerBase)
      {
         if (containerBase == null || containerBase is IDiagramModel)
            return _shapes.ToList();

         var exported = containerBase.GetAllChildren<IBaseNode>().ToList();
         return _shapes.Where(x => exported.Contains(x.Key)).ToList();
      }

      private void onGetActiveObjectInfo(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != _diagramControl)
            return;

         var node = itemAt(e.ControlMousePosition)?.Tag as ElementBaseNode;
         if (node == null)
            return;

         var lines = new List<string> {node.Description};
         if (node is MoleculeNode && node.CanLink)
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

         var movedNodes = e.Items.Where(x => x.Item.Tag is ElementBaseNode).ToList();
         if (!movedNodes.Any())
            return;

         _syncing = true;
         try
         {
            _model.StartTransaction();
            movedNodes.Each(x =>
            {
               var node = (ElementBaseNode) x.Item.Tag;
               node.Location = new PointF(x.NewDiagramPosition.X + node.Size.Width / 2, x.NewDiagramPosition.Y + node.Size.Height / 2);
            });
            _model.FinishTransaction("Move");
         }
         finally
         {
            _syncing = false;
         }

         _presenter.SelectionMoved(this, EventArgs.Empty);
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
            defer(() => _diagramControl.Items.Remove(e.Item));
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
         var fromPort = portFor(fromNode, connector.BeginItem as DiagramShape, connector.BeginItemPointIndex, connector.BeginPoint);
         var toPort = portFor(toNode, connector.EndItem as DiagramShape, connector.EndItemPointIndex, connector.EndPoint);
         _diagramControl.Items.Remove(connector);
         if (_readOnly || fromNode == null || toNode == null)
            return;

         OnLinkCreated(fromNode, toNode, fromPort, toPort);
      }

      private static ElementBaseNode nodeOf(IDiagramItem item) => (item as DiagramItem)?.Tag as ElementBaseNode;

      private static object portFor(ElementBaseNode node, DiagramShape shape, int pointIndex, PointFloat point)
      {
         if (!(node is ReactionNode reactionNode))
            return null;
         return ReactionConnectionPoints.LinkTypeFor(pointIndex) ?? nearestLinkType(reactionNode, shape, point);
      }

      private static ReactionLinkType nearestLinkType(ReactionNode node, DiagramShape shape, PointFloat point)
      {
         var connectionPoints = ReactionConnectionPoints.For(node.DisplayEductsRight);
         var nearestIndex = Enumerable.Range(0, connectionPoints.Count)
            .OrderBy(i => distance(new PointF(point.X, point.Y), new PointF(shape.X + connectionPoints[i].X * shape.Width, shape.Y + connectionPoints[i].Y * shape.Height)))
            .First();
         return ReactionConnectionPoints.LinkTypeFor(nearestIndex).Value;
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
            var allowed = !_readOnly && node.CanLink && (oppositeNode == null || (oppositeNode.CanLink && !sameKind(node, oppositeNode)));
            if (allowed)
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
         e.Cancel = !isValidConnection(newNode, e.NewIndex, oppositeNode, oppositePointIndex);
      }

      private static bool isValidConnection(ElementBaseNode node, int pointIndex, ElementBaseNode oppositeNode, int oppositePointIndex)
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
         return _shapes.Any(x => x.Key is ElementBaseNode node && node.CanLink && isOverPortOf(node, x.Value, point));
      }

      private static bool isOverPortOf(ElementBaseNode node, DiagramShape shape, PointF point)
      {
         if (node is ReactionNode reactionNode)
            return ReactionConnectionPoints.For(reactionNode.DisplayEductsRight)
               .Any(relativePoint => distance(point, new PointF(shape.X + relativePoint.X * shape.Width, shape.Y + relativePoint.Y * shape.Height)) <= PORT_SIZE);

         var center = new PointF(shape.X + shape.Width / 2, shape.Y + shape.Height / 2);
         var radius = Math.Min(shape.Width, shape.Height) / 2;
         var distanceToCenter = distance(point, center);
         return distanceToCenter <= radius + PORT_SIZE / 2 && distanceToCenter >= radius / 2;
      }

      private static float distance(PointF first, PointF second)
      {
         return (float) Math.Sqrt(Math.Pow(first.X - second.X, 2) + Math.Pow(first.Y - second.Y, 2));
      }

      private void onMouseUp(MouseEventArgs e)
      {
         if (e.Button == MouseButtons.Left)
         {
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

         var documentPoint = _diagramControl.PointToDocument(new PointFloat(e.Location));
         _presenter.ShowContextMenu(item?.Tag as IBaseNode, e.Location, new PointF(documentPoint.X, documentPoint.Y));
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
            case IBaseNode node when _shapes.TryGetValue(node, out var shape):
               return shape;
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

         var bounds = item.Bounds;
         _diagramControl.ScrollToPoint(new PointFloat(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2), HorzAlignment.Center, VertAlignment.Center);
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
         if (_shapes.Count == 0)
            return new Bitmap(1, 1);

         var exportBounds = drawingBounds(shapesToExport(containerBase));
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
