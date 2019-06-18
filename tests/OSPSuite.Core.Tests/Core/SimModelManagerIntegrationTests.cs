using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using SimModelNET;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimModelManager : ContextForIntegration<ISimModelManager>
   {
      private IWithIdRepository _withIdRepository;
      protected IModelCoreSimulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _withIdRepository = IoC.Resolve<IWithIdRepository>();
         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
         new RegisterTaskForSpecs(_withIdRepository).RegisterAllIn(_simulation.Model.Root);
         var schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.SimModel.xsd");
         XMLSchemaCache.InitializeFromFile(schemaPath);
         sut = IoC.Resolve<ISimModelManager>();
      }
   }

   public class When_run_simulation_is_called : concern_for_SimModelManager
   {
      private SimulationRunResults _res;

      protected override void Because()
      {
         _res = sut.RunSimulation(_simulation);
      }

      [Observation]
      public void should_return_success()
      {
         _res.Success.ShouldBeTrue();
      }

      [Observation]
      public void should_return_a_empty_warning_list()
      {
         _res.Warnings.ShouldNotBeNull();
         _res.Warnings.Count().ShouldBeEqualTo(0);
      }
   }

   public class When_retrievng_the_results_of_a_simulation : concern_for_SimModelManager
   {
      private DataRepository _results;

      protected override void Because()
      {
         _results = sut.RunSimulation(_simulation).Results;
      }

      [Observation]
      public void should_return_repository_with_number_of_amounts_and_observers_and_persistent_parameter_plus_one_extra_column_for_time()
      {
         int numberOfAmounts = _simulation.Model.Root.GetAllChildren<IMoleculeAmount>().Count();
         int numberOfObservers = _simulation.Model.Root.GetAllChildren<IObserver>().Count();
         int numberOfPersistentParameter = _simulation.Model.Root.GetAllChildren<IParameter>(parameter => parameter.Persistable).Count();
         _results.Count().ShouldBeEqualTo(numberOfAmounts + numberOfObservers + numberOfPersistentParameter + 1);
      }

      [Observation]
      public void should_have_set_the_molweight_in_amounts_and_observers_when_available()
      {
         foreach (var dataColumn in _results.AllButBaseGrid())
         {
            if (molWeightShouldBeSetFor(dataColumn))
               dataColumn.DataInfo.MolWeight.ShouldBeEqualTo(250);
            else
               dataColumn.DataInfo.MolWeight.ShouldBeNull();
         }
      }

      [Observation]
      public void should_have_set_comparison_threshold_for_observers_and_variables()
      {
         foreach (var dataColumn in _results.AllButBaseGrid())
         {
            dataColumn.DataInfo.ComparisonThreshold.ShouldNotBeNull();
         }
      }

      private bool molWeightShouldBeSetFor(DataColumn dataColumn)
      {
         if (dataColumn.QuantityInfo.Name == "A")
            return true;

         if (!dataColumn.QuantityInfo.Type.Is(QuantityType.Observer))
            return false;

         var path = dataColumn.QuantityInfo.Path.ToList();
         //-2 because last is name of observer
         return path[path.Count - 2] == "A";
      }
   }

   public class When_canceling_a_simulation_run : concern_for_SimModelManager
   {
      private SimulationRunResults _runResults;

      protected override void Context()
      {
         base.Context();
         var interval = _simulation.BuildConfiguration.SimulationSettings.OutputSchema.Intervals.ElementAt(0);
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.END_TIME).Value = 5000000;
         interval.GetSingleChildByName<IParameter>(Constants.Parameters.RESOLUTION).Value = 10000;
      }

      protected override void Because()
      {
         var task = Task.Run(() => sut.RunSimulation(_simulation));
         //needs to sleep so that the action actually starts
         Thread.Sleep(100);
         sut.StopSimulation();
         _runResults = task.Result;
      }

      [Observation]
      public void should_not_crash()
      {
         _runResults.Success.ShouldBeFalse();
      }
   }
}