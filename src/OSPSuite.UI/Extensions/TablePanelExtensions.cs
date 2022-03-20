using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils.Layout;
using DevExpress.XtraEditors;

namespace OSPSuite.UI.Extensions
{
   public static class TablePanelExtensions
   {
      public static TablePanel WithAbsoluteWidth(this TablePanel tablePanel, int columnIndex0, int width)
      {
         tablePanel.Columns[columnIndex0].Style = TablePanelEntityStyle.Absolute;
         tablePanel.Columns[columnIndex0].Width = width;
         return tablePanel;
      }

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

      public static void AdjustButtonSize(this TablePanel tablePanel, SimpleButton button, int? width=null, int? height = null)
      {
         var row = tablePanel.RowFor(button);
         var col = tablePanel.ColumnFor(button);
         if (width.HasValue)
         {
            col.Style = TablePanelEntityStyle.Absolute;
            col.Width = width.Value;
         }

         if (height.HasValue)
         {
            row.Style = TablePanelEntityStyle.Absolute;
            row.Height = height.Value;
         }
      }

      public static TablePanelRow RowFor(this TablePanel tablePanel, Control control)
      {
         return tablePanel.Rows[tablePanel.GetRow(control)];
      }
      public static TablePanelColumn ColumnFor(this TablePanel tablePanel, Control control)
      {
         return tablePanel.Columns[tablePanel.GetColumn(control)];
      }
   }
}