using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraBars;
using DevExpress.XtraGrid;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views
{
   public partial class BaseExplorerView : BaseUserControl, IExplorerView, IViewWithPopup, ILatchable
   {
      protected IExplorerPresenter _presenter;
      private readonly TreeNodeExplorerViewDragDropBinder _treeNodeExplorerViewDragDrogBinder;
      public bool IsLatched { get; set; }

      public BaseExplorerView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _toolTipController.Initialize(imageListRetriever, UIConstants.TOOL_TIP_INITIAL_DELAY_LONG);
         treeView.StateImageList = imageListRetriever.AllImagesForTreeView;
         treeView.OptionsBehavior.AllowExpandOnDblClick = false;
         treeView.DataColumn.SortMode = ColumnSortMode.Custom;
         treeView.DataColumn.SortOrder = SortOrder.Ascending;
         treeView.ToolTipController = _toolTipController;
         _toolTipController.GetActiveObjectInfo += (o, e) => treeView.ShowToolTip(e);
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImagesForContextMenu};
         _treeNodeExplorerViewDragDrogBinder = new TreeNodeExplorerViewDragDropBinder(treeView);
         treeView.MouseDown += (o, e) => OnEvent(TreeMouseDown, e);
         treeView.MouseMove += (o, e) => OnEvent(TreeMouseMove, e);
         treeView.SelectionChanged += (o, e) => OnEvent(() => manageSelectedNodes());
         treeView.NodeDoubleClick += node => OnEvent(() => nodeDoubleClick(node));
         treeView.NodeClick += (e, node) => OnEvent(() => nodeClick(e, node));

         treeView.OptionsSelection.MultiSelect = true;
      }

      private void manageSelectedNodes(ITreeNode selectedNode = null, bool canClearSelection = true)
      {
         if (_presenter == null)
            return;

         this.DoWithinLatch(() =>
         {
            var multiSelectAllowed = _presenter.AllowMultiSelectFor(selectedNodes.Union(new[] {selectedNode}));

            //valid multi select. Nothing to do
            if (multiSelectAllowed && isMultiSelectModifier(ModifierKeys))
               return;

            // Not a valid multiselect scenario or clear allowed, just do it
            if (!multiSelectAllowed || canClearSelection)
               resetTreeViewSelection();
         });
      }

      private void resetTreeViewSelection()
      {
         treeView.Selection.Clear();

         // and only the focused node selected
         if (treeView.FocusedNode != null)
            treeView.Selection.Add(treeView.FocusedNode);
      }

      private bool isMultiSelectModifier(Keys modifierKeys)
      {
         return (modifierKeys == Keys.Control || modifierKeys == Keys.Shift);
      }

      private IReadOnlyList<ITreeNode> selectedNodes => treeView.Selection.ToList().Select(treeView.NodeFrom).ToList();

      protected virtual void TreeMouseMove(MouseEventArgs e)
      {
         _treeNodeExplorerViewDragDrogBinder.TreeMouseMove(e);
      }

      protected virtual void TreeMouseDown(MouseEventArgs e)
      {
         _treeNodeExplorerViewDragDrogBinder.TreeMouseDown(e);
      }

      protected void AttachPresenter(IExplorerPresenter presenter)
      {
         _presenter = presenter;
         _treeNodeExplorerViewDragDrogBinder.InitializeDragAndDrop(_presenter);
         treeView.ToolTipForNode = presenter.ToolTipFor;
      }

      private void nodeClick(MouseEventArgs e, ITreeNode selectedNode)
      {
         var showingPopup = e.Button == MouseButtons.Right;
         var previouslySelectedNode = selectedNodes;
         var canClearSelection = previouslySelectedNode.Count == 1 || !showingPopup;

         //this call may  reset the current selection of node
         manageSelectedNodes(selectedNode, canClearSelection);

         if (!showingPopup)
            return;

         var currentlySelectedNode = selectedNodes;
         if (currentlySelectedNode.Count > 1)
            _presenter.CreatePopupMenuFor(currentlySelectedNode).At(e.Location);
         else
            _presenter.CreatePopupMenuFor(selectedNode).At(e.Location);
      }

      private void nodeDoubleClick(ITreeNode node)
      {
         this.DoWithinWaitCursor(() => _presenter.NodeDoubleClicked(node));
      }

      public void Display()
      {
         //nothing to do
      }

      public IUxTreeView TreeView => treeView;

      public ITreeNode AddNode(ITreeNode nodeToAdd)
      {
         treeView.AddNode(nodeToAdd);
         return nodeToAdd;
      }

      public BarManager PopupBarManager { get; }

      public void BeginUpdate()
      {
         treeView.BeginUpdate();
         Updating = true;
      }

      public void EndUpdate()
      {
         treeView.EndUpdate();
         Updating = false;
      }

      public bool Updating { get; private set; }
   }
}