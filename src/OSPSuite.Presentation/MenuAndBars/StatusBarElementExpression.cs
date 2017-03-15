namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IStatusBarElementExpression
   {
      StatusBarPanelExpressionConnection WithCaption(string caption);
      StatusBarPanelExpressionConnection WithValue(object value);
      StatusBarPanelExpressionConnection Enabled(bool enabled);
      StatusBarPanelExpressionConnection ToolTipText(string toolTipText);
      StatusBarPanelExpressionConnection Visible(bool visible);
   }

   public class StatusBarPanelExpressionConnection
   {
      public StatusBarPanelExpressionConnection(IStatusBarElementExpression statusBarPanelExpressionConnection)
      {
         And = statusBarPanelExpressionConnection;
      }

      public IStatusBarElementExpression And { get; }
   }

   public class NullStatusBarElementExpression : IStatusBarElementExpression
   {
      public StatusBarPanelExpressionConnection WithCaption(string caption)
      {
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection WithValue(object value)
      {
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection Enabled(bool enabled)
      {
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection ToolTipText(string toolTipText)
      {
         return new StatusBarPanelExpressionConnection(this);
      }

      public StatusBarPanelExpressionConnection Visible(bool visible)
      {
         return new StatusBarPanelExpressionConnection(this);
      }
   }
}