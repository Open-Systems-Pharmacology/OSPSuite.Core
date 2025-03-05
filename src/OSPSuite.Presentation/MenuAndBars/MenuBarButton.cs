using OSPSuite.Presentation.UICommands;
using System.Threading.Tasks;

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

      /// <summary>
      ///    Command that will be executed
      /// </summary>
      IUICommandAsync AsyncCommand { get; set; }


      /// <summary>
      ///    underlying button was clicked
      /// </summary>
      Task ClickAsync();
   }

   public class MenuBarButton : MenuBarItem, IMenuBarButton
   {
      public IUICommand Command { get; set; }
      public IUICommandAsync AsyncCommand { get; set; }

      public MenuBarButton()
      {
         Command = new NotImplementedCommand();
         AsyncCommand = new NotImplementedCommandAsync();
      }

      public virtual void Click()
      {
         Command.ExecuteWithinExceptionHandler();
      }

      public virtual async Task ClickAsync()
      {
         await AsyncCommand.ExecuteAsync();
      }
   }
}