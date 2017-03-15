using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Services
{
   public interface IIdGenerator
   {
      string NewId();
   }

   public class IdGenerator : IIdGenerator
   {
      public string NewId()
      {
         return ShortGuid.NewGuid().ToString();
      }
   }
}