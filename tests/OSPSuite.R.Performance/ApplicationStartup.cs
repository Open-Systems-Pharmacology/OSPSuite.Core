using System;
using System.IO;

namespace OSPSuite.R.Performance
{
   public static class ApplicationStartup
   {
      public static void Initialize()
      {
         var apiConfig = new ApiConfig
         {
            DimensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.Dimensions.xml"),
            PKParametersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.PKParameters.xml"),
         };
         Api.InitializeOnce(apiConfig);
      }
   }
}