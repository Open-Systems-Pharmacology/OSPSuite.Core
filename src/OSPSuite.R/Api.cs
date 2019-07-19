using OSPSuite.R.Bootstrap;

namespace OSPSuite.R
{
   public class ApiConfig
   {
      public string DimensionFilePath { get; set; }
      public string PKParametersFilePath { get; set; }
   }

   public static class Api
   {
      public static void InitializeOnce(ApiConfig apiConfig)
      {
         ApplicationStartup.Initialize(apiConfig);
      }
   }
}