using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.Views
{
   public interface IStatusBarView
   {
      void AddItem(StatusBarElement elementToAdd);
      IStatusBarElementExpression BarElementExpressionFor(StatusBarElement statusBarElement);
   }
}