using OSPSuite.Core.Services;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.UICommands
{
   public class SaveFavoritesToFileUICommand : IUICommand
   {
      private readonly IFavoriteTask _favoriteTask;

      public SaveFavoritesToFileUICommand(IFavoriteTask favoriteTask)
      {
         _favoriteTask = favoriteTask;
      }

      public void Execute()
      {
         _favoriteTask.SaveToFile();
      }
   }
}