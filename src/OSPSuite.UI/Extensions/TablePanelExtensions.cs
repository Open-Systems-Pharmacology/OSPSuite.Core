using System.Windows.Forms;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Extensions
{
   public static class TablePanelExtensions
   {
      public static void AdjustButtonWithImageOnly(this TablePanel tablePanel, SimpleButton button)
      {
         tablePanel.AdjustButtonSize(button, UIConstants.Size.RADIO_GROUP_HEIGHT, UIConstants.Size.BUTTON_HEIGHT);
      }

      public static void AdjustLongButtonWidth(this TablePanel tablePanel, SimpleButton button)
      {
         tablePanel.AdjustButtonSize(button, UIConstants.Size.LARGE_BUTTON_WIDTH);
      }

      public static void AdjustButton(this TablePanel tablePanel, SimpleButton button)
      {
         tablePanel.AdjustButtonSize(button, UIConstants.Size.BUTTON_WIDTH, UIConstants.Size.BUTTON_HEIGHT);
      }

      /// <summary>
      ///    Adjust the width and height of the column where the <paramref name="button" /> is defined.
      /// </summary>
      /// <param name="tablePanel">TablePanel containing the button</param>
      /// <param name="button">button to resize</param>
      /// <param name="width">Optional width of the button. If null, the width will remain as is</param>
      /// <param name="height">Optional height of the button. If null, the height will remain as is</param>
      public static void AdjustButtonSize(this TablePanel tablePanel, SimpleButton button, int? width = null, int? height = null)
      {
         var row = tablePanel.RowFor(button);
         var col = tablePanel.ColumnFor(button);
         if (width.HasValue)
         {
            col.Style = TablePanelEntityStyle.Absolute;
            col.Width = width.Value + button.Margin.Horizontal;
         }

         if (height.HasValue)
         {
            row.Style = TablePanelEntityStyle.Absolute;
            row.Height = height.Value + button.Margin.Vertical;
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