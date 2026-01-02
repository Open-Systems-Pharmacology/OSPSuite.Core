using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using System;
using System.Linq;

namespace OSPSuite.Core
{
   public abstract class concern_for_SpatialStructure : ContextForIntegration<SpatialStructure>
   {
      protected SimulationConfiguration _simulationConfiguration;
      protected SimulationBuilder _simulationBuilder;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulationConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateSimulationConfiguration();
         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
      }

      protected override void Context()
      {
         sut = new SpatialStructure();
      }
   }

   public class When_getting_the_caption_of_a_building_block_without_a_module : concern_for_SpatialStructure
   {
      protected override void Context()
      {
         base.Context();
         sut.Name = "spatialStructure";
      }

      [Observation]
      public void the_display_name_is_only_the_building_block_name()
      {
         sut.DisplayName.ShouldBeEqualTo("spatialStructure");
      }
   }

   public class When_getting_the_caption_of_a_building_block_within_a_module : concern_for_SpatialStructure
   {
      private Module _module;

      protected override void Context()
      {
         base.Context();
         sut.Name = "spatialStructure";
         _module = new Module().WithName("moduleName");
         _module.Add(sut);
      }

      [Observation]
      public void the_display_name_contains_the_module_name_and_the_building_block_name()
      {
         sut.DisplayName.Contains("moduleName").ShouldBeTrue();
         sut.DisplayName.Contains("spatialStructure").ShouldBeTrue();
      }
   }

   internal class When_told_to_update_properties_from_a_source_spatial_structure_with_clone_manager_for_building_blocks : concern_for_SpatialStructure
   {
      private SpatialStructure _sourceSpatialStructure;
      private ICloneManagerForBuildingBlock _cloneManager;
      private const uint _buildingBlockVersion = 5;

      protected override void Context()
      {
         base.Context();
         _sourceSpatialStructure = _simulationBuilder.SpatialStructureAndMergeBehaviors[0].spatialStructure;
         _sourceSpatialStructure.Version = _buildingBlockVersion;
         _cloneManager = IoC.Resolve<ICloneManagerForBuildingBlock>();
      }

      protected override void Because()
      {
         sut = _cloneManager.Clone(_sourceSpatialStructure, new FormulaCache());
      }

      [Observation]
      public void all_neighbors_should_be_cloned()
      {
         sut.Neighborhoods.Each(neighbor => neighbor.FirstNeighborPath.ShouldNotBeNull());
         sut.Neighborhoods.Each(neighbor => neighbor.SecondNeighborPath.ShouldNotBeNull());
      }

      [Observation]
      public void neighbor_paths_should_be_cloned_not_shared()
      {
         var sourceNeighborhood = _sourceSpatialStructure.Neighborhoods.First();
         var clonedNeighborhood = sut.Neighborhoods.First();

         // Check that paths are different instances
         clonedNeighborhood.FirstNeighborPath.ShouldNotBeEqualTo(sourceNeighborhood.FirstNeighborPath);
         clonedNeighborhood.SecondNeighborPath.ShouldNotBeEqualTo(sourceNeighborhood.SecondNeighborPath);

         // Check that path values remain the same
         clonedNeighborhood.FirstNeighborPath.PathAsString.ShouldBeEqualTo(sourceNeighborhood.FirstNeighborPath.PathAsString);
         clonedNeighborhood.SecondNeighborPath.PathAsString.ShouldBeEqualTo(sourceNeighborhood.SecondNeighborPath.PathAsString);
      }

      [Observation]
      public void should_have_set_BuildingVersion()
      {
         sut.Version.ShouldBeEqualTo(_buildingBlockVersion);
      }
   }

   internal class When_merging_reaction_with_parameters_and_formula_reference_in_integration : concern_for_SpatialStructure
   {
      private ModuleConfiguration _moduleConfigurationA;
      private IDimension _amountPerTimeDimension;

      private ReactionBuilder _R3;
      private IParameter _k1;
      private IParameter _k2;
      private IParameter _k3;
      private IFormula _r1Formula;
      private IFormula _r1K3Formula;

      protected override void Context()
      {
         base.Context();
         _moduleConfigurationA = new ModuleConfiguration(new Module());
         var reactionsA = new ReactionBuildingBlock();
         _moduleConfigurationA.Module.Add(reactionsA);
         _moduleConfigurationA.Module.MergeBehavior = MergeBehavior.Extend;

         var helpers = IoC.Resolve<ModelHelperForSpecs>();
         _amountPerTimeDimension = helpers.AmountPerTimeDimension;
         _r1Formula = A.Fake<IFormula>();
         _r1K3Formula = A.Fake<IFormula>();

         _R3 = new ReactionBuilder()
            .WithName("R2")
            .WithKinetic(_r1Formula)
            .WithDimension(_amountPerTimeDimension);

         _k1 = helpers.NewConstantParameter("k1", 22);
         _k1.BuildMode = ParameterBuildMode.Local;
         _R3.AddParameter(_k1);

         _k2 = helpers.NewConstantParameter("k2", 1);
         _k2.BuildMode = ParameterBuildMode.Global;
         _R3.AddParameter(_k2);

         _k3 = helpers.NewConstantParameter("k3", 1).WithFormula(_r1K3Formula);
         _k3.BuildMode = ParameterBuildMode.Global;
         _R3.AddParameter(_k3);

         _R3.AddEduct(new ReactionPartnerBuilder("A", 1));
         _R3.AddProduct(new ReactionPartnerBuilder("C", 1));

         reactionsA.Add(_R3);

         _simulationConfiguration.AddModuleConfiguration(_moduleConfigurationA);

         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _simulationBuilder.PerformMerge(_simulationConfiguration);
      }

      [Observation]
      public void should_preserve_reaction_parameters_and_build_modes()
      {
         var rxn = _simulationBuilder.Reactions.Single(x => x.Name == "R2");

         var p1 = rxn.Parameters.Single(p => p.Name == "k1");
         p1.Value.ShouldBeEqualTo(22);
         p1.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Local);

         var p2 = rxn.Parameters.Single(p => p.Name == "k2");
         p2.Value.ShouldBeEqualTo(1);
         p2.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);

         var p3 = rxn.Parameters.Single(p => p.Name == "k3");
         p3.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);
         p3.Formula.ShouldBeEqualTo(_r1K3Formula);
      }

      [Observation]
      public void should_preserve_reaction_formula_and_dimension()
      {
         var rxn = _simulationBuilder.Reactions.Single(x => x.Name == "R2");
         rxn.Formula.ShouldBeEqualTo(_r1Formula);
         rxn.Dimension.ShouldBeEqualTo(_amountPerTimeDimension);
      }

      [Observation]
      public void should_preserve_reaction_partners()
      {
         var rxn = _simulationBuilder.Reactions.Single(x => x.Name == "R2");
         rxn.EductBy("A").StoichiometricCoefficient.ShouldBeEqualTo(1);
         rxn.ProductBy("C").StoichiometricCoefficient.ShouldBeEqualTo(1);
      }
   }

   internal class When_merging_reaction_with_overwrite_in_integration : concern_for_SpatialStructure
   {
      private ModuleConfiguration _moduleExtend;
      private ModuleConfiguration _moduleOverwrite;
      private IDimension _amountPerTimeDimExtend;
      private IDimension _amountPerTimeDimOverwrite;

      private ReactionBuilder _extendR;
      private ReactionBuilder _overwriteR;
      private IParameter _extendParam;
      private IParameter _overwriteParam;
      private IFormula _extendFormula;
      private IFormula _overwriteFormula;

      protected override void Context()
      {
         base.Context();

         var reactionsExtend = new ReactionBuildingBlock();
         var reactionsOverwrite = new ReactionBuildingBlock();

         _moduleExtend = new ModuleConfiguration(new Module());
         _moduleOverwrite = new ModuleConfiguration(new Module());

         _moduleExtend.Module.Add(reactionsExtend);
         _moduleOverwrite.Module.Add(reactionsOverwrite);

         _moduleExtend.Module.MergeBehavior = MergeBehavior.Extend;
         _moduleOverwrite.Module.MergeBehavior = MergeBehavior.Overwrite;

         var helpers = IoC.Resolve<ModelHelperForSpecs>();
         _amountPerTimeDimExtend = helpers.AmountPerTimeDimension;
         _amountPerTimeDimOverwrite = helpers.AmountPerTimeDimension;

         _extendFormula = A.Fake<IFormula>();
         _overwriteFormula = A.Fake<IFormula>();

         _extendR = new ReactionBuilder()
            .WithName("R2")
            .WithKinetic(_extendFormula)
            .WithDimension(_amountPerTimeDimExtend);
         _extendParam = helpers.NewConstantParameter("k_extend", 10);
         _extendParam.BuildMode = ParameterBuildMode.Local;
         _extendR.AddParameter(_extendParam);
         _extendR.AddModifier("M_extend");
         _extendR.AddEduct(new ReactionPartnerBuilder("A", 1));
         _extendR.AddProduct(new ReactionPartnerBuilder("B", 2));
         reactionsExtend.Add(_extendR);

         _overwriteR = new ReactionBuilder()
            .WithName("R2")
            .WithKinetic(_overwriteFormula)
            .WithDimension(_amountPerTimeDimOverwrite);
         _overwriteParam = helpers.NewConstantParameter("k_overwrite", 99);
         _overwriteParam.BuildMode = ParameterBuildMode.Global;
         _overwriteR.AddParameter(_overwriteParam);
         _overwriteR.AddModifier("M_overwrite");
         _overwriteR.AddEduct(new ReactionPartnerBuilder("X", 3));
         _overwriteR.AddProduct(new ReactionPartnerBuilder("Y", 4));
         reactionsOverwrite.Add(_overwriteR);

         _simulationConfiguration.AddModuleConfiguration(_moduleExtend);
         _simulationConfiguration.AddModuleConfiguration(_moduleOverwrite);

         _simulationBuilder = new SimulationBuilderForSpecs(_simulationConfiguration);
         _simulationBuilder.PerformMerge(_simulationConfiguration);
      }

      [Observation]
      public void should_use_overwrite_reaction_as_final()
      {
         var rxn = _simulationBuilder.Reactions.Single(x => x.Name == "R2");

         rxn.EductBy("X").StoichiometricCoefficient.ShouldBeEqualTo(3);
         rxn.ProductBy("Y").StoichiometricCoefficient.ShouldBeEqualTo(4);

         rxn.EductBy("A").ShouldBeNull();
         rxn.ProductBy("B").ShouldBeNull();

         rxn.Parameters.Any(p => p.Name == "k_extend").ShouldBeFalse();
         var pOver = rxn.Parameters.Single(p => p.Name == "k_overwrite");
         pOver.Value.ShouldBeEqualTo(99);
         pOver.BuildMode.ShouldBeEqualTo(ParameterBuildMode.Global);

         rxn.ModifierNames.ShouldContain("M_overwrite");
         rxn.ModifierNames.ShouldNotContain("M_extend");

         rxn.Formula.ShouldBeEqualTo(_overwriteFormula);
         rxn.Dimension.ShouldBeEqualTo(_amountPerTimeDimOverwrite);
      }
   }
}