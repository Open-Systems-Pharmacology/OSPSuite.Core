using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.UI.Extensions
{
   public class StatusBarElementExpression : IStatusBarElementExpression
   {
      private readonly BarItem _barItem;

      public StatusBarElementExpression(BarItemLink barItem)
      {
         _barItem = barItem.Item;
      }

      public StatusBarPanelExpressionConnection WithCaption(string caption)
      {
         _barItem.Caption = caption;
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection WithValue(object value)
      {
         var barEditItem = _barItem as BarEditItem;
         if (barEditItem != null)
         {
            barEditItem.EditValue = value;
         }
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection Enabled(bool enabled)
      {
         _barItem.Enabled = enabled;
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection ToolTipText(string toolTipText)
      {
         _barItem.Hint = toolTipText;
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection Visible(bool visible)
      {
         _barItem.Visibility = visible ? BarItemVisibility.Always : BarItemVisibility.Never;
         var barEditItem = _barItem as BarEditItem;
         if (barEditItem != null)
         {
            var repositoryItemProgressBar = barEditItem.Edit as RepositoryItemProgressBar;
            if (repositoryItemProgressBar != null)
               repositoryItemProgressBar.ShowTitle = visible;
         }

         return new StatusBarPanelExpressionConnection(this);
      }
   }
}