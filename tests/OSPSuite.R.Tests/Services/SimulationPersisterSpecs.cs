using System.Linq;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.CLI.Core.MinimalImplementations;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.R.Domain;
using OSPSuite.Utility;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_SimulationPersister : ContextForIntegration<ISimulationPersister>
   {
      protected override void Context()
      {
         sut = Api.GetSimulationPersister();
      }
   }

   public class When_loading_a_simulation_from_file : concern_for_SimulationPersister
   {
      private string _simulationFile;
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulationFile = HelperForSpecs.DataFile("S1.pkml");
      }

      protected override void Because()
      {
         _simulation = sut.LoadSimulation(_simulationFile);
      }

      [Observation]
      public void should_be_able_to_return_the_expected_simulation()
      {
         _simulation.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_kept_the_grouping_information_on_loaded_parameters()
      {
         var parameter = _simulation.Model.Root.EntityAt<IParameter>(Constants.ORGANISM, Constants.Parameters.WEIGHT);
         parameter.GroupName.ShouldNotBeEqualTo(Constants.Groups.UNDEFINED);
      }

      [Observation]
      public void the_group_repository_is_registered_correctly()
      {
         Api.Container.Resolve<IGroupRepository>().ShouldBeAnInstanceOf<GroupRepository>();
      }
   }

   public class When_saving_a_simulation_to_file : concern_for_SimulationPersister
   {
      private string _simulationFile;
      private Simulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulationFile = FileHelper.GenerateTemporaryFileName();
         var fileName = HelperForSpecs.DataFile("S1.pkml");
         _simulation = sut.LoadSimulation(fileName);
      }

      protected override void Because()
      {
         sut.SaveSimulation(_simulation, _simulationFile);
      }

      [Observation]
      public void should_be_able_to_return_the_expected_simulation()
      {
         FileHelper.FileExists(_simulationFile).ShouldBeTrue();
      }

      [Observation]
      public void should_not_write_any_change_to_the_file()
      {
         XElement.Load(_simulationFile).Element(nameof(SimulationTransfer.OriginalQuantityValues)).ShouldBeNull();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_simulationFile);
      }
   }

   public abstract class concern_for_saving_a_simulation_changed_in_R : concern_for_SimulationPersister
   {
      protected string _simulationFile;
      protected Simulation _simulation;
      protected SimulationTransfer _savedSimulationTransfer;
      protected IEntityPathResolver _entityPathResolver;

      protected override void Context()
      {
         base.Context();
         _entityPathResolver = Api.Container.Resolve<IEntityPathResolver>();
         _simulationFile = FileHelper.GenerateTemporaryFileName();
      }

      protected override void Because()
      {
         sut.SaveSimulation(_simulation, _simulationFile);
         _savedSimulationTransfer = Api.Container.Resolve<ISimulationPersistor>().Load(_simulationFile);
      }

      protected OriginalQuantityValue SavedOriginalValueFor(IEntity entity, OriginalQuantityValue.Types type = OriginalQuantityValue.Types.Quantity)
      {
         string path = _entityPathResolver.ObjectPathFor(entity);
         return _savedSimulationTransfer.OriginalQuantityValues.SingleOrDefault(x => x.Path == path && x.Type == type);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_simulationFile);
      }
   }

   public class When_saving_a_simulation_whose_values_were_changed_in_R : concern_for_saving_a_simulation_changed_in_R
   {
      private IParameter _constantParameter;
      private IParameter _formulaParameter;
      private IParameter _restoredParameter;
      private MoleculeAmount _moleculeAmount;
      private double _constantParameterValue;
      private double _formulaParameterValue;
      private double _moleculeAmountValue;
      private double _scaleDivisor;

      protected override void Context()
      {
         base.Context();
         _simulation = sut.LoadSimulation(HelperForSpecs.DataFile("S1.pkml"));
         var root = _simulation.Model.Root;
         var constantParameters = root.GetAllChildren<IParameter>(x => !x.IsFixedValue && x.Formula.IsConstant());
         _constantParameter = constantParameters[0];
         _restoredParameter = constantParameters[1];
         _formulaParameter = root.GetAllChildren<IParameter>(x => !x.IsFixedValue && x.Formula.IsExplicit() && x.Value > 0).First();
         _moleculeAmount = root.GetAllChildren<MoleculeAmount>().First();

         _constantParameterValue = _constantParameter.Value;
         _formulaParameterValue = _formulaParameter.Value;
         _moleculeAmountValue = _moleculeAmount.Value;
         _scaleDivisor = _moleculeAmount.ScaleDivisor;

         Api.GetContainerTask().SetValueByPath(_simulation, _entityPathResolver.PathFor(_constantParameter), _constantParameterValue + 1, throwIfNotFound: true);
         _formulaParameter.Value = _formulaParameterValue * 2;
         _moleculeAmount.Value = _moleculeAmountValue + 1;
         _moleculeAmount.ScaleDivisor = _scaleDivisor / 10;

         var restoredValue = _restoredParameter.Value;
         _restoredParameter.Value = restoredValue + 1;
         _restoredParameter.Value = restoredValue;
      }

      [Observation]
      public void should_write_the_original_value_of_the_changed_parameter()
      {
         SavedOriginalValueFor(_constantParameter).Value.ShouldBeEqualTo(_constantParameterValue);
      }

      [Observation]
      public void should_write_the_original_value_of_the_formula_parameter_overwritten_with_a_value()
      {
         SavedOriginalValueFor(_formulaParameter).Value.ShouldBeEqualTo(_formulaParameterValue);
      }

      [Observation]
      public void should_write_the_original_initial_value_and_scale_divisor_of_the_molecule()
      {
         SavedOriginalValueFor(_moleculeAmount).Value.ShouldBeEqualTo(_moleculeAmountValue);
         SavedOriginalValueFor(_moleculeAmount, OriginalQuantityValue.Types.ScaleDivisor).Value.ShouldBeEqualTo(_scaleDivisor);
      }

      [Observation]
      public void should_not_write_a_parameter_set_back_to_its_original_value()
      {
         SavedOriginalValueFor(_restoredParameter).ShouldBeNull();
      }

      [Observation]
      public void should_only_write_the_changed_quantities()
      {
         _savedSimulationTransfer.OriginalQuantityValues.Count.ShouldBeEqualTo(4);
      }
   }

   public class When_saving_a_simulation_with_changes_tracked_before_it_was_loaded_in_R : concern_for_saving_a_simulation_changed_in_R
   {
      private string _trackedSimulationFile;
      private IParameter _trackedParameter;
      private const double TRACKED_ORIGINAL_VALUE = 123;

      protected override void Context()
      {
         base.Context();
         var corePersistor = Api.Container.Resolve<ISimulationPersistor>();
         var simulationTransfer = corePersistor.Load(HelperForSpecs.DataFile("S1.pkml"));
         var parameter = simulationTransfer.Simulation.Model.Root.GetAllChildren<IParameter>(x => x.Formula.IsConstant()).First();
         var originalQuantityValue = Api.Container.Resolve<IQuantityToOriginalQuantityValueMapper>().MapFrom(parameter);
         originalQuantityValue.Value = TRACKED_ORIGINAL_VALUE;
         simulationTransfer.OriginalQuantityValues.Add(originalQuantityValue);
         simulationTransfer.HasUntraceableChanges = true;
         _trackedSimulationFile = FileHelper.GenerateTemporaryFileName();
         corePersistor.Save(simulationTransfer, _trackedSimulationFile);

         _simulation = sut.LoadSimulation(_trackedSimulationFile);
         _trackedParameter = _simulation.Model.Root.EntityAt<IParameter>(_entityPathResolver.PathFor(parameter).ToPathArray());
         _trackedParameter.Value = _trackedParameter.Value + 1;
      }

      [Observation]
      public void should_keep_the_original_value_of_the_tracked_change()
      {
         SavedOriginalValueFor(_trackedParameter).Value.ShouldBeEqualTo(TRACKED_ORIGINAL_VALUE);
         _savedSimulationTransfer.OriginalQuantityValues.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_keep_the_untraceable_changes_flag()
      {
         _savedSimulationTransfer.HasUntraceableChanges.ShouldBeTrue();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_trackedSimulationFile);
      }
   }
}