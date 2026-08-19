using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Helpers.ConstantsForSpecs;
using IContainer = OSPSuite.Core.Domain.IContainer;

namespace OSPSuite.Core
{
   internal abstract class concern_for_ModuleIntegration : ContextForIntegration<IModelConstructor>
   {
      protected CreationResult _result;
      protected IModel _model;

      protected string _modelName = "MyModel";
      protected SimulationBuilder _simulationBuilder;
      protected SimulationConfiguration _simulationConfiguration;

      protected abstract Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder();

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         var moduleHelper = IoC.Resolve<ModuleHelperForSpecs>();
         _simulationConfiguration = SimulationConfigurationBuilder()(moduleHelper);
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         _model = _result.Model;
         _simulationBuilder = _result.SimulationBuilder;
      }
   }

   internal class When_running_the_case_study_for_module_integration : concern_for_ModuleIntegration
   {
      [Observation]
      public void should_return_a_successful_validation_with_warning()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Warning.NeighborIsLogical(Bone, "not_physical"))).ShouldNotBeNull();
      }

      [Observation]
      public void should_have_added_a_tumor_organ_in_heart_organ()
      {
         var tumor = _model.ModelOrganCompartment(Heart, Tumor);
         tumor.ShouldNotBeNull();

         var tumor_pls = tumor.Container(Plasma);
         tumor_pls.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_left_the_original_container_of_the_spatial_structure_intact()
      {
         var module2 = _simulationConfiguration.ModuleConfigurations[1].Module;
         var lungModule2 = module2.SpatialStructure.FindByName(Lung);
         lungModule2.ShouldNotBeNull();
         lungModule2.ParentContainer.ShouldBeNull();
      }

      [Observation]
      public void should_have_created_the_neighborhood_between_a_physical_and_a_logical_container()
      {
         _model.Neighborhoods.FindByName("not_physical").ShouldNotBeNull();
      }

      [Observation]
      public void should_have_replaced_the_molecule_properties_with_the_ones_from_the_last_overwrite_module()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         moleculeAGlobalContainer.ShouldNotBeNull();

         //the last module defines an empty global molecule properties container that replaces the ones from the previous modules
         moleculeAGlobalContainer.Parameter("P1").ShouldBeNull();
         moleculeAGlobalContainer.Parameter("P2").ShouldBeNull();
         moleculeAGlobalContainer.Parameter("P3").ShouldBeNull();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationWithLogicalNeighborhood();
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_overwrite_for_passive_transport : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         var moduleHelper = IoC.Resolve<ModuleHelperForSpecs>();
         _simulationConfiguration = SimulationConfigurationBuilder()(moduleHelper);
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         _model = _result.Model;
      }

      [Observation]
      public void it_should_not_have_merged_the_molecule_list()
      {
         var lng_pls_to_lng_cell = _model.Root.EntityAt<Neighborhood>(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell");
         var passiveTransportA = lng_pls_to_lng_cell.EntityAt<Transport>("A", "PT1");
         var passiveTransportB = lng_pls_to_lng_cell.EntityAt<Transport>("B", "PT1");
         var passiveTransportC = lng_pls_to_lng_cell.EntityAt<Transport>("C", "PT1");
         //Module 1 is ALL EXCEPT B
         //Module 2 B and C
         //Result should be B and C only

         //it was not in the used module which only has B and C
         passiveTransportA.ShouldBeNull();
         passiveTransportB.ShouldNotBeNull();
         passiveTransportC.ShouldNotBeNull();
      }
   }

   internal class When_merging_the_global_molecule_properties_from_a_module_with_merge_behavior_overwrite : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForOverrideMergeBehavior();

      [Observation]
      public void should_only_have_created_the_molecule_parameters_defined_in_the_overwrite_module()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         moleculeAGlobalContainer.ShouldNotBeNull();

         moleculeAGlobalContainer.Parameter("P1").Value.ShouldBeEqualTo(100);
         moleculeAGlobalContainer.Parameter("P2").ShouldBeNull();
         moleculeAGlobalContainer.Parameter("P3").Value.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_track_the_parameters_to_the_overwrite_module()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         var parameter1 = moleculeAGlobalContainer.Parameter("P1");
         var parameter3 = moleculeAGlobalContainer.Parameter("P3");
         var module2 = _simulationConfiguration.ModuleConfigurations.Find(x => x.Module.IsNamed("Module2"));
         var module2Global = module2.Module.SpatialStructure.GlobalMoleculeDependentProperties;
         var parameter1Module2 = module2Global.Parameter("P1");
         var parameter3Module2 = module2Global.Parameter("P3");

         //we use entity path here because we are not dealing with simulations and we do not want to remove the first entry
         _simulationBuilder.SimulationEntitySourceFor(parameter1).SourcePath.ShouldBeEqualTo(parameter1Module2.EntityPath());
         _simulationBuilder.SimulationEntitySourceFor(parameter3).SourcePath.ShouldBeEqualTo(parameter3Module2.EntityPath());
      }
   }

   internal class When_merging_the_global_molecule_properties_from_a_module_with_merge_behavior_extend : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      [Observation]
      public void should_have_merged_the_existing_parameters()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         moleculeAGlobalContainer.ShouldNotBeNull();

         moleculeAGlobalContainer.Parameter("P1").Value.ShouldBeEqualTo(100);
         moleculeAGlobalContainer.Parameter("P2").Value.ShouldBeEqualTo(20);
         moleculeAGlobalContainer.Parameter("P3").Value.ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_track_the_parameter_accordingly()
      {
         var moleculeAGlobalContainer = _model.Root.Container("A");
         var parameter1 = moleculeAGlobalContainer.Parameter("P1");
         var parameter2 = moleculeAGlobalContainer.Parameter("P2");
         var parameter3 = moleculeAGlobalContainer.Parameter("P3");
         var module1 = _simulationConfiguration.ModuleConfigurations.Find(x => x.Module.IsNamed("Module1"));
         var module2 = _simulationConfiguration.ModuleConfigurations.Find(x => x.Module.IsNamed("Module2"));
         var module1Global = module1.Module.SpatialStructure.GlobalMoleculeDependentProperties;
         var module2Global = module2.Module.SpatialStructure.GlobalMoleculeDependentProperties;
         var parameter2Module1 = module1Global.Parameter("P2");
         var parameter1Module2 = module2Global.Parameter("P1");
         var parameter3Module2 = module2Global.Parameter("P3");

         //we use entity path here because we are not dealing with simulations and we do not want to remove the first entry
         _simulationBuilder.SimulationEntitySourceFor(parameter1).SourcePath.ShouldBeEqualTo(parameter1Module2.EntityPath());
         _simulationBuilder.SimulationEntitySourceFor(parameter2).SourcePath.ShouldBeEqualTo(parameter2Module1.EntityPath());
         _simulationBuilder.SimulationEntitySourceFor(parameter3).SourcePath.ShouldBeEqualTo(parameter3Module2.EntityPath());
      }
   }

   internal abstract class concern_for_module_integration_with_a_molecule_properties_top_container : concern_for_ModuleIntegration
   {
      private IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         base.Context();
         _objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
      }

      protected SimulationConfiguration AddMoleculePropertiesTopContainer(ModuleHelperForSpecs helper, SimulationConfiguration configuration)
      {
         var module1 = configuration.ModuleConfigurations[0].Module;
         var artPlasma = module1.SpatialStructure.TopContainers.FindByName(Constants.ORGANISM).EntityAt<IContainer>(ArterialBlood, Plasma);
         artPlasma.Add(createMoleculeProperties(helper, "K1", 5));

         var module2 = configuration.ModuleConfigurations[1].Module;
         var topMoleculeProperties = createMoleculeProperties(helper, "K2", 10);
         topMoleculeProperties.ParentPath = new ObjectPath(Constants.ORGANISM, ArterialBlood, Plasma);
         module2.SpatialStructure.AddTopContainer(topMoleculeProperties);

         return configuration;
      }

      private IContainer createMoleculeProperties(ModuleHelperForSpecs helper, string parameterName, double value)
      {
         var moleculeProperties = _objectBaseFactory.Create<IContainer>()
            .WithName(Constants.MOLECULE_PROPERTIES)
            .WithMode(ContainerMode.Logical)
            .WithContainerType(ContainerType.Molecule);

         moleculeProperties.Add(helper.NewConstantParameter(parameterName, value));
         return moleculeProperties;
      }

      protected IContainer MoleculeAInArterialBloodPlasma => _model.Root.EntityAt<IContainer>(Constants.ORGANISM, ArterialBlood, Plasma, "A");
   }

   internal class When_overwriting_a_molecule_properties_container_defined_as_top_container : concern_for_module_integration_with_a_molecule_properties_top_container
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() =>
         x => AddMoleculePropertiesTopContainer(x, x.CreateSimulationConfigurationForOverrideMergeBehavior());

      [Observation]
      public void should_only_add_the_local_molecule_parameters_defined_in_the_overwrite_module()
      {
         var moleculeA = MoleculeAInArterialBloodPlasma;
         moleculeA.ShouldNotBeNull();
         moleculeA.Parameter("K1").ShouldBeNull();
         moleculeA.Parameter("K2").Value.ShouldBeEqualTo(10);
      }
   }

   internal class When_extending_a_molecule_properties_container_defined_as_top_container : concern_for_module_integration_with_a_molecule_properties_top_container
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() =>
         x => AddMoleculePropertiesTopContainer(x, x.CreateSimulationConfigurationForExtendMergeBehavior());

      [Observation]
      public void should_add_the_local_molecule_parameters_defined_in_both_modules()
      {
         var moleculeA = MoleculeAInArterialBloodPlasma;
         moleculeA.ShouldNotBeNull();
         moleculeA.Parameter("K1").Value.ShouldBeEqualTo(5);
         moleculeA.Parameter("K2").Value.ShouldBeEqualTo(10);
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extent_for_passive_transport : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         var moduleHelper = IoC.Resolve<ModuleHelperForSpecs>();
         _simulationConfiguration = SimulationConfigurationBuilder()(moduleHelper);
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         _model = _result.Model;
      }

      [Observation]
      public void it_should_have_merged_the_molecule_list()
      {
         var lng_pls_to_lng_cell = _model.Root.EntityAt<Neighborhood>(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell");
         var tags = lng_pls_to_lng_cell.Tags.Select(x => x.Value).ToString(", ");

         lng_pls_to_lng_cell.Tags.Contains("NeighborhoodTag1").ShouldBeTrue(tags);
         lng_pls_to_lng_cell.Tags.Contains("NeighborhoodTag2").ShouldBeTrue(tags);
      }

      [Observation]
      public void the_equation_of_the_kinetic_tab_is_overwritten()
      {
         var lng_pls_to_lng_cell = _model.Root.EntityAt<Neighborhood>(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell");
         var passiveTransportB = lng_pls_to_lng_cell.EntityAt<Transport>("B", "PT1");
         //Value of the equation in module extending
         passiveTransportB.Formula.Calculate(passiveTransportB).ShouldBeEqualTo(2);
      }

      [Observation]
      public void the_parameter_list_is_extended()
      {
         var lng_pls_to_lng_cell = _model.Root.EntityAt<Neighborhood>(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell");
         var passiveTransportB = lng_pls_to_lng_cell.EntityAt<Transport>("B", "PT1");
         var parameter = passiveTransportB.Parameter("P_Extended");
         parameter.ShouldNotBeNull();
         parameter.Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void the_source_and_target_lists_are_extended()
      {
         // new conditions are added. It is not possible to remove source/target conditions.
         var passiveTransport = _result.SimulationBuilder.PassiveTransports.FindByName("PT1");
         passiveTransport.TargetCriteria.ToString().Contains("LUNG_CELL").ShouldBeTrue();
      }

      [Observation]
      public void include_exclude_list_for_molecules_are_extended()
      {
         var passiveTransport = _result.SimulationBuilder.PassiveTransports.FindByName("PT1");
         passiveTransport.MoleculeList.MoleculeNames.ShouldContain("B");
         passiveTransport.MoleculeList.MoleculeNames.ShouldContain("C");
         passiveTransport.MoleculeList.MoleculeNamesToExclude.ShouldNotContain("B");
      }

      [Observation]
      public void behavior_of_all_is_overwritten()
      {
         var passiveTransport = _result.SimulationBuilder.PassiveTransports.FindByName("PT1");
         passiveTransport.MoleculeList.ForAll.ShouldBeFalse();
      }

      [Observation]
      public void simple_properties_of_the_transporter_such_as_crate_process_rate_or_plot_process_rate_are_overwritten()
      {
         var passiveTransport = _result.SimulationBuilder.PassiveTransports.FindByName("PT1");
         passiveTransport.CreateProcessRateParameter.ShouldBeTrue();
         passiveTransport.ProcessRateParameterPersistable.ShouldBeTrue();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend_for_neighborhood : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      protected override void Because()
      {
         sut = IoC.Resolve<IModelConstructor>();
         var moduleHelper = IoC.Resolve<ModuleHelperForSpecs>();
         _simulationConfiguration = SimulationConfigurationBuilder()(moduleHelper);
         _result = sut.CreateModelFrom(_simulationConfiguration, _modelName);
         _model = _result.Model;
      }

      [Observation]
      public void should_have_merged_the_tag_at_the_neighborhood_level()
      {
         var lng_pls_to_lng_cell = _model.Root.EntityAt<Neighborhood>(Constants.NEIGHBORHOODS, "lng_pls_to_lng_cell");
         var tags = lng_pls_to_lng_cell.Tags.Select(x => x.Value).ToString(", ");

         lng_pls_to_lng_cell.Tags.Contains("NeighborhoodTag1").ShouldBeTrue(tags);
         lng_pls_to_lng_cell.Tags.Contains("NeighborhoodTag2").ShouldBeTrue(tags);
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_override_to_extend : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehaviorOverridingModuleBehavior();

      [Observation]
      public void should_have_merged_the_events_with_both_present()
      {
         var eventGroup1 = _model.Root.EntityAt<EventGroup>("Organism", "ArterialBlood", "Plasma", "eventGroup1");
         var eventGroup2 = _model.Root.EntityAt<EventGroup>("Organism", "ArterialBlood", "Plasma", "eventGroup2");
         eventGroup1.ShouldNotBeNull();
         eventGroup2.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_added_the_missing_parameters_to_lung()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P2).Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_merged_the_tag_at_the_container_level()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Tags.Contains("Tag1").ShouldBeTrue();
         lung.Tags.Contains("Tag2").ShouldBeTrue();
      }

      [Observation]
      public void should_have_updated_the_container_mode_to_the_one_of_the_merged_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Mode.ShouldBeEqualTo(ContainerMode.Physical);
      }

      [Observation]
      public void should_have_not_merged_the_tag_at_the_parameter_level()
      {
         var lungQ = _model.Root.EntityAt<Parameter>(Constants.ORGANISM, Lung, "Q");
         lungQ.Tags.Contains("ParamTag1").ShouldBeFalse();
         lungQ.Tags.Contains("ParamTag2").ShouldBeTrue();
      }

      [Observation]
      public void should_have_updating_existing_parameters()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(Q).Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_have_kept_parameters_defined_in_the_source_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P).Value.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_added_the_existing_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Container(Interstitial).ShouldNotBeNull();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehavior();

      [Observation]
      public void should_have_added_the_missing_parameters_to_lung()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P2).Value.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_updating_existing_parameters()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(Q).Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_have_kept_parameters_defined_in_the_source_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P).Value.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_added_the_existing_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Container(Interstitial).ShouldNotBeNull();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend_and_recursive_container : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehaviorWithRecursiveContainers();

      [Observation]
      public void should_have_added_the_missing_container_to_lung_and_arterial_blood()
      {
         var lngInt = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung, Interstitial);
         lngInt.ShouldNotBeNull();

         var artInt = _model.Root.EntityAt<Container>(Constants.ORGANISM, ArterialBlood, Interstitial);
         artInt.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_updating_existing_parameters()
      {
         var lungPls = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung, Plasma);
         lungPls.Parameter(Volume).Value.ShouldBeEqualTo(20);
         lungPls.Parameter(Q).Value.ShouldBeEqualTo(21);
      }

      [Observation]
      public void should_have_kept_parameters_defined_in_the_source_container()
      {
         var lung = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung);
         lung.Parameter(P).Value.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_have_kept_existing_container()
      {
         var lungCell = _model.Root.EntityAt<Container>(Constants.ORGANISM, Lung, Cell);
         lungCell.ShouldNotBeNull();
      }
   }

   internal class When_constructing_a_simulation_based_on_an_invalid_parmaeter_overwritten_with_PV : concern_for_ModuleIntegration
   {
      [Observation]
      public void should_be_able_to_construct_the_simulation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Valid, _result.ValidationResult.Messages.Select(x => x.Text).ToString("\n"));
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder()
      {
         //Construct a simple configuration with one module, one spatial structure with one parameter with a formula that cannot be evaluated and 
         //PV overrides the formula
         return helper =>
         {
            var objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
            var parameterValueCreator = IoC.Resolve<IParameterValuesCreator>();
            var simulationConfiguration = new SimulationConfiguration
            {
               SimulationSettings = helper.CreateSimulationSettings()
            };

            var module = objectBaseFactory.Create<Module>();
            var spatialStructure = helper.CreateSpatialStructure();
            var organism = helper.CreateOrganism();
            spatialStructure.AddTopContainer(organism);

            var explicitFormula = new ExplicitFormula("foo");
            explicitFormula.AddObjectPath(new FormulaUsablePath("foo").WithAlias("foo"));
            var parameter = helper.NewConstantParameter("param", 10).WithFormula(explicitFormula);
            organism.Add(parameter);

            module.Add(spatialStructure);

            var parameterValueBuildingBlock = objectBaseFactory.Create<ParameterValuesBuildingBlock>();
            parameterValueBuildingBlock.Add(parameterValueCreator.CreateParameterValue(new ObjectPath(organism.Name, "param"), 10, Constants.Dimension.NO_DIMENSION));
            module.Add(parameterValueBuildingBlock);
            var moduleConfiguration = new ModuleConfiguration(module);
            simulationConfiguration.AddModuleConfiguration(moduleConfiguration);

            return simulationConfiguration;
         };
      }
   }

   internal class When_overriding_neighbors_in_neighborhood : concern_for_ModuleIntegration
   {
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module1;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForExtendMergeBehavior();
         _module1 = configuration.ModuleConfigurations.ToArray()[0].Module;
         _module2 = configuration.ModuleConfigurations.ToArray()[1].Module;

         // reverse the neighbors to test that updating neighbors in a neighborhood works
         var neighborhoodToUpdate = _module1.SpatialStructure.Neighborhoods.FindByName("art_pls_to_bon_pls");
         var neighborhood = _neighborhoodFactory.CreateBetween(neighborhoodToUpdate.SecondNeighbor, neighborhoodToUpdate.FirstNeighbor);
         neighborhood.Name = neighborhoodToUpdate.Name;
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void the_simulation_neighborhood_neighbors_should_be_updated()
      {
         var simulationNeighborhood = _model.Root.GetAllChildren<IContainer>().FindByName("Neighborhoods").GetAllChildren<Neighborhood>().FindByName("art_pls_to_bon_pls");
         var originalNeighborhoodBuilder = _module1.SpatialStructure.Neighborhoods.FindByName("art_pls_to_bon_pls");
         var changedNeighborhoodBuilder = _module2.SpatialStructure.Neighborhoods.FindByName("art_pls_to_bon_pls");

         simulationNeighborhood.FirstNeighbor.ConsolidatedPath().ShouldBeEqualTo(changedNeighborhoodBuilder.FirstNeighborPath);
         simulationNeighborhood.SecondNeighbor.ConsolidatedPath().ShouldBeEqualTo(changedNeighborhoodBuilder.SecondNeighborPath);

         simulationNeighborhood.FirstNeighbor.ConsolidatedPath().ShouldBeEqualTo(originalNeighborhoodBuilder.SecondNeighborPath);
         simulationNeighborhood.SecondNeighbor.ConsolidatedPath().ShouldBeEqualTo(originalNeighborhoodBuilder.FirstNeighborPath);
      }
   }

   internal class When_extending_a_neighborhood_with_neighbors_that_cannot_be_resolved : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "art_pls_to_bon_pls";
      private readonly ObjectPath _invalidNeighborPath = new ObjectPath(Constants.ORGANISM, "DOES_NOT_EXIST");
      private readonly ObjectPath _validNeighborPath = new ObjectPath(Constants.ORGANISM, Bone, Plasma);
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForExtendMergeBehavior();
         _module2 = configuration.ModuleConfigurations[1].Module;

         //redefine a neighborhood of the first module with a neighbor that cannot be resolved in the merged structure
         var neighborhood = _neighborhoodFactory.CreateBetween(_invalidNeighborPath, _validNeighborPath);
         neighborhood.Name = NEIGHBORHOOD_NAME;
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_fail_the_simulation_construction_with_an_error()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Error.NeighborhoodNeighborsNotFoundInModel(NEIGHBORHOOD_NAME, _invalidNeighborPath.PathAsString, _validNeighborPath.PathAsString, _module2.SpatialStructure.DisplayName))).ShouldNotBeNull();
      }

      [Observation]
      public void should_not_have_removed_the_neighborhood_defined_in_the_first_module()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldNotBeNull();
      }
   }

   internal class When_overwriting_a_neighborhood_with_neighbors_that_cannot_be_resolved : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "art_pls_to_bon_pls";
      private readonly ObjectPath _invalidNeighborPath = new ObjectPath(Constants.ORGANISM, "DOES_NOT_EXIST");
      private readonly ObjectPath _validNeighborPath = new ObjectPath(Constants.ORGANISM, Bone, Plasma);
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForOverrideMergeBehavior();
         _module2 = configuration.ModuleConfigurations[1].Module;

         //redefine a neighborhood of the first module with a neighbor that cannot be resolved in the merged structure
         var neighborhood = _neighborhoodFactory.CreateBetween(_invalidNeighborPath, _validNeighborPath);
         neighborhood.Name = NEIGHBORHOOD_NAME;
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_fail_the_simulation_construction_with_an_error()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Error.NeighborhoodNeighborsNotFoundInModel(NEIGHBORHOOD_NAME, _invalidNeighborPath.PathAsString, _validNeighborPath.PathAsString, _module2.SpatialStructure.DisplayName))).ShouldNotBeNull();
      }

      [Observation]
      public void should_not_have_removed_the_neighborhood_defined_in_the_first_module()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldNotBeNull();
      }
   }

   internal class When_creating_a_new_neighborhood_with_neighbors_that_cannot_be_resolved : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "neighborhood_with_invalid_neighbors";
      private readonly ObjectPath _invalidNeighborPath = new ObjectPath(Constants.ORGANISM, "DOES_NOT_EXIST");
      private readonly ObjectPath _validNeighborPath = new ObjectPath(Constants.ORGANISM, Bone, Plasma);
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForExtendMergeBehavior();
         _module2 = configuration.ModuleConfigurations[1].Module;

         //add a new neighborhood with a neighbor that cannot be resolved in the merged structure
         var neighborhood = _neighborhoodFactory.CreateBetween(_invalidNeighborPath, _validNeighborPath);
         neighborhood.Name = NEIGHBORHOOD_NAME;
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_fail_the_simulation_construction_with_an_error()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Error.NeighborhoodNeighborsNotFoundInModel(NEIGHBORHOOD_NAME, _invalidNeighborPath.PathAsString, _validNeighborPath.PathAsString, _module2.SpatialStructure.DisplayName))).ShouldNotBeNull();
      }

      [Observation]
      public void should_not_have_created_the_neighborhood()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldBeNull();
      }
   }

   internal class When_extending_a_neighborhood_with_a_neighborhood_without_neighbors : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "bon_pls_to_ven_pls";
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForExtendMergeBehavior();
         _module2 = configuration.ModuleConfigurations[1].Module;

         //redefine the neighborhood without neighbors to remove it from the simulation
         var neighborhood = _neighborhoodFactory.Create().WithName(NEIGHBORHOOD_NAME);
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_have_removed_the_neighborhood_defined_in_the_first_module()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldBeNull();
      }

      [Observation]
      public void should_warn_that_the_neighborhood_was_removed_from_the_simulation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Warning.NeighborhoodRemovedFromModel(NEIGHBORHOOD_NAME, _module2.SpatialStructure.DisplayName))).ShouldNotBeNull();
      }
   }

   internal class When_overwriting_a_neighborhood_with_a_neighborhood_without_neighbors : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "bon_pls_to_ven_pls";
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForOverrideMergeBehavior();
         _module2 = configuration.ModuleConfigurations[1].Module;

         //redefine the neighborhood without neighbors to remove it from the simulation
         var neighborhood = _neighborhoodFactory.Create().WithName(NEIGHBORHOOD_NAME);
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_have_removed_the_neighborhood_defined_in_the_first_module()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldBeNull();
      }

      [Observation]
      public void should_warn_that_the_neighborhood_was_removed_from_the_simulation()
      {
         _result.ValidationResult.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings, _result.ValidationResult.Messages.Select(m => m.Text).ToString("\n"));
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Warning.NeighborhoodRemovedFromModel(NEIGHBORHOOD_NAME, _module2.SpatialStructure.DisplayName))).ShouldNotBeNull();
      }
   }

   internal class When_the_first_module_defines_a_neighborhood_without_neighbors : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "neighborhood_without_neighbors";
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module1;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForExtendMergeBehavior();
         _module1 = configuration.ModuleConfigurations[0].Module;

         //there is no neighborhood to remove in the first module. The neighborhood is simply ignored
         var neighborhood = _neighborhoodFactory.Create().WithName(NEIGHBORHOOD_NAME);
         _module1.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_not_fail_the_simulation_construction()
      {
         _result.ValidationResult.ValidationState.ShouldNotBeEqualTo(ValidationState.Invalid);
      }

      [Observation]
      public void should_not_have_created_the_neighborhood()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldBeNull();
      }

      [Observation]
      public void should_not_warn_that_a_neighborhood_was_removed()
      {
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Warning.NeighborhoodRemovedFromModel(NEIGHBORHOOD_NAME, _module1.SpatialStructure.DisplayName))).ShouldBeNull();
      }
   }

   internal class When_a_module_defines_a_neighborhood_without_neighbors_matching_no_existing_neighborhood : concern_for_ModuleIntegration
   {
      private const string NEIGHBORHOOD_NAME = "neighborhood_without_neighbors";
      private INeighborhoodBuilderFactory _neighborhoodFactory;
      private Module _module2;

      protected override void Context()
      {
         base.Context();
         _neighborhoodFactory = IoC.Resolve<INeighborhoodBuilderFactory>();
      }

      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x =>
      {
         var configuration = x.CreateSimulationConfigurationForExtendMergeBehavior();
         _module2 = configuration.ModuleConfigurations[1].Module;

         //no neighborhood with this name exists in the previously merged modules. The neighborhood is simply ignored
         var neighborhood = _neighborhoodFactory.Create().WithName(NEIGHBORHOOD_NAME);
         _module2.SpatialStructure.AddNeighborhood(neighborhood);

         return configuration;
      };

      [Observation]
      public void should_not_fail_the_simulation_construction()
      {
         _result.ValidationResult.ValidationState.ShouldNotBeEqualTo(ValidationState.Invalid);
      }

      [Observation]
      public void should_not_have_created_the_neighborhood()
      {
         _model.Neighborhoods.FindByName(NEIGHBORHOOD_NAME).ShouldBeNull();
      }

      [Observation]
      public void should_not_warn_that_a_neighborhood_was_removed()
      {
         _result.ValidationResult.Messages.FirstOrDefault(x => x.Text.Equals(Warning.NeighborhoodRemovedFromModel(NEIGHBORHOOD_NAME, _module2.SpatialStructure.DisplayName))).ShouldBeNull();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_overwrite_for_eventGroup : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForOverrideMergeBehavior();

      [Observation]
      public void should_have_merged_the_events_with_second_present_only()
      {
         var eventGroup1 = _model.Root.EntityAt<EventGroup>("Organism", "ArterialBlood", "Plasma", "eventGroup1");
         eventGroup1.ShouldNotBeNull();
         eventGroup1.Events.Count().ShouldBeEqualTo(1);
         eventGroup1.Events.First().Name.ShouldBeEqualTo("eventBuilder2");
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_extend_for_two_eventGroup_with_same_name_different_eventNames : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForExtendMergeBehaviorSameEventGroupName();

      [Observation]
      public void should_have_merged_the_events_with_second_present_but_four_events()
      {
         var eventGroup1 = _model.Root.EntityAt<EventGroup>("Organism", "ArterialBlood", "Plasma", "eventGroup1");
         eventGroup1.ShouldNotBeNull();
         eventGroup1.Events.Count().ShouldBeEqualTo(4);
         eventGroup1.Events.FirstOrDefault(x => x.Name == "eventBuilder1").ShouldNotBeNull();
         eventGroup1.Events.FirstOrDefault(x => x.Name == "eventBuilder12").ShouldNotBeNull();
         eventGroup1.Events.FirstOrDefault(x => x.Name == "eventBuilder2").ShouldNotBeNull();
         eventGroup1.Events.FirstOrDefault(x => x.Name == "eventBuilder22").ShouldNotBeNull();
      }
   }

   internal class When_running_the_case_study_for_module_integration_with_merge_behavior_overwrite_for_two_eventGroup_with_same_name_different_eventNames : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder() => x => x.CreateSimulationConfigurationForOverrideMergeBehaviorSameEventGroupName();

      [Observation]
      public void should_have_merged_the_events_with_second_present_but_two_events()
      {
         var eventGroup1 = _model.Root.EntityAt<EventGroup>("Organism", "ArterialBlood", "Plasma", "eventGroup1");
         eventGroup1.ShouldNotBeNull();
         eventGroup1.Events.Count().ShouldBeEqualTo(2);
         eventGroup1.Events.FirstOrDefault(x => x.Name == "eventBuilder2").ShouldNotBeNull();
         eventGroup1.Events.FirstOrDefault(x => x.Name == "eventBuilder22").ShouldNotBeNull();
      }
   }

   internal class When_constructing_a_simulation_spatial_structure_must_contain_event_container : concern_for_ModuleIntegration
   {
      protected override Func<ModuleHelperForSpecs, SimulationConfiguration> SimulationConfigurationBuilder()
      {
         return helper =>
         {
            var objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
            var simulationConfiguration = new SimulationConfiguration
            {
               SimulationSettings = helper.CreateSimulationSettings()
            };

            var module = objectBaseFactory.Create<Module>();
            var spatialStructure = helper.CreateSpatialStructure();
            var organism = helper.CreateOrganism();
            spatialStructure.AddTopContainer(organism);

            module.Add(spatialStructure);

            var moduleConfiguration = new ModuleConfiguration(module);
            simulationConfiguration.AddModuleConfiguration(moduleConfiguration);

            return simulationConfiguration;
         };
      }

      [Observation]
      public void spatial_structure_should_have_events_container_present()
      {
         var spatialStructure = _simulationConfiguration.ModuleConfigurations[0].Module.SpatialStructure;
         spatialStructure.ShouldNotBeNull();
         spatialStructure.TopContainers.FindByName("Events").ShouldNotBeNull();
      }
   }
}