using System;
using OSPSuite.R.Bootstrap;
using OSPSuite.R.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

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

      public static IContainerTask GetContainerTask() => resolveTask<IContainerTask>();

      public static ISimulationLoader GetSimulationLoader() => resolveTask<ISimulationLoader>();

      public static ISimulationRunner GetSimulationRunner() => resolveTask<ISimulationRunner>();

      private static T resolveTask<T>()
      {
         try
         {
            return IoC.Resolve<T>();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.FullMessage());
            throw;
         }
      }
   }
}