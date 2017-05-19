using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace OSPSuite.Starter.Services
{
   public class DataNamingService : IDataNamingService
   {
      public string GetTimeName()
      {
         return "Time";
      }

      public string GetNewRepositoryName()
      {
         return ShortGuid.NewGuid();
      }

      public string GetEntityName(string id)
      {
         return id;
      }
   }
}