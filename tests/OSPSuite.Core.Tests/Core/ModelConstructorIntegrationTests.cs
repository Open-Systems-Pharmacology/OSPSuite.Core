using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core
{
   public abstract class concern_for_ModelConstructor : ContextForIntegration<IModelConstructor>
   {
      protected IBuildConfiguration _buildConfiguration;
      protected CreationResult _result;
      protected IModel _model;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _buildConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateBuildConfiguration();
      }

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         _result = sut.CreateModelFrom(_buildConfiguration, "MyModel");
         _model = _result.Model;
      }
   }

   public class When_running_the_case_study : concern_for_ModelConstructor
   {
      [Observation]
      public void should_return_a_successful_validation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Valid, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
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
         var transports = _model.Neighborhoods.GetAllChildren<ITransport>(t => t.Name.Equals("T1_1"));

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
         var allContainingR1 = from reaction in organism.GetAllChildren<IReaction>()
            where reaction.Name.Equals("R1")
            select reaction.ParentContainer;

         var boneCell = _model.ModelOrganCompartment(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell);
         var lungCell = _model.ModelOrganCompartment(ConstantsForSpecs.Lung, ConstantsForSpecs.Cell);

         allContainingR1.ShouldOnlyContain(boneCell, lungCell);
      }

      [Observation]
      public void Influx_transport_from_transporter_D_should_only_take_place_between_lung_plasma_and_lung_cell_and_should_only_transport_molecule_A_and_B()
      {
         var A = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A");
         var B = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "B");

         var allContainingTransporter1 = _model.Neighborhoods.GetAllChildren<INeighborhood>()
            .SelectMany(x => x.GetAllChildren<IContainer>()).Where(c => c.ContainsName("My Transport1"));

         allContainingTransporter1.ShouldOnlyContain(A, B);
      }

      [Observation]
      public void Efflux_transport_from_transporter_E_should_only_take_place_between_bone_cell_and_bon_plasma_and_should_only_transport_molecule_A()
      {
         var A = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A");

         var allContainingTransporter1 = _model.Neighborhoods.GetAllChildren<INeighborhood>()
            .SelectMany(x => x.GetAllChildren<IContainer>()).Where(c => c.ContainsName("My Transport2"));

         allContainingTransporter1.ShouldOnlyContain(A);
      }

      [Observation]
      public void Observer_AmoutObs_1_should_have_only_been_created_for_molecules_A_and_B_in_art_plasma_and_lung_plasma_and_ven_plasma_and_bone_plasma_und_molecule_D_in_lung_plasma_and_ven_plasma_and_molecule_E_in_ven_plasma()
      {
         string observerName = "AmountObs_1";
         var allContainingObserver = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName(observerName));
         var art_pls_A = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.ArterialBlood, ConstantsForSpecs.Plasma, "A");
         var art_pls_B = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.ArterialBlood, ConstantsForSpecs.Plasma, "B");

         var lng_pls_A = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma, "A");
         var lng_pls_D = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma, "D");
         var lng_pls_B = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma, "B");

         var ven_pls_A = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.VenousBlood, ConstantsForSpecs.Plasma, "A");
         var ven_pls_B = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.VenousBlood, ConstantsForSpecs.Plasma, "B");
         var ven_pls_D = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.VenousBlood, ConstantsForSpecs.Plasma, "D");
         var ven_pls_E = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.VenousBlood, ConstantsForSpecs.Plasma, "E");

         var bon_pls_A = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Plasma, "A");
         var bon_pls_B = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Plasma, "B");

         allContainingObserver.ShouldOnlyContain(art_pls_A, art_pls_B, lng_pls_A, lng_pls_B, lng_pls_D, ven_pls_A, ven_pls_B, ven_pls_D, ven_pls_E, bon_pls_A, bon_pls_B);
      }

      [Observation]
      public void Observer_AmoutObs_2_should_have_only_been_created_for_molecules_C_in_lung_cell_and_bone_cell_and_molecule_E_in_bone_cell()
      {
         string observerName = "AmountObs_2";
         var lung_cell_C = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Cell, "C");
         var bon_cell_C = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "C");
         var bon_cell_E = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "E");
         var allContainingObserver = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName(observerName));
         allContainingObserver.ShouldOnlyContain(lung_cell_C, bon_cell_C, bon_cell_E);
      }

      [Observation]
      public void Observer_AmoutObs_3_should_have_only_been_created_for_molecules_A_B_D_in_lung_cell_and_bone_cell()
      {
         string observerName = "AmountObs_3";
         var lung_cell_A = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Cell, "A");
         var lung_cell_B = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Cell, "B");
         ;
         var bon_cell_A = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "A");
         var bon_cell_B = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "B");

         var bon_cell_F = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "F");
         var allContainingObserver = _model.Root.GetAllChildren<IContainer>().Where(c => c.ContainsName(observerName));
         allContainingObserver.ShouldOnlyContain(lung_cell_A, lung_cell_B, bon_cell_A, bon_cell_B, bon_cell_F);
      }

      [Observation]
      public void Observer_ContainerObs_1_should_have_been_created_under_the_molecule_properties_container_for_C_under_organism()
      {
         string observerName = "ContainerObs_1";

         var organism = _model.Root.GetSingleChildByName<IContainer>(ConstantsForSpecs.Organism);
         var moleculeContainer = organism.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeTrue();
      }


      [Observation]
      public void Observer_InContainerObserver_should_have_been_created_under_the_molecule_properties_container_for_C_under_lung_plasma()
      {
         string observerName = "InContainerObserver";

         var lung_plasma = _model.ModelOrganCompartment(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma);
         var moleculeContainer = lung_plasma.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeTrue();
      }

      [Observation]
      public void Observer_NotInContainerObserver_should_have_been_created_under_the_molecule_properties_container_for_C_under_all_plasma_except_lung()
      {
         string observerName = "NotInContainerObserver";

         var lung_plasma = _model.ModelOrganCompartment(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma);
         var moleculeContainer = lung_plasma.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeFalse();


         var bone_plasma = _model.ModelOrganCompartment(ConstantsForSpecs.Bone, ConstantsForSpecs.Plasma);
         moleculeContainer = bone_plasma.GetSingleChildByName<IContainer>("C");
         moleculeContainer.ContainsName(observerName).ShouldBeTrue();

      }


      [Observation]
      public void bolus_application_should_only_take_place_in_arterial_blood_plasma()
      {
         var bolusApplication = _model.ModelOrganCompartment(ConstantsForSpecs.ArterialBlood, ConstantsForSpecs.Plasma);
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
         var lungPlasmaA = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma, "A");
         var parameterHelp = lungPlasmaA.GetSingleChildByName<IParameter>("HelpMe");
         parameterHelp.ShouldNotBeNull();
         parameterHelp.Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_created_a_help_parameter_for_the_molecule_A_under_boneg_plasma_with_the_value_20()
      {
         var lungPlasmaA = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Plasma, "A");
         var parameterHelp = lungPlasmaA.GetSingleChildByName<IParameter>("HelpMe");
         parameterHelp.ShouldNotBeNull();
         parameterHelp.Value.ShouldBeEqualTo(20);
      }

      [Observation]
      public void should_not_have_created_a_help_parameter_for_the_molecule_B_under_lung_plasma()
      {
         var lungPlasmaA = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Lung, ConstantsForSpecs.Plasma, "B");
         var parameterHelp = lungPlasmaA.GetSingleChildByName<IParameter>("HelpMe");
         parameterHelp.ShouldBeNull();
      }

      [Observation]
      public void should_have_created_a_process_rate_parameter_for_transporter_1_with_the_tag_of_the_neighborhood_and_molecule()
      {
         var processRateParameter = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A")
            .GetSingleChildByName<IContainer>("My Transport1")
            .GetSingleChildByName<IContainer>("Transporter #1")
            .GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);

         processRateParameter.Tags.Contains(Constants.Parameters.PROCESS_RATE).ShouldBeTrue();
         processRateParameter.Tags.Contains("lng_pls_to_lng_cell").ShouldBeTrue();
         processRateParameter.Tags.Contains("A").ShouldBeTrue();
      }

      [Observation]
      public void the_process_rate_parameters_created_should_be_hidden_and_not_editable()
      {
         var processRateParameter = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A")
            .GetSingleChildByName<IContainer>("My Transport1")
            .GetSingleChildByName<IContainer>("Transporter #1")
            .GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);

         processRateParameter.Editable.ShouldBeFalse();
         processRateParameter.Visible.ShouldBeFalse();
      }

      [Observation]
      public void should_have_created_a_process_rate_parameter_for_transporter_2_with_the_tag_of_the_neighborhood_and_molecule()
      {
         var processRateParameter = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A")
            .GetSingleChildByName<IContainer>("My Transport2")
            .GetSingleChildByName<IContainer>("Transporter #2")
            .GetSingleChildByName<IParameter>(Constants.Parameters.PROCESS_RATE);

         processRateParameter.Tags.Contains(Constants.Parameters.PROCESS_RATE).ShouldBeTrue();
         processRateParameter.Tags.Contains("bon_pls_to_bon_cell").ShouldBeTrue();
         processRateParameter.Tags.Contains("A").ShouldBeTrue();
      }

      [Observation]
      public void should_have_created_two_eplicit_formula_for_the_parameter_referencing_a_dynamic_formula()
      {
         _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A").GetSingleChildByName<IParameter>(ConstantsForSpecs.SumProcessRate)
            .Formula.ShouldBeAnInstanceOf<ExplicitFormula>();

         _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A").GetSingleChildByName<IParameter>(ConstantsForSpecs.SumProcessRate)
            .Formula.ShouldBeAnInstanceOf<ExplicitFormula>();
      }

      [Observation]
      public void should_have_expended_the_dynamic_formula_to_use_only_valid_parameters_in_lung_pls_to_cell_neighborhood()
      {
         var formula = _model.MoleculeContainerInNeighborhood("lng_pls_to_lng_cell", "A").GetSingleChildByName<IParameter>(ConstantsForSpecs.SumProcessRate)
            .Formula.DowncastTo<ExplicitFormula>();
         formula.ObjectPaths.Count().ShouldBeEqualTo(1);
         formula.ObjectPaths.ElementAt(0).ShouldContain("lng_pls_to_lng_cell", "A", "My Transport1", "Transporter #1", Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void should_have_expended_the_dynamic_formula_to_use_only_valid_parameters_in_bon_pls_to_cell_neighborhood()
      {
         var formula = _model.MoleculeContainerInNeighborhood("bon_pls_to_bon_cell", "A").GetSingleChildByName<IParameter>(ConstantsForSpecs.SumProcessRate)
            .Formula.DowncastTo<ExplicitFormula>();
         formula.ObjectPaths.Count().ShouldBeEqualTo(1);
         formula.ObjectPaths.ElementAt(0).ShouldContain("bon_pls_to_bon_cell", "A", "My Transport2", "Transporter #2", Constants.Parameters.PROCESS_RATE);
      }

      [Observation]
      public void each_parameter_using_formula_defined_in_the_simulation_should_have_a_corresponding_builder()
      {
         var errorList = new List<string>();
         foreach (var entity in _model.Root.GetAllChildren<IEntity>())
         {
            var builder = _buildConfiguration.BuilderFor(entity);
            if (builder != null) continue;
            if (entity.IsNamed(Constants.NEIGHBORHOODS)) continue;
            errorList.Add($"No builder found for {entity.Name}");
         }

         errorList.Count.ShouldBeEqualTo(0, errorList.ToString("\n"));
      }

      [Observation]
      public void should_have_created_ontogeny_factor_and_half_life_parameter_in_all_endogenous_molecules()
      {
         var bone_cell_F = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "F");
         bone_cell_F.ShouldNotBeNull();
         bone_cell_F.ContainsName(Constants.ONTOGENY_FACTOR).ShouldBeTrue();
         bone_cell_F.ContainsName(Constants.HALF_LIFE).ShouldBeTrue();
         bone_cell_F.ContainsName(Constants.DEGRADATION_COEFF).ShouldBeTrue();

         var bone_cell_E = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "E");
         bone_cell_E.ShouldNotBeNull();
         bone_cell_E.ContainsName(Constants.ONTOGENY_FACTOR).ShouldBeFalse();
         bone_cell_E.ContainsName(Constants.HALF_LIFE).ShouldBeFalse();
         bone_cell_E.ContainsName(Constants.DEGRADATION_COEFF).ShouldBeFalse();
      }

      [Observation]
      public void should_have_update_the_scale_factor()
      {
         var bone_cell_E = _model.ModelOrganCompartmentMolecule(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell, "E");
         bone_cell_E.ScaleDivisor.ShouldBeEqualTo(2.5);
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
      public void should_have_updated_the_formula_of_any_formula_parameter_in_the_spatial_structure_whose_value_was_overwritten_in_the_parameter_start_value_with_a_constant_formula()
      {
         var bone_cell = _model.ModelOrganCompartment(ConstantsForSpecs.Bone, ConstantsForSpecs.Cell);
         var parameter = bone_cell.Parameter("FormulaParameterOverwritten");
         parameter.Formula.IsConstant().ShouldBeTrue();
         parameter.Value.ShouldBeEqualTo(300);
         parameter.IsFixedValue.ShouldBeFalse();
      }

      [Observation]
      public void should_be_able_to_resolve_global_parmaeters_defined_in_reaction_referencing_other_global_parameters_from_another_reaction()
      {
         var r2k2Global = _model.Root.EntityAt<IParameter>("R2", "k2");
         var r1k2Global = _model.Root.EntityAt<IParameter>("R1", "k2");
         r2k2Global.Value.ShouldBeEqualTo(r1k2Global.Value);
      }
   }

   internal static class ModelExtensionsForSpecs
   {
      internal static IContainer MoleculeContainerInNeighborhood(this IModel model, string neighborhoodName, string moleculeName)
      {
         return model.Neighborhoods.GetSingleChildByName<INeighborhood>(neighborhoodName).GetSingleChildByName<IContainer>(moleculeName);
      }

      internal static IContainer ModelOrganCompartment(this IModel model, string organName, string compartmentName)
      {
         return model.Root.GetSingleChildByName<IContainer>(ConstantsForSpecs.Organism).GetSingleChildByName<IContainer>(organName).GetSingleChildByName<IContainer>(compartmentName);
      }

      internal static IMoleculeAmount ModelOrganCompartmentMolecule(this IModel model, string organName, string compartmentName, string moleculeName)
      {
         return model.ModelOrganCompartment(organName, compartmentName).GetSingleChildByName<IMoleculeAmount>(moleculeName);
      }
   }

   public class When_a_molecule_start_value_is_defined_for_logical_container : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         var moleculeStartValue = _buildConfiguration.MoleculeStartValues.First();
         var physicalContainer = _buildConfiguration.SpatialStructure.TopContainers.Select(x => moleculeStartValue.ContainerPath.TryResolve<IContainer>(x)).First(x => x != null);

         physicalContainer.Mode = ContainerMode.Logical;
      }

      [Observation]
      public void should_identify_an_issue_with_start_values_being_defined_for_logical_containers()
      {
         _result.ValidationResult.Messages.Count(message => message.Object.IsAnImplementationOf<IMoleculeStartValue>()).ShouldBeEqualTo(2);
      }
   }

   public class When_running_the_case_study_with_a_missing_parameter : concern_for_ModelConstructor
   {
      protected override void Context()
      {
         base.Context();
         var paraToRemove = _buildConfiguration.Molecules["A"].Parameters.SingleOrDefault(para => para.Name == "logMA");
         _buildConfiguration.Molecules["A"].RemoveParameter(paraToRemove);
      }

      [Observation]
      public void should_return_a_successful_validation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
         _result.ValidationResult.Messages.Count().ShouldBeEqualTo(1, _result.ValidationResult.Messages.Select(x => x.Text).ToString("\n"));
      }
   }
}