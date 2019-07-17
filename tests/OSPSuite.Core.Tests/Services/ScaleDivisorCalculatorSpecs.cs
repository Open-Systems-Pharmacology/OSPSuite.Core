using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ScaleDivisorCalculator : ContextSpecification<IScaleDivisorCalculator>
   {
      protected IModelCoreSimulation _simulation;
      private IMoleculeAmount _moleculeAmount1;
      private IMoleculeAmount _moleculeAmount2;
      protected DataRepository _originalResults;
      protected DataColumn _originalDataColumn;
      protected string _molecule1Path;
      protected string _molecule2Path;
      private ISimModelManager _simModelManager;
      private IContainerTask _containerTask;
      protected ScaleDivisorOptions _options;
      protected PathCache<IMoleculeAmount> _moleculeAmountCache;
      private IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         var entityPathFactory = new EntityPathResolverForSpecs();
         _objectPathFactory = new ObjectPathFactory(new AliasCreator());
         _simulation = A.Fake<IModelCoreSimulation>().WithName("Sim");
         _simModelManager = A.Fake<ISimModelManager>();
         _containerTask = A.Fake<IContainerTask>();
         _options = new ScaleDivisorOptions();
         _moleculeAmountCache = new PathCache<IMoleculeAmount>(entityPathFactory);
         var rootContainer = new ARootContainer().WithName(_simulation.Name)
            .WithContainerType(ContainerType.Simulation);

         _simulation.Model.Root = rootContainer;
         _moleculeAmount1 = new MoleculeAmount().WithName("M1");
         _moleculeAmount2 = new MoleculeAmount().WithName("M2");

         rootContainer.Add(_moleculeAmount1);
         rootContainer.Add(_moleculeAmount2);

         _moleculeAmountCache.Add(_moleculeAmount1);
         _moleculeAmountCache.Add(_moleculeAmount2);

         _molecule1Path = entityPathFactory.PathFor(_moleculeAmount1);
         _molecule2Path = entityPathFactory.PathFor(_moleculeAmount2);

         _originalResults = new DataRepository();
         _simulation.Results = _originalResults;

         var baseGrid = new BaseGrid("Time", Constants.Dimension.NO_DIMENSION) {Values = new[] {0f, 1f, 2f, 3f}};
         _originalDataColumn = new DataColumn("M1", Constants.Dimension.NO_DIMENSION, baseGrid) {Values = new[] {0f, 10f, 20f, 30f}};
         _originalDataColumn.QuantityInfo.Path = _objectPathFactory.CreateAbsoluteObjectPath(_moleculeAmount1);
         _originalResults.Add(_originalDataColumn);

         A.CallTo(_containerTask).WithReturnType<PathCache<IMoleculeAmount>>().Returns(_moleculeAmountCache);
         var simResults = new DataRepository();
         var baseGrid2 = new BaseGrid("Time", Constants.Dimension.NO_DIMENSION) {Values = new[] {0f, 1f, 2f, 3f}};
         var res1 = new DataColumn("M1", Constants.Dimension.NO_DIMENSION, baseGrid2) {Values = new[] {0f, 10f, 20f, 30f}};
         res1.QuantityInfo.Path = _objectPathFactory.CreateAbsoluteObjectPath(_moleculeAmount1);
         simResults.Add(res1);

         var res2 = new DataColumn("M2", Constants.Dimension.NO_DIMENSION, baseGrid2) {Values = new[] {0f, 11f, 12f, 13f}};
         res2.QuantityInfo.Path = _objectPathFactory.CreateAbsoluteObjectPath(_moleculeAmount2);
         simResults.Add(res2);

         var simulationRunResults = new SimulationRunResults(true, Enumerable.Empty<SolverWarning>(), simResults);
         A.CallTo(() => _simModelManager.RunSimulation(_simulation)).Returns(simulationRunResults);
         sut = new ScaleDivisorCalculator(_simModelManager, _containerTask, _objectPathFactory);
      }
   }

   public class When_calculating_the_scale_divisor_for_a_given_simulation : concern_for_ScaleDivisorCalculator
   {
      [Observation]
      public async Task should_return_a_scale_factor_for_each_molecule_amount_defined_in_the_model()
      {
         _options.UseRoundedValue = false;
         var results = await sut.CalculateScaleDivisorsAsync(_simulation, _options);
         assertScaleFactor(results, _molecule1Path, factorFor(new[] {10f, 20f, 30f}));
         assertScaleFactor(results, _molecule2Path, factorFor(new[] {11f, 12f, 13f}));
      }

      private double factorFor(float[] values)
      {
         var mean = values.ConvertToLog10Array().ArithmeticMean();
         return Math.Pow(10, mean);
      }

      private void assertScaleFactor(IEnumerable<ScaleDivisor> scaleFactors, string moleculePath, double value)
      {
         var scaleFactor = scaleFactors.Find(x => x.QuantityPath == moleculePath);
         scaleFactor.ShouldNotBeNull();
         scaleFactor.Value.ShouldBeEqualTo(value);
      }

      [Observation]
      public async Task should_not_override_the_previous_results_and_selected_output()
      {
         var results = await sut.CalculateScaleDivisorsAsync(_simulation, _options);
         _simulation.Results.ShouldBeEqualTo(_originalResults);
         _simulation.Results.AllButBaseGrid().ShouldOnlyContain(_originalDataColumn);
      }
   }

   public class When_calculating_the_scale_divisor_for_a_given_simulation_and_a_list_of_molecule_amounts : concern_for_ScaleDivisorCalculator
   {
      [Observation]
      public async Task should_not_erase_the_molecule_amounts_given_as_parameter()
      {
         _options.UseRoundedValue = false;
         var results = await sut.CalculateScaleDivisorsAsync(_simulation, _options, _moleculeAmountCache);
         _moleculeAmountCache.Any().ShouldBeTrue();
      }
   }

   public class When_resetting_the_scale_divisior_defined_in_a_list_of_molecules : concern_for_ScaleDivisorCalculator
   {
      private IReadOnlyCollection<ScaleDivisor> _result;

      protected override void Because()
      {
         _result = sut.ResetScaleDivisors(_moleculeAmountCache);
      }

      [Observation]
      public void should_return_a_list_of_scale_factor_with_value_set_to_1()
      {
         _result.Count.ShouldBeEqualTo(_moleculeAmountCache.Count);
         _result.Each(x => x.Value.ShouldBeEqualTo(1));
      }
   }
}