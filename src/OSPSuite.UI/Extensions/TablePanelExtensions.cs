using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Extensions
{
   public static class TablePanelExtensions
   {
      public static void AdjustButtonWithImageOnly(this TablePanel tablePanel, SimpleButton button)
      {
         tablePanel.AdjustControlSize(button, UIConstants.Size.RADIO_GROUP_HEIGHT, UIConstants.Size.BUTTON_HEIGHT);
      }

      public static void AdjustLongButtonWidth(this TablePanel tablePanel, SimpleButton button)
      {
         tablePanel.AdjustControlSize(button, UIConstants.Size.LARGE_BUTTON_WIDTH);
      }

      public static void AdjustButton(this TablePanel tablePanel, SimpleButton button)
      {
         tablePanel.AdjustControlSize(button, UIConstants.Size.BUTTON_WIDTH, UIConstants.Size.BUTTON_HEIGHT);
      }

      /// <summary>
      ///    Adjust the width and height of the column where the <paramref name="control" /> is defined.
      /// </summary>
      /// <param name="tablePanel">TablePanel containing the button</param>
      /// <param name="control">Control to resize</param>
      /// <param name="width">Optional width of the button. If null, the width will remain as is</param>
      /// <param name="height">Optional height of the button. If null, the height will remain as is</param>
      public static void AdjustControlSize(this TablePanel tablePanel, Control control, int? width = null, int? height = null)
      {
         var row = tablePanel.RowFor(control);
         var col = tablePanel.ColumnFor(control);
         if (width.HasValue)
         {
            col.Style = TablePanelEntityStyle.AutoSize;
            col.Width = width.Value + control.Margin.Horizontal;
            // control.MaximumSize = new Size(width.Value, control.Height);
            control.Width = width.Value;
         }

         if (height.HasValue)
         {
            row.Style = TablePanelEntityStyle.AutoSize;
            row.Height = height.Value + control.Margin.Vertical;
            // control.MaximumSize = new Size(control.Width, height.Value);
            control.Height = height.Value;
         }
      }

      /// <summary>
      ///    Returns the <see cref="TablePanelRow" /> where the <paramref name="control" /> is located
      /// </summary>
      public static TablePanelRow RowFor(this TablePanel tablePanel, Control control)
      {
         return tablePanel.Rows[tablePanel.GetRow(control)];
      }

      /// <summary>
      ///    Returns the <see cref="TablePanelColumn" /> where the <paramref name="control" /> is located
      /// </summary>
      public static TablePanelColumn ColumnFor(this TablePanel tablePanel, Control control)
      {
         return tablePanel.Columns[tablePanel.GetColumn(control)];
      }
   }
}