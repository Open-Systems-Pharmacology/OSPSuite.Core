using System;
using System.Collections.Concurrent;
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

   internal class AccessCounter
   {
      private int _counter = 0;
      public int Max { get; private set; }
      public void Enter()
      {
         Max = Math.Max(Max, ++_counter);
      }
      public void Leave()
      {
         _counter--;
      }
   }

   public class When_running_some_tasks_using_the_concurrency_manager_on_multiple_cores_but_the_tasks_are_not_running_on_different_thread : concern_for_ConcurrencyManager
   {
      private int[] _data;
      private ConcurrentDictionary<int, ConcurrencyManagerResult<int>> _results = new ConcurrentDictionary<int, ConcurrencyManagerResult<int>>();

      protected override async Task Context()
      {
         await base.Context();
         _data = new[] {10, 20, 30, 40, 50};
      }

      protected override async Task Because()
      {
         await sut.RunAsync(1, _data, x => x.ToString(), actionToRun, CancellationToken.None, _results);
      }

      private int actionToRun(int coreIndex, int data, CancellationToken token)
      {
         return coreIndex + data;
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
      private ConcurrentDictionary<int, ConcurrencyManagerResult<int>> _results = new ConcurrentDictionary<int, ConcurrencyManagerResult<int>>();

      protected override async Task Context()
      {
         await base.Context();
         _data = new[] { 10, 20, 30, 40, 50 };
      }

      protected override async Task Because()
      {
         await sut.RunAsync(3, _data, x => x.ToString(), actionToRun, CancellationToken.None, _results);
      }

      private int actionToRun(int coreIndex, int data, CancellationToken token)
      {
         return coreIndex + data;
      }

      [Observation]
      public void should_spread_the_data_to_run_as_expected()
      {
         // we expect some data to not be equal to their id as we add the core index
         var outputs = _results.Values.Select(x => x.Result).OrderBy(x => x).ToArray();
         outputs.ShouldNotBeEqualTo(_data);
      }
   }

   public class When_running_more_tasks_than_cores : concern_for_ConcurrencyManager
   {
      private int[] _data;
      private ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>> _results = new ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>>();
      private AccessCounter _accessCounter;

      protected override async Task Context()
      {
         await base.Context();
         _data = Enumerable.Range(0, 3 * Environment.ProcessorCount).ToArray();
         _accessCounter = new AccessCounter();
      }

      protected override async Task Because()
      {
         await sut.RunAsync(_data.Length, _data.Select(x => _accessCounter).ToList(), x => Guid.NewGuid().ToString(), actionToRun, CancellationToken.None, _results);
      }

      private int actionToRun(int coreIndex, AccessCounter data, CancellationToken token)
      {
         data.Enter();
         Thread.Sleep(100);
         data.Leave();
         return coreIndex;
      }

      [Observation]
      public void should_not_exceed_cores()
      {
         _accessCounter.Max.ShouldBeSmallerThan(Environment.ProcessorCount);
      }
   }

   public class When_running_several_times_less_tasks_than_cores : concern_for_ConcurrencyManager
   {
      private int[] _data;
      private AccessCounter _accessCounter;
      private ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>> _results1 = new ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>>();
      private ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>> _results2 = new ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>>();
      private ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>> _results3 = new ConcurrentDictionary<AccessCounter, ConcurrencyManagerResult<int>>();

      protected override async Task Context()
      {
         await base.Context();
         _data = Enumerable.Range(0, Environment.ProcessorCount / 2).ToArray();
         _accessCounter = new AccessCounter();
      }

      protected override Task Because()
      {
         var tasks = new List<Task>();
         tasks.Add(sut.RunAsync(_data.Length, _data.Select(x => _accessCounter).ToArray(), x => x.ToString(), actionToRun, CancellationToken.None, _results1));
         tasks.Add(sut.RunAsync(_data.Length, _data.Select(x => _accessCounter).ToArray(), x => x.ToString(), actionToRun, CancellationToken.None, _results2));
         tasks.Add(sut.RunAsync(_data.Length, _data.Select(x => _accessCounter).ToArray(), x => x.ToString(), actionToRun, CancellationToken.None, _results3));
         Task.WaitAll(tasks.ToArray());
         return Task.CompletedTask;
      }

      private int actionToRun(int coreIndex, AccessCounter data, CancellationToken token)
      {
         data.Enter();
         Thread.Sleep(100);
         data.Leave();
         return coreIndex;
      }

      [Observation]
      public void should_use_free_cores_on_the_second_run()
      {
         _accessCounter.Max.ShouldBeGreaterThan(_data.Length);
      }

      [Observation]
      public void but_should_not_exceed_cores()
      {
         _accessCounter.Max.ShouldBeSmallerThan(Environment.ProcessorCount);
      }
   }
}