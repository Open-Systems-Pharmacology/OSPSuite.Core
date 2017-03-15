using System;
using System.Drawing;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.ViewInfo;

namespace OSPSuite.UI.Controls
{
   public partial class UxTreeList : DevExpress.XtraTreeList.TreeList
   {
      public UxTreeList()
      {
         InitializeComponent();
      }

      protected override TreeListViewInfo CreateViewInfo()
      {
         return new TreeListViewInfoEmptyIcon(this);
      }

      private class TreeListViewInfoEmptyIcon : TreeListViewInfo
      {
         public TreeListViewInfoEmptyIcon(DevExpress.XtraTreeList.TreeList treeList) : base(treeList)
         {
         }

         protected override Point GetDataBoundsLocation(TreeListNode node, int top)
         {
            Point result = base.GetDataBoundsLocation(node, top);
            if (Size.Empty != RC.SelectImageSize && -1 == node.SelectImageIndex)
               result.X -= RC.SelectImageSize.Width;
            if (Size.Empty != RC.StateImageSize && -1 == node.StateImageIndex)
               result.X -= RC.StateImageSize.Width;
            return result;
         }

         protected override void CalcStateImage(RowInfo ri)
         {
            base.CalcStateImage(ri);
            if (Size.Empty != RC.SelectImageSize && -1 == ri.Node.SelectImageIndex)
               ri.StateImageLocation.X -= RC.SelectImageSize.Width;
         }

         protected override void CalcSelectImage(RowInfo ri)
         {
            base.CalcSelectImage(ri);
            if (-1 == ri.Node.SelectImageIndex) ri.SelectImageLocation = Point.Empty;
         }
      }

      public void DoWithinBatchUpdate(Action action)
      {
         //in latch avoid event while clearing the tree
         BeginUnboundLoad();
         try
         {
            action();
         }
         finally
         {
            EndUnboundLoad();
         }
      }
   }
}