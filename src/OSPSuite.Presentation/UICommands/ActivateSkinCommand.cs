using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ActivateSkinCommand : IUICommand
   {
      private readonly IPresentationUserSettings _userSettings;
      private readonly ISkinManager _skinManager;
      public string SkinName { get; set; }

      public ActivateSkinCommand(IPresentationUserSettings userSettings, ISkinManager skinManager)
      {
         _userSettings = userSettings;
         _skinManager = skinManager;
      }

      public void Execute()
      {
         _skinManager.ActivateSkin(_userSettings, SkinName);
      }
   }
}