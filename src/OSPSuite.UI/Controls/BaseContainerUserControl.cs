using System.Windows.Forms;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Controls
{
   /// <summary>
   ///    Defines a user control that contains other user control for which the height should be calculated dynamically
   /// </summary>
   public partial class BaseContainerUserControl : BaseUserControl
   {
      public BaseContainerUserControl()
      {
         InitializeComponent();
      }

      protected LayoutControlItem AddViewTo(LayoutControlGroup layoutControlGroup, LayoutControl layoutControl, IView view)
      {
         var group = layoutControlGroup.AddGroup();
         return AddViewToGroup(group, layoutControl, view);
      }

      protected LayoutControlItem AddViewToGroup(LayoutControlGroup group, LayoutControl layoutControl, IView view)
      {
         var layoutControlItem = group.AddItem();
         group.Text = view.Caption;
         if (view.ApplicationIcon != null)
            group.CaptionImageIndex = ApplicationIcons.IconIndex(view.ApplicationIcon);

         AddViewTo(layoutControlItem, layoutControl, view);

         layoutControlItem.TextVisible = false;

         return layoutControlItem;
      }

      protected void AddViewTo(LayoutControlItem layoutControlItem, LayoutControl layoutControl, IView view)
      {
         if (layoutControlItem.Control != null)
            layoutControlItem.Control.FillWith(view);
         else
            layoutControlItem.Control = view.DowncastTo<Control>();

         var resizableView = view as IResizableView;
         if (resizableView == null) return;

         layoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         resizableView.HeightChanged += (o, e) => AdjustLayoutItemSize(layoutControlItem, layoutControl, resizableView, e.Height);
      }

      protected virtual void AdjustLayoutItemSize(LayoutControlItem layoutControlItem, LayoutControl layoutControl, IResizableView view, int height)
      {
         if (layoutControlItem == null || layoutControlItem.Parent == null)
            return;

         try
         {
            layoutControlItem.Owner.BeginUpdate();
            layoutControlItem.AdjustControlHeight(layoutControl, height);
            var group = layoutControlItem.Parent;
            //only one item in the group and the view should be hidden
            group.Visibility = LayoutVisibilityConvertor.FromBoolean(height > 0 || group.Items.Count > 1);
            view.Repaint();
         }
         finally
         {
            layoutControlItem.Owner.EndUpdate();
         }
      }

      protected void AddEmptyPlaceHolder(LayoutControl layoutControl)
      {
         var dummyGroup = layoutControl.AddGroup();
         addEmptyPlaceHolder(dummyGroup);
      }

      protected void AddEmptyPlaceHolder(LayoutControlGroup layoutControlGroup)
      {
         var dummyGroup = layoutControlGroup.AddGroup();
         addEmptyPlaceHolder(dummyGroup);
      }

      private static void addEmptyPlaceHolder(LayoutControlGroup dummyGroup)
      {
         dummyGroup.Add(new EmptySpaceItem());
         dummyGroup.TextVisible = false;
         dummyGroup.GroupBordersVisible = false;
      }
   }
}