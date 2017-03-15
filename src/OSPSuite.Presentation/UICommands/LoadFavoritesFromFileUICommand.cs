using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class LoadFavoritesFromFileUICommand : IUICommand
   {
      private readonly IFavoriteTask _favoriteTask;

      public LoadFavoritesFromFileUICommand(IFavoriteTask favoriteTask)
      {
         _favoriteTask = favoriteTask;
      }

      public void Execute()
      {
         _favoriteTask.LoadFromFile();
      }
   }
}