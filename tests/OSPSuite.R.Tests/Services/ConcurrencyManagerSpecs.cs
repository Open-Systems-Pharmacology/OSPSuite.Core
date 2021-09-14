using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_ConcurrencyManager : ContextSpecificationAsync<IConcurrencyManager>
   {
      protected override Task Context()
      {
         sut = new ConcurrencyManager();
         return _completed;
      }
   }

   public class When_running_some_tasks_using_the_concurrency_manager_on_multiple_cores_but_the_tasks_are_not_running_on_different_thread : concern_for_ConcurrencyManager
   {
      private int[] _data;
      private IReadOnlyDictionary<int, ConcurrencyManagerResult<int>> _results;

      protected override async Task Context()
      {
         await base.Context();
         _data = new[] {10, 20, 30, 40, 50};
      }

      protected override async Task Because()
      {
         _results = await sut.RunAsync(3, _data, x => x.ToString(), actionToRun, CancellationToken.None);
      }

      private Task<int> actionToRun(int coreIndex, int data, CancellationToken token)
      {
         return Task.FromResult(coreIndex + data);
      }

      [Observation]
      public void should_spread_the_data_to_run_as_expected()
      {
         // we expect the same data because only one core is used. (index 0)
         var outputs = _results.Values.Select(x => x.Result).OrderBy(x => x).ToArray();
         outputs.ShouldBeEqualTo(_data);
      }
   }

   public class When_running_some_tasks_using_the_concurrency_manager_on_multiple_cores_and_tasks_are_not_running_on_different_thread : concern_for_ConcurrencyManager
   {
      private int[] _data;
      private IReadOnlyDictionary<int, ConcurrencyManagerResult<int>> _results;

      protected override async Task Context()
      {
         await base.Context();
         _data = new[] { 10, 20, 30, 40, 50 };
      }

      protected override async Task Because()
      {
         _results = await sut.RunAsync(3, _data, x => x.ToString(), actionToRun, CancellationToken.None);
      }

      private Task<int> actionToRun(int coreIndex, int data, CancellationToken token)
      {
         return Task.Run(() => coreIndex + data, token);
      }

      [Observation]
      public void should_spread_the_data_to_run_as_expected()
      {
         // we expect some data to not be equal to their id as we add the core index
         var outputs = _results.Values.Select(x => x.Result).OrderBy(x => x).ToArray();
         outputs.ShouldNotBeEqualTo(_data);
      }
   }
}