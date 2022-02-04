using System;
using System.Collections.Generic;
using OSPSuite.Core.Extensions;
using OSPSuite.R.Domain;
using OSPSuite.R.Services;

namespace OSPSuite.R.Performance
{
   internal class Program
   {
      static void Main(string[] args)
      {
         /*if (args.Length != 2)
         {
            Console.WriteLine("Not enough parameters: pkml file, number of load");
            return;
         }*/

         ApplicationStartup.Initialize();

         // var pkmlPath = args[0];
         // var numberOfSimulationToRun = int.Parse(args[1]);

         var pkmlPath = @"C:\tests\v11\1455\testRunSimulations\BigSim2.pkml";
         var numberOfSimulationToRun = 12;

         for (int i = 0; i < 1; i++)
         {
            performTask(pkmlPath, numberOfSimulationToRun);
            Console.WriteLine();
         }
         Console.ReadKey();
      }

      private static void performTask(string pkmlPath, int numberOfSimulationToRun)
      {
         var simulationTask = Api.GetSimulationPersister();
         var concurrentSimulationRunner = Api.GetConcurrentSimulationRunner();
         var begin = DateTime.UtcNow;
         var allSimulations = new List<Simulation>();
         for (int i = 0; i < numberOfSimulationToRun; i++)
         {
            var sim = simulationTask.LoadSimulation(pkmlPath, resetIds: true);
            allSimulations.Add(sim);
         }

         var end = DateTime.UtcNow;
         var timeSpent = end - begin;

         Console.WriteLine($"Loading {numberOfSimulationToRun} simulations in {timeSpent.ToDisplay()}");

         var simulationRunOptions = new SimulationRunOptions
         {
            NumberOfCoresToUse = Math.Max(Environment.ProcessorCount - 1, 1)
         };
         begin = DateTime.UtcNow;
         concurrentSimulationRunner.SimulationRunOptions = simulationRunOptions;
         foreach (var simulation in allSimulations)
         {
            concurrentSimulationRunner.AddSimulation(simulation);
         }

         var res = concurrentSimulationRunner.RunConcurrently();
         end = DateTime.UtcNow;
         timeSpent = end - begin;
         Console.WriteLine($"Running {numberOfSimulationToRun} simulations in {timeSpent.ToDisplay()}");
         allSimulations.Clear();
         doGC(concurrentSimulationRunner);
      }

      private static void doGC(IConcurrentSimulationRunner concurrentSimulationRunner)
      {
         concurrentSimulationRunner.Dispose();
         concurrentSimulationRunner.GCCollectAndCompact();
         for (int i = 0; i < 4; i++)
         {
            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
         }
      }
   }
}