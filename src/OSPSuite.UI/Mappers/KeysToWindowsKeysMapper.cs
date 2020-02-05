using OSPSuite.Presentation.Core;
using OSPSuite.Utility;

namespace OSPSuite.UI.Mappers
{
   public interface IKeysToWindowsKeysMapper : IMapper<Keys, System.Windows.Forms.Keys>
   {
   }

   public class KeysToWindowsKeysMapper : IKeysToWindowsKeysMapper
   {
      public System.Windows.Forms.Keys MapFrom(Keys key)
      {
         return (System.Windows.Forms.Keys) (key);
      }
   }
}