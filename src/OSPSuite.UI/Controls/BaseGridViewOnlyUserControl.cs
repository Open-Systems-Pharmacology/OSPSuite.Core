﻿using DevExpress.XtraLayout.Utils;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Controls
{
   public partial class BaseGridViewOnlyUserControl : BaseResizableUserControl
   {
      public BaseGridViewOnlyUserControl()
      {
         InitializeComponent();
         gridView.AllowsFiltering = false;
         gridView.ShowRowIndicator = false;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemGrid.TextVisible = false;
      }

      public override int OptimalHeight
      {
         get
         {
            if (GridVisible)
               return gridView.OptimalHeight + layoutItemGrid.Padding.Height;

            return 0;
         }
      }

      protected bool GridVisible
      {
         get => layoutItemGrid.Visible;
         set => layoutItemGrid.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
      }

      public override void AdjustHeight()
      {
         layoutItemGrid.AdjustGridViewHeight(gridView, layoutControl);
         base.AdjustHeight();
      }

      public override void Repaint()
      {
         gridView.LayoutChanged();
      }
   }
}