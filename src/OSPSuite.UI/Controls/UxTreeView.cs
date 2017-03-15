using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Mappers;

namespace OSPSuite.UI.Controls
{
   /// <summary>
   ///    <inheritdoc />
   /// </summary>
   public class UxTreeView : UxTreeList, IUxTreeView, ILatchable
   {
      private const string _infoColumn = "info";
      public event Action<ITreeNode> NodeDoubleClick = delegate { };
      public event Action<MouseEventArgs, ITreeNode> NodeClick = delegate { };
      public event Action<ITreeNode> SelectedNodeChanged = delegate { };
      public event Action<NodeChangingEventArgs> SelectedNodeChanging = delegate { };
      private readonly ICache<string, TreeListNode> _allNodes;
      private readonly Func<string, TreeListNode> _onMisingKey = s => null;
      private bool _shouldExpandAddedNode;
      private bool _useLazyLoading;
      private readonly ToolTipPartsToSuperToolTipMapper _toolTipMapper;
      public Func<ITreeNode, IEnumerable<ToolTipPart>> ToolTipForNode { get; set; }

      public bool IsLatched { get; set; }

      public UxTreeView()
      {
         DataColumn = Columns.Add();
         DataColumn.Name = "colInfo";
         DataColumn.FieldName = _infoColumn;
         DataColumn.Visible = true;
         DataColumn.VisibleIndex = 0;
         _allNodes = new Cache<string, TreeListNode>(_onMisingKey);
         FocusedNodeChanged += (o, e) => this.DoWithinLatch(() => onFocusedNodeChanged(o, e));
         MouseClick += mouseClickEvent;
         MouseDoubleClick += mouseDoubleClickEvent;
         BeforeExpand += beforeNodeExpand;
         NodeCellStyle += nodeCellStyleEvent;
         OptionsView.ShowColumns = false;
         OptionsView.ShowIndicator = false;
         OptionsView.ShowHorzLines = false;
         OptionsView.ShowVertLines = false;
         OptionsBehavior.Editable = false;
         ShouldExpandAddedNode = true;
         UseLazyLoading = false;
         _toolTipMapper = new ToolTipPartsToSuperToolTipMapper();
         CustomDrawNodeCell += dxTreeViewCustomDrawNodeCell;
         ToolTipForNode = node => node.ToolTip;
         ToolTipController = new ToolTipController();
         ToolTipController.GetActiveObjectInfo += (o, e) => ShowToolTip(e);
      }

      public TreeListColumn DataColumn { get; }

      public SuperToolTip ShowToolTip(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != this || ToolTipForNode == null)
            return null;

         var hit = CalcHitInfo(e.ControlMousePosition);

         if (hit.HitInfoType != HitInfoType.Cell)
            return null;

         var node = NodeFrom(hit.Node);
         if (node == null)
            return null;

         var cellInfo = new TreeListCellToolTipInfo(hit.Node, hit.Column, null);
         var info = new ToolTipControlInfo(cellInfo, string.Empty)
         {
            SuperTip = _toolTipMapper.MapFrom(ToolTipForNode(node))
         };

         e.Info = info;

         return info.SuperTip;
      }

      private void beforeNodeExpand(object sender, BeforeExpandEventArgs e)
      {
         if (!e.CanExpand) return;
         if (!UseLazyLoading) return;

         var nodeToExpand = NodeFrom(e.Node);
         foreach (var child in nodeToExpand.Children)
         {
            var childNode = NodeFrom(child);
            //already added and expended!
            if (childNode != null && childNode.HasChildren)
               continue;

            child.Children.Each(AddNode);
         }
      }

