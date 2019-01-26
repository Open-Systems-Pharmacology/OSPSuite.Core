using System;
using System.Drawing;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.ViewInfo;

namespace OSPSuite.UI.Controls
{
   
   public partial class UxTreeList : TreeList
   {
      public UxTreeList()
      {
         InitializeComponent();
      }

      // see https://github.com/DevExpress-Examples/how-to-get-rid-of-the-space-reserved-for-an-image-in-a-treelistnode-e2153

      protected override TreeListViewInfo CreateViewInfo()
      {
         return new TreeListViewInfoEmptyIcon(this);
      }

      public class TreeListViewInfoEmptyIcon : TreeListViewInfo
      {
         public TreeListViewInfoEmptyIcon(TreeList treeList) : base(treeList)
         {
         }

         protected override void CalcSelectImageBounds(RowInfo rInfo, Rectangle indentBounds)
         {
            base.CalcSelectImageBounds(rInfo, indentBounds);
            if (-1 == rInfo.SelectImageIndex) rInfo.SelectImageBounds = Rectangle.Empty;
         }

         protected override void CalcStateImageBounds(RowInfo rInfo, Rectangle indentBounds)
         {
            base.CalcStateImageBounds(rInfo, indentBounds);
            if ( -1 == rInfo.StateImageIndex) rInfo.StateImageBounds = Rectangle.Empty;
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