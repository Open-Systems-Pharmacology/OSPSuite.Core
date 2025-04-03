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

namespace OSPSuite.Core
{
   public abstract class concern_for_SimModelManagerAsync : ContextForIntegrationAsync<ISimModelManager>
   {
      protected IWithIdRepository _withIdRepository;
      protected IModelCoreSimulation _simulation;
      protected CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

      public override async Task GlobalContext()
      {
         await base.GlobalContext();

         _withIdRepository = IoC.Resolve<IWithIdRepository>();
         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
         new RegisterTaskForSpecs(_withIdRepository).RegisterAllIn(_simulation.Model.Root);

         sut = IoC.Resolve<ISimModelManager>();
      }
   }

   public class When_run_simulation_is_called : concern_for_SimModelManagerAsync
   {
      private SimulationRunResults _res;

      protected override async Task Because()
      {
         _res = await Task.Run(() => sut.RunSimulation(_simulation));
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

   public class When_retrieving_the_results_of_a_simulation : concern_for_SimModelManagerAsync
   {
      private DataRepository _results;

      protected override async Task Because()
      {
         _results = (await Task.Run(() => sut.RunSimulation(_simulation))).Results;
      }

      [Observation]
      public void should_return_repository_with_number_of_amounts_and_observers_and_persistent_parameter_plus_one_extra_column_for_time()
      {
         int numberOfAmounts = _simulation.Model.Root.GetAllChildren<MoleculeAmount>().Count;
         int numberOfObservers = _simulation.Model.Root.GetAllChildren<Observer>().Count;
         int numberOfPersistentParameter = _simulation.Model.Root.GetAllChildren<IParameter>(parameter => parameter.Persistable).Count ;
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
         if (dataColumn.Name == "A")
            return true;

         if (!dataColumn.QuantityInfo.Type.Is(QuantityType.Observer))
            return false;

         var path = dataColumn.QuantityInfo.Path.ToList();
         //-2 because last is name of observer
         return path[path.Count - 2] == "A";
      }
   }

   public class When_running_two_simulations_concurrently : concern_for_SimModelManagerAsync
   {
      protected IModelCoreSimulation _simulation2;
      private SimulationRunResults _runResults1;
      private SimulationRunResults _runResults2;

      protected override async Task Context()
      {
         await base.Context();
         _simulation2 = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
      }

      protected override async Task Because()
      {
         _runResults1 = await sut.RunSimulationAsync(_simulation, _cancellationTokenSource.Token);
         _runResults2 = await sut.RunSimulationAsync(_simulation2, _cancellationTokenSource.Token);
      }

      [Observation]
      public void should_be_successful()
      {
         _runResults1.Success.ShouldBeTrue();
         _runResults2.Success.ShouldBeTrue();
      }
   }
}