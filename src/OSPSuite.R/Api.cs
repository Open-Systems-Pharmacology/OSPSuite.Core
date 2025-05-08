using System;
using DevExpress.Utils.IoC;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.R.Bootstrap;
using OSPSuite.R.Domain;
using OSPSuite.R.Services;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;
using IContainerTask = OSPSuite.R.Services.IContainerTask;
using IDataRepositoryTask = OSPSuite.R.Services.IDataRepositoryTask;

namespace OSPSuite.R
{
   public class ApiConfig
   {
      public string DimensionFilePath { get; set; }
      public string PKParametersFilePath { get; set; }
   }

   public static class Api
   {
      public static IContainer Container { get; private set; }

      public static void InitializeOnce(ApiConfig apiConfig, Action<IContainer> registerAction = null)
      {
         Container = ApplicationStartup.Initialize(apiConfig, registerAction);
      }

      public static IContainerTask GetContainerTask() => resolveTask<IContainerTask>();

      public static ISimulationPersister GetSimulationPersister() => resolveTask<ISimulationPersister>();

      public static ISimulationRunner GetSimulationRunner() => resolveTask<ISimulationRunner>();

      public static IPKAnalysisTask GetPKAnalysisTask() => resolveTask<IPKAnalysisTask>();

      public static IPopulationTask GetPopulationTask() => resolveTask<IPopulationTask>();

      public static ISimulationResultsTask GetSimulationResultsTask() => resolveTask<ISimulationResultsTask>();

      public static IOutputIntervalFactory GetOutputIntervalFactory() => resolveTask<IOutputIntervalFactory>();

      public static ISimulationBatchFactory GetSimulationBatchFactory() => resolveTask<ISimulationBatchFactory>();

      public static ISensitivityAnalysisRunner GetSensitivityAnalysisRunner() => resolveTask<ISensitivityAnalysisRunner>();

      public static ISensitivityAnalysisTask GetSensitivityAnalysisTask() => resolveTask<ISensitivityAnalysisTask>();

      public static IDimensionTask GetDimensionTask() => resolveTask<IDimensionTask>();

      public static IDimensionFactory GetDimensionFactory() => resolveTask<IDimensionFactory>();

      public static IPKParameterTask GetPKParameterTask() => resolveTask<IPKParameterTask>();

      public static IFullPathDisplayResolver GetFullPathDisplayResolver() => resolveTask<IFullPathDisplayResolver>();

      public static IDataRepositoryTask GetDataRepositoryTask() => resolveTask<IDataRepositoryTask>();

      public static IOSPSuiteLogger GetLogger() => resolveTask<IOSPSuiteLogger>();

      public static IConcurrentSimulationRunner GetConcurrentSimulationRunner() => resolveTask<IConcurrentSimulationRunner>();

      public static IDataImporterTask GetDataImporterTask() => resolveTask<IDataImporterTask>();

      public static ISimulationTask GetSimulationTask() => resolveTask<ISimulationTask>();

      /// <summary>
      /// Forces the Garbage collection 
      /// </summary>
      public static void ForceGC()
      {
         GarbageCollectionTask.ForceGC();
      }

      private static T resolveTask<T>()
      {
         try
         {
            return Container.Resolve<T>();
         }
         catch (Exception e)
         {
            Console.WriteLine(e.FullMessage());
            throw;
         }
      }
   }
}