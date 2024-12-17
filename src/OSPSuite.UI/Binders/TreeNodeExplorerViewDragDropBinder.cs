using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Binders
{
   public class TreeNodeExplorerViewDragDropBinder
   {
      private readonly UxTreeView _treeView;
      private TreeListHitInfo _hitInfo;
      private ICanDragDropPresenter _presenter;
      private DragDropInfo _dragDropInfo;
      private DragDropEffects _supportedEffects;

      public TreeNodeExplorerViewDragDropBinder(UxTreeView treeView)
      {
         _treeView = treeView;
      }

      public void InitializeDragAndDrop(ICanDragDropPresenter presenter)
      {
         _presenter = presenter;
         _treeView.CalcNodeDragImageIndex += (o, e) => onEvent(calcNodeDragImageIndex, e);

         _treeView.OptionsDragAndDrop.DragNodesMode = DragNodesMode.Multiple;
         _treeView.AllowDrop = true;

         _treeView.DragOver += (o, e) => onEvent(treeViewDragOver, e);
         _treeView.DragDrop += (o, e) => onEvent(treeViewDragDrop, e);

         _supportedEffects = DragDropEffects.Move;
         if (_presenter.CopyAllowed())
            _supportedEffects = DragDropEffects.Copy;
      }

      private void treeViewDragDrop(DragEventArgs e)
      {
         var p = clientPointFrom(e);
         var targetNode = _treeView.CalcHitInfo(p).Node;

         var draggedNodes = getDraggedNodesFrom(e);
         var keyFlags = e.AsKeyFlags();

         draggedNodes.Each(node => _presenter.DropNode(node, nodeFrom(targetNode), keyFlags));

         e.Effect = DragDropEffects.None;
      }

      private Point clientPointFrom(DragEventArgs e)
      {
         return _treeView.PointToClient(new Point(e.X, e.Y));
      }

      private static IEnumerable<ITreeNode> getDraggedNodesFrom(DragEventArgs e)
      {
         return e.Data<IEnumerable<ITreeNode>>();
      }

      private void treeViewDragOver(DragEventArgs e)
      {
         var draggedNodes = getDraggedNodesFrom(e);
         e.Effect = getDragDropEffect(draggedNodes, e.AsKeyFlags());
      }

      private void calcNodeDragImageIndex(CalcNodeDragImageIndexEventArgs e)
      {
         var selectedNodes = getSelectedTreeNodes();

         var dragDropEffects = getDragDropEffect(selectedNodes, e.DragArgs.AsKeyFlags());
         switch (dragDropEffects)
         {
            case DragDropEffects.None:
               e.ImageIndex = -1; // no icon
               break;
            case DragDropEffects.Copy:
               e.ImageIndex = presenterSupportsCopy() ? 
                  1 : // the copy icon (a plus sign)
                  2; // ctrl key has no effect
               break;
            default:
               e.ImageIndex = 2; // the reorder icon (a curved arrow)
               break;
         }
      }

      private bool presenterSupportsCopy()
      {
         return _supportedEffects.HasFlag(DragDropEffects.Copy);
      }

      private IReadOnlyList<ITreeNode> getSelectedTreeNodes()
      {
         return _treeView.Selection.ToList().Select(nodeFrom).ToList();
      }

      /// <summary>
      ///    Retrieve the desired node effect depending on the current mouse position
      /// </summary>
      private DragDropEffects getDragDropEffect(IEnumerable<ITreeNode> dragNodes, DragDropKeyFlags keyFlags)
      {
         if (dragNodes == null)
            return DragDropEffects.None;

         var p = _treeView.PointToClient(Control.MousePosition);
         var targetNode = _treeView.CalcHitInfo(p).Node;

         var treeNodes = dragNodes.ToList();

         if (!treeNodes.All(_presenter.CanDrag))
            return DragDropEffects.None;

         if (treeNodes.All(node => _presenter.CanDrop(node, nodeFrom(targetNode), keyFlags)))
            return keyFlags.HasFlag(DragDropKeyFlags.CtrlKey) && presenterSupportsCopy() ? DragDropEffects.Copy : DragDropEffects.Move;

         return DragDropEffects.None;
      }

      private ITreeNode nodeFrom(TreeListNode treeListNode)
      {
         return _treeView.NodeFrom(treeListNode);
      }

      public virtual void TreeMouseDown(MouseEventArgs e)
      {
         _hitInfo = null;
         var hitInfo = _treeView.CalcHitInfo(new Point(e.X, e.Y));
         if (hitInfo == null) return;
         if (hitInfo.Node == null) return;
         if (!_presenter.CanDrag(nodeFrom(hitInfo.Node))) return;
         _hitInfo = hitInfo;
      }

      public virtual void TreeMouseMove(MouseEventArgs e)
      {
         if (_hitInfo == null) return;
         if (_hitInfo.Node == null) return;
         if (e.Button != MouseButtons.Left) return;
         if (mouseDidNotReallyMove(e, _hitInfo)) return;

         _dragDropInfo = new DragDropInfo(getSelectedTreeNodes());
         try
         {
            _treeView.DoDragDrop(_dragDropInfo, _supportedEffects);
            DXMouseEventArgs.GetMouseArgs(e).Handled = true;
         }
         finally
         {
            _hitInfo = null;
            _dragDropInfo = null;
         }
      }

      private bool mouseDidNotReallyMove(MouseEventArgs e, TreeListHitInfo hitInfo)
      {
         return
            areComparable(e.X, hitInfo.MousePoint.X) &&
            areComparable(e.Y, hitInfo.MousePoint.Y);
      }

      private bool areComparable(int value1InPixel, int value2InPixel)
      {
         return Math.Abs(value1InPixel - value2InPixel) <= 1;
      }

      private void onEvent<TEventArgs>(Action<TEventArgs> action, TEventArgs e)
      {
         this.DoWithinExceptionHandler(() => action(e));
      }
   }
}