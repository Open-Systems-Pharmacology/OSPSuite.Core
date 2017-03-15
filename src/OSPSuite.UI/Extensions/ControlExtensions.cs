using System;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Extensions
{
   public static class ControlExtensions
   {
      /// <summary>
      ///    Fill the given <paramref name="control" /> with the <paramref name="viewToAdd" />.
      /// </summary>
      /// <param name="control">Parent control to fill</param>
      /// <param name="viewToAdd">View that will be added in the control. The view will fill the available space</param>
      public static void FillWith(this Control control, IView viewToAdd)
      {
         control.FillWith(viewToAdd as Control);
      }

      public static void FillWith(this PopupContainerControl popupController, IView viewToAdd)
      {
         popupController.FillWith(viewToAdd as Control);
      }

      /// <summary>
      ///    Fill the given <paramref name="popupController" /> with the <paramref name="controlToAdd" />
      /// </summary>
      /// <param name="popupController">
      ///    Popup container control to fill.The popup control size will be adjusted to fit the
      ///    <paramref name="controlToAdd" />
      /// </param>
      /// <param name="controlToAdd">Control that will be added in the control.The control will fill the available space</param>
      public static void FillWith(this PopupContainerControl popupController, Control controlToAdd)
      {
         if (controlToAdd == null) return;
         var size = controlToAdd.Size;
         //necessary to cast as Control to avoid loop
         FillWith((Control) popupController, controlToAdd);
         popupController.Size = size;
      }

      /// <summary>
      ///    Fill the given <paramref name="control" /> with the <paramref name="controlToAdd" />
      /// </summary>
      /// <param name="control">Parent control to fill</param>
      /// <param name="controlToAdd">Control that will be added in the control.The control will fill the available space</param>
      public static void FillWith(this Control control, Control controlToAdd)
      {
         if (controlToAdd == null) return;

         controlToAdd.Dock = DockStyle.Fill;
         control.Controls.Add(controlToAdd);
         control.Refresh();
         control.Margin = new Padding(0);
         controlToAdd.Padding = new Padding(0);
         controlToAdd.Margin = new Padding(0);

         var panel = control as PanelControl;

         //only for standard panel. Group Control generally displays a caption and border should be visible
         if (panel != null && !panel.IsAnImplementationOf<GroupControl>())
            panel.BorderStyle = BorderStyles.NoBorder;

         if (control.Controls.Count == 1) return;
         //remove any controls that was already there to release controls
         //properly and avoid too many controls in the parent control
         control.Controls.RemoveAt(0);
      }

      /// <summary>
      ///    Clear the control content
      /// </summary>
      public static void Clear(this Control control)
      {
         control.Controls.Clear();
      }

      public static void HideBorderIfRequired(this LayoutControlGroup layoutControlGroup)
      {
         /*hide the border if the selected skin is one that does not support border layout*/
         if (UIConstants.Skins.SkinsWithoutBorder.Contains(UserLookAndFeel.Default.SkinName))
            layoutControlGroup.GroupBordersVisible = false;
      }

      public static void DoWithinSuspendedLayout(this Control control, Action actionToBatch)
      {
         try
         {
            control.SuspendLayout();
            actionToBatch();
         }
         finally
         {
            control.ResumeLayout();
         }
      }

      public static void DoWithinWaitCursor(this Control control, Action actionToPerform)
      {
         var currentCursor = control.Cursor;
         try
         {
            if (currentCursor != Cursors.WaitCursor)
               control.Cursor = Cursors.WaitCursor;

            control.DoWithinExceptionHandler(actionToPerform);
         }
         finally
         {
            if (currentCursor != Cursors.WaitCursor)
               control.Cursor = currentCursor;
         }
      }
   }
}