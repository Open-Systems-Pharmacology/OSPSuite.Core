﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Core.Importer.Services;
using OSPSuite.Utility.Events;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_SimulationResultsImportTask : ContextSpecification<ISimulationResultsImportTask>
   {
      protected IIndividualResultsImporter _individualResultsImporter;
      private IEntitiesInSimulationRetriever _containerTask;
      protected SimulationResultsImport _results;
      protected readonly List<IndividualResults> _individualResults1 = new List<IndividualResults>();
      protected readonly List<IndividualResults> _individualResults2 = new List<IndividualResults>();
      private const string _file1 = "File1";
      private const string _file2 = "File2";
      protected IModelCoreSimulation _populationSimulation;
      protected List<string> _files;
      protected CancellationToken _cancellationToken;
      private readonly PathCache<IQuantity> _allQuantities = new PathCache<IQuantity>(new EntityPathResolverForSpecs());
      private IProgressManager _progressManager;

      protected override void Context()
      {
         _individualResultsImporter = A.Fake<IIndividualResultsImporter>();
         _containerTask = A.Fake<IEntitiesInSimulationRetriever>();
         _progressManager = A.Fake<IProgressManager>();
         _populationSimulation = A.Fake<IModelCoreSimulation>();
         _files = new List<string> {_file1, _file2};
         _cancellationToken = new CancellationToken();
         sut = new SimulationResultsImportTask(_containerTask, _individualResultsImporter, _progressManager);
         A.CallTo(_containerTask).WithReturnType<PathCache<IQuantity>>().Returns(_allQuantities);
         A.CallTo(() => _individualResultsImporter.ImportFrom(_file1, _populationSimulation, A<IImportLogger>._)).Returns(_individualResults1);
         A.CallTo(() => _individualResultsImporter.ImportFrom(_file2, _populationSimulation, A<IImportLogger>._)).Returns(_individualResults2);
      }

      protected IndividualResults CreateIndividualResults(int individualId, int length = 5, float defaultValue = 10)
      {
         return new IndividualResults
         {
            IndividualId = individualId,
            Time = new QuantityValues {Values = new float[length].InitializeWith(defaultValue)}
         };
      }
   }

   public class When_importing_the_simulation_results_defined_in_a_valid_set_of_files : concern_for_SimulationResultsImportTask
   {
      protected override void Context()
      {
         base.Context();
         _individualResults1.Add(CreateIndividualResults(1));
         _individualResults1.Add(CreateIndividualResults(2));
         _individualResults2.Add(CreateIndividualResults(3));
         _individualResults2.Add(CreateIndividualResults(4));
      }

      [Observation]
      public async Task should_return_the_simulation_results_containing_all_results_defined_in_the_files()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.SimulationResults.Count().ShouldBeEqualTo(4);
      }

      [Observation]
      public async Task should_return_a_valid_status()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.HasError.ShouldBeFalse();
      }
   }

   public class When_importing_the_simulation_results_defined_in_a_set_of_file_containing_duplicate_ids : concern_for_SimulationResultsImportTask
   {
      protected override void Context()
      {
         base.Context();
         _individualResults1.Add(CreateIndividualResults(1));
         _individualResults1.Add(CreateIndividualResults(2));
         _individualResults2.Add(CreateIndividualResults(2));
         _individualResults2.Add(CreateIndividualResults(4));
      }

      [Observation]
      public async Task should_return_an_invalid_status()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.HasError.ShouldBeTrue();
      }
   }

   public class When_importing_results_defined_in_a_set_of_files_with_different_time_length : concern_for_SimulationResultsImportTask
   {
      protected override void Context()
      {
         base.Context();
         _individualResults1.Add(CreateIndividualResults(1, length: 2));
         _individualResults1.Add(CreateIndividualResults(2, length: 3));
      }

      [Observation]
      public async Task should_return_an_invalid_status()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.HasError.ShouldBeTrue();
      }
   }

   public class When_importing_results_defined_in_a_set_of_files_with_different_time_vectors : concern_for_SimulationResultsImportTask
   {
      protected override void Context()
      {
         base.Context();
         _individualResults1.Add(CreateIndividualResults(1, defaultValue: 2));
         _individualResults1.Add(CreateIndividualResults(2, defaultValue: 3));
      }

      [Observation]
      public async Task should_return_an_invalid_status()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.HasError.ShouldBeTrue();
      }
   }

   public class When_importing_results_defined_in_a_set_of_files_with_different_quantity_paths : concern_for_SimulationResultsImportTask
   {
      protected override void Context()
      {
         base.Context();
         var individualResults1 = CreateIndividualResults(1);
         individualResults1.Add(new QuantityValues {QuantityPath = "Path1"});
         individualResults1.Add(new QuantityValues {QuantityPath = "Path2"});
         var individualResults2 = CreateIndividualResults(2);
         individualResults2.Add(new QuantityValues {QuantityPath = "Path2"});
         individualResults2.Add(new QuantityValues {QuantityPath = "Path3"});

         _individualResults1.Add(individualResults1);
         _individualResults1.Add(individualResults2);
      }

      [Observation]
      public async Task should_return_an_invalid_status()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.HasError.ShouldBeTrue();
      }
   }

   public class When_importing_results_defined_in_a_set_of_files_with_different_quantity_paths_length : concern_for_SimulationResultsImportTask
   {
      protected override void Context()
      {
         base.Context();
         var individualResults1 = CreateIndividualResults(1);
         individualResults1.Add(new QuantityValues {QuantityPath = "Path1"});
         var individualResults2 = CreateIndividualResults(2);
         individualResults2.Add(new QuantityValues {QuantityPath = "Path1"});
         individualResults2.Add(new QuantityValues {QuantityPath = "Path2"});

         _individualResults1.Add(individualResults1);
         _individualResults1.Add(individualResults2);
      }

      [Observation]
      public async Task should_return_an_invalid_status()
      {
         _results = await sut.ImportResults(_populationSimulation, _files, _cancellationToken);
         _results.HasError.ShouldBeTrue();
      }
   }
}