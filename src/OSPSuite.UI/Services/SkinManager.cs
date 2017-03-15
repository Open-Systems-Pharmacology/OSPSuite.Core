using System.Collections.Generic;
using System.Linq;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using OSPSuite.Presentation;
using OSPSuite.Presentation.Services;

namespace OSPSuite.UI.Services
{
   public class SkinManager : ISkinManager
   {
      private readonly UserLookAndFeel _lookAndFeel;
      private bool _initialized;

      public SkinManager(UserLookAndFeel lookAndFeel)
      {
         _lookAndFeel = lookAndFeel;
      }

      public void Start()
      {
         if (_initialized) return;
         BonusSkins.Register();
         DevExpress.Skins.SkinManager.EnableFormSkins();
         DevExpress.Skins.SkinManager.EnableMdiFormSkins();
         LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
         _initialized = true;
      }

      public void ActivateSkin(IPresentationUserSettings userSettings, string skinName)
      {
         Start();
         userSettings.ActiveSkin = skinName;
         _lookAndFeel.SetSkinStyle(skinName);
      }

      public IEnumerable<string> All()
      {
         Start();
         return DevExpress.Skins.SkinManager.Default.Skins.Cast<SkinContainer>()
            .Where(isFeatured)
            .Select(skin => skin.SkinName)
            .OrderBy(x => x);
      }

      private bool isFeatured(SkinContainer skin)
      {
         return !UI.UIConstants.Skins.ForbiddenSkins.Contains(skin.SkinName);
      }
   }
}