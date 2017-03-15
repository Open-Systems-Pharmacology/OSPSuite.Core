using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Data
{
   public class NullDataRepository : DataRepository
   {
      public NullDataRepository()
      {
         Name = Captions.EmptyName;
         Description = Captions.EmptyDescription;
      }
   }
}