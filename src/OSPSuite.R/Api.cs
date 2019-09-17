using System;
using OSPSuite.Core.Domain.Services;
using OSPSuite.R.Bootstrap;
using OSPSuite.R.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainerTask = OSPSuite.R.Services.IContainerTask;

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

      public static ISimulationPersister GetSimulationPersister() => resolveTask<ISimulationPersister>();

      public static ISimulationRunner GetSimulationRunner() => resolveTask<ISimulationRunner>();

      public static IPKAnalysesTask GetPKAnalysesTask() => resolveTask<IPKAnalysesTask>();

      public static ISimulationExporter GetSimulationExporter() => resolveTask<ISimulationExporter>();

      public static IPopulationImporter GetPopulationImporter() => resolveTask<IPopulationImporter>();

      public static ISimulationResultsTask GetSimulationResultsTask() => resolveTask<ISimulationResultsTask>();

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