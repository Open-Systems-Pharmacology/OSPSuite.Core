using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.UICommands
{
   public class ShowHelpCommand : IUICommand
   {
      private readonly IMainView _mainView;

      public ShowHelpCommand(IMainView mainView)
      {
         _mainView = mainView;
      }

      public void Execute()
      {
         _mainView.ShowHelp();
      }
   }
}