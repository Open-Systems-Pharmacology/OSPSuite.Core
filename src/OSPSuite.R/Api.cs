using System;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.R.Bootstrap;
using OSPSuite.R.Services;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;
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
      private static IContainer _container;

      public static void InitializeOnce(ApiConfig apiConfig)
      {
         _container = ApplicationStartup.Initialize(apiConfig);
      }

      public static IContainerTask GetContainerTask() => resolveTask<IContainerTask>();

      public static ISimulationPersister GetSimulationPersister() => resolveTask<ISimulationPersister>();

      public static ISimulationRunner GetSimulationRunner() => resolveTask<ISimulationRunner>();

      public static IPKAnalysisTask GetPKAnalysisTask() => resolveTask<IPKAnalysisTask>();

      public static IPopulationTask GetPopulationTask() => resolveTask<IPopulationTask>();

      public static ISimulationResultsTask GetSimulationResultsTask() => resolveTask<ISimulationResultsTask>();

      public static IOutputIntervalFactory GetOutputIntervalFactory() => resolveTask<IOutputIntervalFactory>();

      public static ISensitivityAnalysisRunner GetSensitivityAnalysisRunner() => resolveTask<ISensitivityAnalysisRunner>();

      public static ISensitivityAnalysisTask GetSensitivityAnalysisTask() => resolveTask<ISensitivityAnalysisTask>();

      public static IDimensionTask GetDimensionTask() => resolveTask<IDimensionTask>();

      public static IPKParameterTask GetPKParameterTask() => resolveTask<IPKParameterTask>();

      public static IFullPathDisplayResolver GetFullPathDisplayResolver() => resolveTask<IFullPathDisplayResolver>();

      public static IOSPLogger GetLogger() => resolveTask<IOSPLogger>();

      private static T resolveTask<T>()
      {
         try
         {
            return _container.Resolve<T>();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.FullMessage());
            throw;
         }
      }
   }
}