      private void mouseDoubleClickEvent(object sender, MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Left) return;
         var hitInfo = CalcHitInfo(new Point(e.X, e.Y));
         if (hitInfo?.Node == null) return;
         SetFocusedNode(hitInfo.Node);
         NodeDoubleClick(NodeFrom(hitInfo.Node));
      }

      private void nodeCellStyleEvent(object sender, GetCustomNodeCellStyleEventArgs e)
      {
         var node = NodeFrom(e.Node);
         if (node == null) return;
         if (node.ForeColor == Color.Empty)
            return;

         e.Appearance.ForeColor = node.ForeColor;
      }

      private void onFocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
      {
         var nodeChangingEventArgs = new NodeChangingEventArgs(NodeFrom(e.Node), NodeFrom(e.OldNode));
         SelectedNodeChanging(nodeChangingEventArgs);
         //Node changing cancel, reselect the old node, without raising event
         if (nodeChangingEventArgs.Cancel)
         {
            FocusedNode = e.OldNode;
            return;
         }

         SelectedNodeChanged(NodeFrom(e.Node));
      }

      private void dxTreeViewCustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
      {
         if (!Selection.Contains(e.Node)) return;

         var brush = getSolidBrush(SystemColors.Window);
         e.Graphics.FillRectangle(brush, e.Bounds);

         var highlightBrush = getSolidBrush(SystemColors.Highlight);
         var measureString = e.Graphics.MeasureString(e.CellText, e.Appearance.Font);
         var stringTop = e.Bounds.Y + Convert.ToInt32((e.Bounds.Height - measureString.Height) / 2);

         var stringRectangle = new Rectangle(e.EditViewInfo.ContentRect.Left, stringTop,
            Convert.ToInt32(measureString.Width + 1),
            Convert.ToInt32(measureString.Height));

         e.Graphics.FillRectangle(highlightBrush, stringRectangle);

         SolidBrush textBrush = getSolidBrush(SystemColors.HighlightText);
         e.Graphics.DrawString(e.CellText, e.Appearance.Font, textBrush, stringRectangle);
         e.Handled = true;
      }

      private SolidBrush getSolidBrush(Color systemColor)
      {
         var color = LookAndFeelHelper.GetSystemColor(LookAndFeel, systemColor);
         return new SolidBrush(color);
      }

      private void mouseClickEvent(object sender, MouseEventArgs e)
      {
         var hitInfo = CalcHitInfo(new Point(e.X, e.Y));
         if (hitInfo?.Node == null) return;
         SetFocusedNode(hitInfo.Node);
         NodeClick(e, NodeFrom(hitInfo.Node));
      }

      public void DestroyAllNodes()
      {
         removeAllNodes(true);
      }

      public void Clear()
      {
         removeAllNodes(false);
      }

      private void removeAllNodes(bool deleteNodes)
      {
         //in latch avoid event while clearing the tree
         DoWithinBatchUpdate(() =>
            this.DoWithinLatch(() =>
            {
               for (int i = Nodes.Count - 1; i >= 0; i--)
               {
                  removeNode(NodeFrom(Nodes[i]), deleteNodes);
               }
               _allNodes.Clear();
            }));
      }

      public bool IsNodeExpanded(ITreeNode treeNode)
      {
         var node = NodeFrom(treeNode);
         return node != null && node.Expanded;
      }

      public void AddNode(ITreeNode nodeToAdd)
      {
         DoWithinBatchUpdate(() =>
         {
            TreeListNode addedNode;
            var created = false;
            if (nodeExists(nodeToAdd))
               addedNode = NodeFrom(nodeToAdd);
            else
            {
               addedNode = addNodeToNodeCollection(NodeFrom(nodeToAdd.ParentNode), nodeToAdd);
               created = true;
            }

            //Add children nodes
            if (shouldAddChildren(addedNode))
               nodeToAdd.Children.Each(AddNode);

            if (created)
               addedNode.Expanded = ShouldExpandAddedNode;
         });
      }

      private bool shouldAddChildren(TreeListNode addedNode)
      {
         if (UseLazyLoading)
            return addedNode.ParentNode == null || addedNode.ParentNode.Expanded;

         return true;
      }

      /// <summary>
      ///    Gets or sets a value indicating whether to use lazy loading.
      /// </summary>
      /// <value>
      ///    <c>true</c> if using lazy loading; otherwise, <c>false</c>.
      ///    default: <c>false</c>
      /// </value>
      /// <remarks>
      ///    Ensure that the used <see cref="ITreeNode" /> also supports lazy loading
      ///    UseLazyLoading =true with ShouldExpandAddedNodes = true makes no sense.
      /// </remarks>
      public bool UseLazyLoading
      {
         get { return _useLazyLoading; }
         set
         {
            _useLazyLoading = value;
            if (_useLazyLoading)
               ShouldExpandAddedNode = false;
         }
      }

      private TreeListNode addNodeToNodeCollection(TreeListNode parentNode, ITreeNode node)
      {
         var addedNode = AppendNode(null, parentNode);
         setNodeText(addedNode, node.Text);
         setNodeIcon(addedNode, node.Icon);
         node.TextChanged += nodeTextChanged;
         node.IconChanged += iconChanged;
         node.ForeColorChanged += foreColorChanged;
         addedNode.Tag = node;
         addNode(node.Id, addedNode);
         //now if the node was not available in the parent node (for example after a merge,add on the parent node as well)
         var parentTreeNode = NodeFrom(parentNode);
         if (parentTreeNode != null && !parentTreeNode.Children.Contains(node))
            parentTreeNode.AddChild(node);

         addedNode.Expanded = ShouldExpandAddedNode;
         return addedNode;
      }

      private void addNode(string id, TreeListNode node)
      {
         _allNodes.Add(id, node);
      }

      private void iconChanged(ITreeNode node)
      {
         setNodeIcon(NodeFrom(node), node.Icon);
      }

      private void foreColorChanged(ITreeNode node)
      {
         RefreshNode(NodeFrom(node));
      }

      private void nodeTextChanged(ITreeNode node)
      {
         setNodeText(NodeFrom(node), node.Text);
      }

      private void setNodeText(TreeListNode node, string text)
      {
         node?.SetValue(_infoColumn, text);
      }

      private void setNodeIcon(TreeListNode node, ApplicationIcon icon)
      {
         if (icon == null) return;
         node.StateImageIndex = icon.Index;
      }

      public ITreeNode NodeById(string id)
      {
         var nodeWithId = treeListNodeById(id);
         return nodeWithId?.Tag as ITreeNode;
      }

      private TreeListNode treeListNodeById(string id)
      {
         return _allNodes[id];
      }

      /// <summary>
      ///    This will remove treeNode and associated controls from local cache.
      ///    It will not affect the treeNode structure. Use this if you intend to reuse the treeNode.
      ///    Otherwise use <seealso cref="RemoveNode" />
      /// </summary>
      /// <param name="treeNode">The ITreeNode that will be removed from cache</param>
      public void RemoveNode(ITreeNode treeNode)
      {
         if (treeNode == null) return;
         DoWithinBatchUpdate(() => removeNode(treeNode, false));
      }

      /// <summary>
      ///    This will remove treeNode and associated controls from the local cache.
      ///    It will also unlink and remove the descendants of the treeNode being removed. To keep the descendency in tact,
      ///    use <seealso cref="RemoveNode" />
      /// </summary>
      /// <param name="treeNode">The ITreeNode that will be removed from cache</param>
      public void DestroyNode(ITreeNode treeNode)
      {
         if (treeNode == null) return;
         DoWithinBatchUpdate(() => removeNode(treeNode, true));
      }

      private void removeNode(ITreeNode treeNode, bool deleteNode)
      {
         deleteTreeListNode(NodeFrom(treeNode));
         treeNode.TextChanged -= nodeTextChanged;
         treeNode.IconChanged -= iconChanged;
         if (deleteNode)
            treeNode.Delete();
      }

      public void DestroyNode(string id)
      {
         removeNode(treeListNodeById(id));
      }

      private void removeNode(TreeListNode node)
      {
         DestroyNode(NodeFrom(node));
      }

      private bool nodeExists(ITreeNode node)
      {
         return _allNodes.Contains(node.Id);
      }

      public ITreeNode NodeFrom(TreeListNode node)
      {
         return node?.Tag as ITreeNode;
      }

      public TreeListNode NodeFrom(ITreeNode node)
      {
         return node == null ? null : treeListNodeById(node.Id);
      }

      public ITreeNode NodeWithTag(object tag)
      {
         return _allNodes.Select(NodeFrom)
            .FirstOrDefault(node => Equals(node.TagAsObject, tag));
      }

      private void deleteTreeListNode(TreeListNode node)
      {
         //node does not exist. Nothing to remove
         if (node == null) return;

         for (int i = node.Nodes.Count - 1; i >= 0; i--)
         {
            deleteTreeListNode(node.Nodes[i]);
         }

         var treeNode = NodeFrom(node);
         //we have to remove from the actual tree view, and from our intern collection of treeNode, key
         _allNodes.Remove(treeNode.Id);

         if (node.ParentNode == null)
            Nodes.Remove(node);
         else
            node.ParentNode.Nodes.Remove(node);

         node.Tag = null;
      }

      protected override void Dispose(bool disposing)
      {
         DestroyAllNodes();
         base.Dispose(disposing);
      }

      public void ExpandNode(ITreeNode nodeToExpand)
      {
         var treeListNode = NodeFrom(nodeToExpand);
         if (treeListNode == null)
            return;

         treeListNode.Expanded = true;
         if (!UseLazyLoading)
            return;

         nodeToExpand.Children.Each(AddNode);
      }

      public void CollapseNode(ITreeNode nodeToCollapse)
      {
         var treeListNode = NodeFrom(nodeToCollapse);
         if (treeListNode == null)
            return;

         treeListNode.Expanded = false;
      }

      public void ToggleExpandState(ITreeNode node)
      {
         if (IsNodeExpanded(node))
            CollapseNode(node);
         else
            ExpandNode(node);
      }

      public void SelectFocusedNodeOrFirst()
      {
         var focusedNode = FocusedNode;
         if (focusedNode == null)
         {
            //no focuses node so far. Select the first one
            MoveFirst();
            return;
         }

         //we already have a focused node
         //necessary to trigger events
         FocusedNode = null;
         FocusedNode = focusedNode;
      }

      public void SelectNode(ITreeNode nodeToSelect)
      {
         FocusedNode = NodeFrom(nodeToSelect);
         SelectFocusedNodeOrFirst();
      }

      public ITreeNode SelectedNode
      {
         get
         {
            var selectedNode = FocusedNode;
            return selectedNode == null ? null : NodeFrom(selectedNode);
         }
      }

      public bool ShouldExpandAddedNode
      {
         private get { return _shouldExpandAddedNode; }
         set
         {
            _shouldExpandAddedNode = value;
            if (_shouldExpandAddedNode)
            {
               UseLazyLoading = false;
            }
         }
      }

      public void ExpandAllNodes()
      {
         ExpandAll();
      }

      public IEnumerable<ITreeNode> RootNodes => from TreeListNode node in Nodes select NodeFrom(node);

      public void Sort()
      {
         BeginSort();
         try
         {
            DataColumn.SortOrder = SortOrder.Ascending;
         }
         finally
         {
            EndSort();
         }
      }
   }
}