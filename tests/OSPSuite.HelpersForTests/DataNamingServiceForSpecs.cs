using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;

namespace OSPSuite.Helpers
{
   public class DataNamingServiceForSpecs : IDataNamingService
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