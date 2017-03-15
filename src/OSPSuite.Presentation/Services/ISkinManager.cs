using System.Collections.Generic;

namespace OSPSuite.Presentation.Services
{
   public interface ISkinManager
   {
      void ActivateSkin(IPresentationUserSettings userSettings, string skinName);
      IEnumerable<string> All();
   }
}