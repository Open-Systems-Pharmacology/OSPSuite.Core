using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;
using static OSPSuite.Helpers.ConstantsForSpecs;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core
{
   internal abstract class concern_for_ModelConstructor : ContextForIntegration<IModelConstructor>
   {
      protected SimulationConfiguration _simulationConfiguration;
      protected CreationResult _result;
      protected IModel _model;

      protected string _modelName = "MyModel";
      protected SimulationBuilder _simulationBuilder;

      protected override void Context()
      {
         base.Context();
         //we create the simulation in context as some tests are modifying it
         _simulationConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateSimulationConfiguration();
      }

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         _model = _result.Model;
         _simulationBuilder = _result.SimulationBuilder;
      }
   }

   internal class When_running_the_case_study : concern_for_ModelConstructor
   {
      [Observation]
      public void should_return_a_successful_validation()
      {
         //the case study creates a warning for a parameter not found
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
      }

      [Observation]
      public void transport_T1_1_should_take_place_between_arterial_plasma_and_bone_plasma_for_species_A_and_B()
      {
         var A = _model.MoleculeContainerInNeighborhood("art_pls_to_bon_pls", "A");
         A.ContainsName("T1_1").ShouldBeTrue();

         var B = _model.MoleculeContainerInNeighborhood("art_pls_to_bon_pls", "B");
         B.ContainsName("T1_1").ShouldBeTrue();
      }

      [Observation]
      public void transport_T1_1_should_have_local_parameters_set()
      {
         var transports = _model.Neighborhoods.GetAllChildren<Transport>(t => t.Name.Equals("T1_1"));

         foreach (var transport in transports)
         {
            var p1 = transport.GetSingleChildByName<IParameter>("p1");
            p1.ShouldNotBeNull();
            p1.Value.ShouldBeEqualTo(1);
         }
      }

      [Observation]
      public void transport_T1_2_should_take_place_between_bone_plasma_and_venous_plasma_for_species_A_and_B()
      {
         var A = _model.MoleculeContainerInNeighborhood("bon_pls_to_ven_pls", "A");
         A.ContainsName("T1_2").ShouldBeTrue();

         var B = _model.MoleculeContainerInNeighborhood("bon_pls_to_ven_pls", "B");
         B.ContainsName("T1_2").ShouldBeTrue();
      }

      [Observation]
      public void transport_T1_3_should_take_place_between_venous_plasma_and_lung_plasma_for_species_A_and_B()
      {
         var A = _model.MoleculeContainerInNeighborhood("ven_pls_to_lng_pls", "A");
         A.ContainsName("T1_3").ShouldBeTrue();

         var B = _model.MoleculeContainerInNeighborhood("ven_pls_to_lng_pls", "B");
         B.ContainsName("T1_3").ShouldBeTrue();
      }

      [Observation]
      public void transport_T1_4_should_take_place_between_lung_plasma_and_arterial_plasma_for_species_A_and_B()
      {
         var A = _model.MoleculeContainerInNeighborhood("lng_pls_to_art_pls", "A");
         A.ContainsName("T1_4").ShouldBeTrue();

         var B = _model.MoleculeContainerInNeighborhood("lng_pls_to_art_pls", "B");
         B.ContainsName("T1_4").ShouldBeTrue();
      }

      [Observation]
      public void transport_T2_should_take_place_between_bone_plasma_and_bone_cell_for_species_A_and_B_but_not_C()
      {
         var A = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A");
         A.ContainsName("T2").ShouldBeTrue();

         var B = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "B");
         B.ContainsName("T2").ShouldBeTrue();

         var C = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "C");
         C.ShouldBeNull();
      }

      [Observation]
      public void transport_T2_should_take_place_between_lung_plasma_and_lung_cell_for_species_A_and_B_but_not_C()
      {
         var A = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A");
         A.ContainsName("T2").ShouldBeTrue();

         var B = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "B");
         B.ContainsName("T2").ShouldBeTrue();

         var C = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "C");
         C.ShouldBeNull();
      }

      [Observation]
      public void reaction_R1_should_only_take_place_in_bone_cell_and_lung_cell()
      {
         var organism = _model.Root;
         var allContainingR1 = from reaction in organism.GetAllChildren<Reaction>()
            where reaction.Name.Equals("R1")
            select reaction.ParentContainer;

         var boneCell = _model.ModelOrganCompartment(Bone, Cell);
         var lungCell = _model.ModelOrganCompartment(Lung, Cell);

         allContainingR1.ShouldOnlyContain(boneCell, lungCell);
      }

      [Observation]
      public void Influx_transport_from_transporter_D_should_only_take_place_between_lung_plasma_and_lung_cell_and_should_only_transport_molecule_A_and_B()
      {
         var A = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A");
         var B = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "B");

         var allContainingTransporter1 = _model.Neighborhoods.GetAllChildren<Neighborhood>()
            .SelectMany(x => x.GetAllChildren<IContainer>()).Where(c => c.ContainsName("My Transport1"));

         allContainingTransporter1.ShouldOnlyContain(A, B);
      }

      [Observation]
      public void Efflux_transport_from_transporter_E_should_only_take_place_between_bone_cell_and_bon_plasma_and_should_only_transport_molecule_A()
      {
         var A = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A");

         var allContainingTransporter1 = _model.Neighborhoods.GetAllChildren<Neighborhood>()
            .SelectMany(x => x.GetAllChildren<IContainer>()).Where(c => c.ContainsName("My Transport2"));

         allContainingTransporter1.ShouldOnlyContain(A);
      }

      [Observation]
      public void Observer_AmountObs_1_should_have_only_been_created_for_molecules_A_and_B_in_art_plasma_and_lung_plasma_and_ven_plasma_and_bone_plasma_und_molecule_D_in_lung_plasma_and_ven_plasma_and_molecule_E_in_ven_plasma()
      {
         string observerName = "AmountObs_1";
         var allContainingObserver = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName(observerName));
         var art_pls_A = _model.ModelOrganCompartmentMolecule(ArterialBlood, Plasma, "A");
         var art_pls_B = _model.ModelOrganCompartmentMolecule(ArterialBlood, Plasma, "B");

         var lng_pls_A = _model.ModelOrganCompartmentMolecule(Lung, Plasma, "A");
         var lng_pls_D = _model.ModelOrganCompartmentMolecule(Lung, Plasma, "D");
         var lng_pls_B = _model.ModelOrganCompartmentMolecule(Lung, Plasma, "B");

         var ven_pls_A = _model.ModelOrganCompartmentMolecule(VenousBlood, Plasma, "A");
         var ven_pls_B = _model.ModelOrganCompartmentMolecule(VenousBlood, Plasma, "B");
         var ven_pls_D = _model.ModelOrganCompartmentMolecule(VenousBlood, Plasma, "D");
         var ven_pls_E = _model.ModelOrganCompartmentMolecule(VenousBlood, Plasma, "E");

         var bon_pls_A = _model.ModelOrganCompartmentMolecule(Bone, Plasma, "A");
         var bon_pls_B = _model.ModelOrganCompartmentMolecule(Bone, Plasma, "B");

         allContainingObserver.ShouldOnlyContain(art_pls_A, art_pls_B, lng_pls_A, lng_pls_B, lng_pls_D, ven_pls_A, ven_pls_B, ven_pls_D, ven_pls_E, bon_pls_A, bon_pls_B);
      }

      [Observation]
      public void Observer_AmountObs_2_should_have_only_been_created_for_molecules_C_in_lung_cell_and_bone_cell_and_molecule_E_in_bone_cell()
      {
         string observerName = "AmountObs_2";
         var lung_cell_C = _model.ModelOrganCompartmentMolecule(Lung, Cell, "C");
         var bon_cell_C = _model.ModelOrganCompartmentMolecule(Bone, Cell, "C");
         var bon_cell_E = _model.ModelOrganCompartmentMolecule(Bone, Cell, "E");
         var allContainingObserver = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName(observerName));
         allContainingObserver.ShouldOnlyContain(lung_cell_C, bon_cell_C, bon_cell_E);
      }

      [Observation]
      public void Observer_AmountObs_3_should_have_only_been_created_for_molecules_A_B_D_in_lung_cell_and_bone_cell()
      {
         string observerName = "AmountObs_3";
         var lung_cell_A = _model.ModelOrganCompartmentMolecule(Lung, Cell, "A");
         var lung_cell_B = _model.ModelOrganCompartmentMolecule(Lung, Cell, "B");
         ;
         var bon_cell_A = _model.ModelOrganCompartmentMolecule(Bone, Cell, "A");
         var bon_cell_B = _model.ModelOrganCompartmentMolecule(Bone, Cell, "B");

         var bon_cell_F = _model.ModelOrganCompartmentMolecule(Bone, Cell, "F");
         var allContainingObserver = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName(observerName));
         allContainingObserver.ShouldOnlyContain(lung_cell_A, lung_cell_B, bon_cell_A, bon_cell_B, bon_cell_F);
      }

      [Observation]
      public void Observer_ContainerObs_1_should_have_been_created_under_the_molecule_properties_container_for_C_under_organism()
      {
         string observerName = "ContainerObs_1";

         var organism = _model.Root.GetSingleChildByName<IContainer>(ORGANISM);
         var moleculeContainer = organism.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeTrue();
      }

      [Observation]
      public void Observer_InContainerObserver_should_have_been_created_under_the_molecule_properties_container_for_C_under_lung_plasma()
      {
         string observerName = "InContainerObserver";

         var lung_plasma = _model.ModelOrganCompartment(Lung, Plasma);
         var moleculeContainer = lung_plasma.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeTrue();
      }

      [Observation]
      public void Observer_NotInContainerObserver_should_have_been_created_under_the_molecule_properties_container_for_C_under_all_plasma_except_lung()
      {
         string observerName = "NotInContainerObserver";

         var lung_plasma = _model.ModelOrganCompartment(Lung, Plasma);
         var moleculeContainer = lung_plasma.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeFalse();


         var bone_plasma = _model.ModelOrganCompartment(Bone, Plasma);
         moleculeContainer = bone_plasma.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeTrue();
      }

      [Observation]
      public void bolus_application_should_only_take_place_in_arterial_blood_plasma()
      {
         var bolusApplication = _model.ModelOrganCompartment(ArterialBlood, Plasma);
         var allContainingBolusApplication = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName("Bolus Application"));
         allContainingBolusApplication.ShouldOnlyContain(bolusApplication);
      }

      [Observation]
      public void distributed_parameter_should_not_be_fixed()
      {
         var distributedParameter =
            _model.Root.GetAllChildren<IDistributedParameter>(child => child.Name.Equals("Distributed")).Single();
         distributedParameter.IsFixedValue.ShouldBeFalse();
      }

      [Observation]
      public void should_have_used_the_calculation_method_CM1_for_the_molecule_A()
      {
         _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A").GetSingleChildByName<IParameter>("K").Formula.Name.ShouldBeEqualTo("PartitionCoeff_1");
         _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A").GetSingleChildByName<IParameter>("K").Formula.Name.ShouldBeEqualTo("PartitionCoeff_1");
      }

      [Observation]
      public void should_have_used_the_calculation_method_CM2_for_the_molecule_B()
      {
         _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "B").GetSingleChildByName<IParameter>("K").Formula.Name.ShouldBeEqualTo("PartitionCoeff_2");
         _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "B").GetSingleChildByName<IParameter>("K").Formula.Name.ShouldBeEqualTo("PartitionCoeff_2");
      }

      [Observation]
      public void should_have_created_a_help_parameter_for_the_molecule_A_under_lung_plasma_with_the_value_10()
      {
         var lungPlasmaA = _model.ModelOrganCompartmentMolecule(Lung, Plasma, "A");
         var parameterHelp = lungPlasmaA.GetSingleChildByName<IParameter>("HelpMe");
         parameterHelp.ShouldNotBeNull();
         parameterHelp.Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_created_a_help_parameter_for_the_molecule_A_under_bone_plasma_with_the_value_20()
      {
         var lungPlasmaA = _model.ModelOrganCompartmentMolecule(Bone, Plasma, "A");
         var parameterHelp = lungPlasmaA.GetSingleChildByName<IParameter>("HelpMe");
         parameterHelp.ShouldNotBeNull();
         parameterHelp.Value.ShouldBeEqualTo(20);
      }

      [Observation]
      public void should_have_created_a_parameter_with_criteria_in_plasma_but_not_in_cells()
      {
         var bone_plasma_A = _model.ModelOrganCompartmentMolecule(Bone, Plasma, "A");
         var localWithCriteria = bone_plasma_A.Parameter("LocalWithCriteria");
         localWithCriteria.ShouldNotBeNull();
         var bone_cells_A = _model.ModelOrganCompartmentMolecule(Bone, Cell, "A");
         localWithCriteria = bone_cells_A.Parameter("LocalWithCriteria");
         localWithCriteria.ShouldBeNull();
      }

      [Observation]
      public void should_not_have_created_a_help_parameter_for_the_molecule_B_under_lung_plasma()
      {
         var lungPlasmaA = _model.ModelOrganCompartmentMolecule(Lung, Plasma, "B");
         var parameterHelp = lungPlasmaA.GetSingleChildByName<IParameter>("HelpMe");
         parameterHelp.ShouldBeNull();
      }

      [Observation]
      public void should_have_created_a_process_rate_parameter_for_transporter_1_with_the_tag_of_the_neighborhood_and_molecule()
      {
         var processRateParameter = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A")
            .GetSingleChildByName<IContainer>("My Transport1")
            .GetSingleChildByName<IContainer>("Transporter #1")
            .GetSingleChildByName<IParameter>(Parameters.PROCESS_RATE);

         processRateParameter.Tags.Contains(Parameters.PROCESS_RATE).ShouldBeTrue();
         processRateParameter.Tags.Contains("lng_pls_to_lng_cell").ShouldBeTrue();
         processRateParameter.Tags.Contains("A").ShouldBeTrue();
      }

      [Observation]
      public void the_process_rate_parameters_created_should_be_hidden_and_not_editable()
      {
         var processRateParameter = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A")
            .GetSingleChildByName<IContainer>("My Transport1")
            .GetSingleChildByName<IContainer>("Transporter #1")
            .GetSingleChildByName<IParameter>(Parameters.PROCESS_RATE);

         processRateParameter.Editable.ShouldBeFalse();
         processRateParameter.Visible.ShouldBeFalse();
      }

      [Observation]
      public void should_have_created_a_process_rate_parameter_for_transporter_2_with_the_tag_of_the_neighborhood_and_molecule()
      {
         var processRateParameter = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A")
            .GetSingleChildByName<IContainer>("My Transport2")
            .GetSingleChildByName<IContainer>("Transporter #2")
            .GetSingleChildByName<IParameter>(Parameters.PROCESS_RATE);

         processRateParameter.Tags.Contains(Parameters.PROCESS_RATE).ShouldBeTrue();
         processRateParameter.Tags.Contains("bon_pls_to_bon_cell").ShouldBeTrue();
         processRateParameter.Tags.Contains("A").ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_two_explicit_formula_for_the_parameter_referencing_a_dynamic_formula()
      {
         _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A").GetSingleChildByName<IParameter>(SumProcessRate)
            .Formula.ShouldBeAnInstanceOf<ExplicitFormula>();

         _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A").GetSingleChildByName<IParameter>(SumProcessRate)
            .Formula.ShouldBeAnInstanceOf<ExplicitFormula>();
      }

      [Observation]
      public void should_have_expended_the_dynamic_formula_to_use_only_valid_parameters_in_lung_pls_to_cell_neighborhood()
      {
         var formula = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A").GetSingleChildByName<IParameter>(SumProcessRate)
            .Formula.DowncastTo<ExplicitFormula>();
         formula.ObjectPaths.Count().ShouldBeEqualTo(1);
         formula.ObjectPaths.ElementAt(0).ShouldContain("lng_pls_to_lng_cell", "A", "My Transport1", "Transporter #1", Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_expended_the_dynamic_formula_to_use_only_valid_parameters_in_bon_pls_to_cell_neighborhood()
      {
         var formula = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A").GetSingleChildByName<IParameter>(SumProcessRate)
            .Formula.DowncastTo<ExplicitFormula>();
         formula.ObjectPaths.Count().ShouldBeEqualTo(1);
         formula.ObjectPaths.ElementAt(0).ShouldContain("bon_pls_to_bon_cell", "A", "My Transport2", "Transporter #2", Parameters.PROCESS_RATE);
      }

      [Observation]
      public void each_parameter_using_formula_defined_in_the_simulation_should_have_a_corresponding_builder()
      {
         var errorList = new List<string>();
         foreach (var entity in _model.Root.GetAllChildren<IEntity>())
         {
            var builder = _simulationBuilder.BuilderFor(entity);
            if (builder != null) continue;
            if (entity.IsNamed(NEIGHBORHOODS)) continue;
            errorList.Add($"No builder found for {entity.Name}");
         }

         errorList.Count.ShouldBeEqualTo(0, errorList.ToString("\n"));
      }

      [Observation]
      public void should_have_created_ontogeny_factor_and_half_life_parameter_in_all_endogenous_molecules()
      {
         var bone_cell_F = _model.ModelOrganCompartmentMolecule(Bone, Cell, "F");
         bone_cell_F.ShouldNotBeNull();
         bone_cell_F.ContainsName(ONTOGENY_FACTOR).ShouldBeTrue();
         bone_cell_F.ContainsName(HALF_LIFE).ShouldBeTrue();
         bone_cell_F.ContainsName(DEGRADATION_COEFF).ShouldBeTrue();

         var bone_cell_E = _model.ModelOrganCompartmentMolecule(Bone, Cell, "E");
         bone_cell_E.ShouldNotBeNull();
         bone_cell_E.ContainsName(ONTOGENY_FACTOR).ShouldBeFalse();
         bone_cell_E.ContainsName(HALF_LIFE).ShouldBeFalse();
         bone_cell_E.ContainsName(DEGRADATION_COEFF).ShouldBeFalse();
      }

      [Observation]
      public void should_have_update_the_scale_factor()
      {
         var bone_cell_E = _model.ModelOrganCompartmentMolecule(Bone, Cell, "E");
         bone_cell_E.ScaleDivisor.ShouldBeEqualTo(2.5);
      }

      [Observation]
      public void should_have_removed_local_molecule_parameters_with_a_NAN_value()
      {
         var bone_cell_E = _model.ModelOrganCompartmentMolecule(Bone, Cell, "E");
         bone_cell_E.Parameter("NaNParam").ShouldBeNull();
         bone_cell_E.Parameter("OtherNaNParam").ShouldNotBeNull();
      }

      [Observation]
      public void should_have_not_removed_global_nan_parameters()
      {
         var global_container = _model.Root.EntityAt<IContainer>("E");
         global_container.Parameter("GlobalNaNParam").ShouldNotBeNull();
      }

      [Observation]
      public void should_have_added_the_local_transporter_process_parameters_in_the_local_transporter_process_container()
      {
         var localParameter = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A")
            .EntityAt<IParameter>("My Transport1", "LocalTransportParameter");
         localParameter.ShouldNotBeNull();
         localParameter.Value.ShouldBeEqualTo(250);
      }

      [Observation]
      public void should_have_updated_the_formula_of_any_formula_parameter_in_the_spatial_structure_whose_value_was_overwritten_in_the_parameter_value_with_a_formula_and_a_constant()
      {
         var bone_cell = _model.ModelOrganCompartment(Bone, Cell);
         var parameter = bone_cell.Parameter("FormulaParameterOverwritten");
         parameter.Value.ShouldBeEqualTo(300);
         parameter.IsFixedValue.ShouldBeTrue();
         //formula is kept
         parameter.Formula.IsConstant().ShouldBeFalse();
      }

      [Observation]
      public void should_track_the_parameter_updated_by_a_formula_accordingly()
      {
         var bone_cell = _model.ModelOrganCompartment(Bone, Cell);
         var parameter = bone_cell.Parameter("FormulaParameterOverwritten");
         var parameterValues = _simulationConfiguration.ModuleConfigurations[0].SelectedParameterValues;
         var parameterValue = parameterValues.First(x => x.Name == parameter.Name);
         var entitySource = _simulationBuilder.SimulationEntitySourceFor(parameter);
         entitySource.SourcePath.ShouldBeEqualTo(parameterValue.Path);
         entitySource.SimulationEntityPath.ShouldBeEqualTo(new[] { ORGANISM, Bone, Cell, "FormulaParameterOverwritten" }.ToPathString());
      }

      [Observation]
      public void should_be_able_to_resolve_global_parameters_defined_in_reaction_referencing_other_global_parameters_from_another_reaction()
      {
         var r2k2Global = _model.Root.EntityAt<IParameter>("R2", "k2");
         var r1k2Global = _model.Root.EntityAt<IParameter>("R1", "k2");
         r2k2Global.Value.ShouldBeEqualTo(r1k2Global.Value);
      }

      [Observation]
      public void should_have_update_the_value_of_a_parameter_defined_in_the_individual_that_exists_in_the_spatial_structure()
      {
         var P_arterial_blood = _model.Root.EntityAt<IParameter>(ORGANISM, ArterialBlood, P);
         P_arterial_blood.Value.ShouldBeEqualTo(20);
      }

      [Observation]
      public void should_have_created_a_new_parameter_defined_in_the_individual_for_entries_that_do_not_exist_in_the_spatial_structure()
      {
         var NEW_PARAM_arterial_blood = _model.Root.EntityAt<IParameter>(ORGANISM, ArterialBlood, "NEW_PARAM");
         NEW_PARAM_arterial_blood.Value.ShouldBeEqualTo(10);
         NEW_PARAM_arterial_blood.Dimension.Name.ShouldBeEqualTo(Dimension.AMOUNT_PER_TIME);
      }

      [Observation]
      public void should_have_created_a_new_distributed_parameter_defined_in_the_individual_for_entries_that_do_not_exist_in_the_spatial_structure()
      {
         var NEW_PARAM_DISTRIBUTED_blood = _model.Root.EntityAt<IDistributedParameter>(ORGANISM, ArterialBlood, "NEW_PARAM_DISTRIBUTED");
         NEW_PARAM_DISTRIBUTED_blood.Value.ShouldBeEqualTo(10);
         NEW_PARAM_DISTRIBUTED_blood.Dimension.Name.ShouldBeEqualTo(Dimension.AMOUNT_PER_TIME);
      }
   }

   internal class When_a_parameter_value_is_defined_with_formula_and_nan_value : concern_for_ModelConstructor
   {
      private ParameterValue _parameterValue;

      protected override void Context()
      {
         base.Context();
         var simulationBuilder = new SimulationBuilder(_simulationConfiguration);

         _parameterValue = simulationBuilder.ParameterValues.First(x => x.Name.Equals("FormulaParameterOverwritten"));
         _parameterValue.Value = double.NaN;
         _parameterValue.Formula = new ExplicitFormula("1");
      }

      [Observation]
      public void should_not_overwrite_the_formula_with_NaN_fixed_value()
      {
         var targetParameter = _parameterValue.Path.TryResolve<IParameter>(_result.Model.Root);
         targetParameter.IsFixedValue.ShouldBeFalse();
         targetParameter.Value.ShouldNotBeEqualTo(double.NaN);
      }
   }

   internal class When_a_initial_condition_is_defined_for_logical_container : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         //we use a sim builder here to modify the configuration on the fly
         var simulationBuilder = new SimulationBuilder(_simulationConfiguration);

         var initialCondition = simulationBuilder.InitialConditions.First();
         var physicalContainer = simulationBuilder.SpatialStructureAndMergeBehaviors.SelectMany(x => x.spatialStructure.TopContainers)
            .Select(x => initialCondition.ContainerPath.TryResolve<IContainer>(x)).First(x => x != null);
         physicalContainer.Mode = ContainerMode.Logical;

         // "Organism|ArterialBlood|Plasma" -> is being set to Logical explicitly on the previous line,
         // as we now are not creating neighborhoods from logical containers, a further validation ends up on this exception:
         // Could not find neighborhood between 'MyModel|Organism|ArterialBlood|Plasma' and 'MyModel|Organism|Bone|Plasma'
         // referenced by formula 'FormulaReferencingNBH' used by 'MyModel|Organism|ArterialBlood|Plasma|RefParam'
         // This parameter has to be removed since the validation will make this fail before testing the initial condition.
         var parameterWithFormula = physicalContainer.Children.FirstOrDefault(x => x.Name == "RefParam") as Parameter;
         physicalContainer.RemoveChild(parameterWithFormula);
      }

      [Observation]
      public void should_identify_an_issue_with_initial_condition_being_defined_for_logical_containers()
      {
         _result.ValidationResult.Messages.Count(message => message.Object.IsAnImplementationOf<InitialCondition>()).ShouldBeEqualTo(2);
      }
   }

   internal class When_running_the_case_study_with_a_missing_parameter : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         //we use a sim builder here to modify the configuration on the fly
         var simulationBuilder = new SimulationBuilder(_simulationConfiguration);
         var molecule = simulationBuilder.MoleculeByName("A");
         var paraToRemove = molecule.Parameters.SingleOrDefault(para => para.Name == "logMA");
         molecule.RemoveParameter(paraToRemove);
      }

      [Observation]
      public void should_return_an_invalid_validation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
         _result.ValidationResult.Messages.Count().ShouldBeEqualTo(4, _result.ValidationResult.Messages.Select(x => x.Text).ToString("\n"));
      }
   }

   internal class When_naming_the_model_like_a_distributed_parameter_sub_parameter : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         _modelName = "Mean";
      }

      [Observation]
      public void should_be_able_to_create_the_model()
      {
         _result.ValidationResult.ValidationState.ShouldNotBeEqualTo(ValidationState.Invalid);
      }
   }

   internal class When_naming_the_model_like_the_neighborhood_container : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         _modelName = NEIGHBORHOODS;
      }

      [Observation]
      public void should_not_be_able_to_create_the_model()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   internal class When_naming_the_model_like_the_organism_container : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         _modelName = ORGANISM;
      }

      [Observation]
      public void should_not_be_able_to_create_the_model()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
         _result.ValidationResult.Messages.Count().ShouldBeEqualTo(1, _result.ValidationResult.Messages.Select(x => x.Text).ToString("\n"));
         _result.ValidationResult.Messages.ElementAt(0).Text.ShouldBeEqualTo(Validation.ModelNameCannotBeNamedLikeATopContainer(_model.Root.GetChildren<IContainer>().AllNames()));
      }
   }

   internal class When_a_parameter_is_added_dynamically_from_the_parameter_values_in_a_well_defined_container : concern_for_ModelConstructor
   {
      [Observation]
      public void should_have_added_the_dynamic_parameter_in_the_target_container()
      {
         var parameter = _model.Root.EntityAt<IParameter>(ORGANISM, "NewParameterAddedFromParameterValues");
         parameter.ShouldNotBeNull();
         parameter.Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_track_the_parameter_accordingly()
      {
         var parameter = _model.Root.EntityAt<IParameter>(ORGANISM, "NewParameterAddedFromParameterValues");
         var parameterValues = _simulationConfiguration.ModuleConfigurations[0].SelectedParameterValues;
         var parameterValue = parameterValues.First(x => x.Name == parameter.Name);
         _simulationBuilder.SimulationEntitySourceFor(parameter).SourcePath.ShouldBeEqualTo(parameterValue.Path);
      }
   }
}