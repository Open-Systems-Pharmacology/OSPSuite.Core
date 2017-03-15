using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.MenuAndBars
{
   public interface IMenuBarButton : IMenuBarItem
   {
      /// <summary>
      ///    Command that will be executed
      /// </summary>
      IUICommand Command { get; set; }

      /// <summary>
      ///    underlying button was clicked
      /// </summary>
      void Click();
   }

   public class MenuBarButton : MenuBarItem, IMenuBarButton
   {
      public IUICommand Command { get; set; }

      public MenuBarButton()
      {
         Command = new NotImplementedCommand();
      }

      public virtual void Click()
      {
         Command.ExecuteWithinExceptionHandler();
      }
   }
}