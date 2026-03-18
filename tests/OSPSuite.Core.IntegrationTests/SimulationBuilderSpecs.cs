using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core;

internal class concern_for_SimulationBuilder : ContextForIntegration<SimulationBuilder>
{
   protected SimulationConfiguration _simulationConfiguration;
   protected ModelHelperForSpecs _modelHelper;

   protected override void Context()
   {
      _simulationConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateSimulationConfiguration();
      _modelHelper = IoC.Resolve<ModelHelperForSpecs>();
   }
}

internal class When_overwriting_molecules_with_parameters_and_formula_reference : concern_for_SimulationBuilder
{
   private IFormula _m1Formula;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IFormula _m2Formula;

   protected override void Context()
   {
      base.Context();
      _m1Formula = new ExplicitFormula("k1");
      _molecule1 = new MoleculeBuilder()
         .WithName("R1")
         .WithDimension(_modelHelper.AmountPerTimeDimension);
      _molecule1.DefaultStartFormula = _m1Formula;
      _molecule1.AddParameter(_modelHelper.NewConstantParameter("shared_param", 1));
      _molecule1.AddParameter(_modelHelper.NewConstantParameter("m1_only_param", 10));
      _molecule1.IsFloating = true;
      _molecule1.IsXenobiotic = true;
      _molecule1.QuantityType = QuantityType.Drug;
      _molecule1.AddUsedCalculationMethod(new UsedCalculationMethod("SharedCategory", "Method_M1"));
      _molecule1.AddUsedCalculationMethod(new UsedCalculationMethod("M1OnlyCategory", "Method_M1Only"));

      var buildingBlock = new MoleculeBuildingBlock { _molecule1 };
      var module1 = new Module { buildingBlock };

      _m2Formula = new ExplicitFormula("k2");
      _molecule2 = new MoleculeBuilder()
         .WithName("R1")
         .WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2.DefaultStartFormula = _m2Formula;
      _molecule2.AddParameter(_modelHelper.NewConstantParameter("shared_param", 2));
      _molecule2.AddParameter(_modelHelper.NewConstantParameter("m2_only_param", 20));
      _molecule2.IsFloating = false;
      _molecule2.IsXenobiotic = false;
      _molecule2.QuantityType = QuantityType.Enzyme;
      _molecule2.DisplayUnit = Constants.Dimension.NO_DIMENSION.DefaultUnit;
      _molecule2.AddUsedCalculationMethod(new UsedCalculationMethod("SharedCategory", "Method_M2"));
      _molecule2.AddUsedCalculationMethod(new UsedCalculationMethod("M2OnlyCategory", "Method_M2Only"));

      var buildingBlock2 = new MoleculeBuildingBlock { _molecule2 };
      var module2 = new Module { buildingBlock2 };
      module2.MergeBehavior = MergeBehavior.Overwrite;

      _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
      _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2));
      sut = new SimulationBuilderForSpecs(_simulationConfiguration);
   }

   [Observation]
   public void the_formula_is_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.DefaultStartFormula.ShouldBeEqualTo(_m2Formula);
   }

   [Observation]
   public void the_parameters_should_be_only_from_molecule2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      var sharedParam = moleculeBuilder.GetSingleChildByName<Parameter>("shared_param");
      var m1OnlyParam = moleculeBuilder.GetSingleChildByName<Parameter>("m1_only_param");
      var m2OnlyParam = moleculeBuilder.GetSingleChildByName<Parameter>("m2_only_param");

      sharedParam.Value.ShouldBeEqualTo(2);
      m1OnlyParam.ShouldBeNull();
      m2OnlyParam.Value.ShouldBeEqualTo(20);
   }

   [Observation]
   public void the_shared_category_calculation_method_should_be_replaced_by_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      var sharedMethod = moleculeBuilder.UsedCalculationMethods.Single(x => x.Category == "SharedCategory");
      sharedMethod.CalculationMethod.ShouldBeEqualTo("Method_M2");
   }

   [Observation]
   public void the_distinct_category_calculation_methods_should_be_only_from_molecule2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      var m1Only = moleculeBuilder.UsedCalculationMethods.SingleOrDefault(x => x.Category == "M1OnlyCategory");
      m1Only.ShouldBeNull();

      var m2Only = moleculeBuilder.UsedCalculationMethods.Single(x => x.Category == "M2OnlyCategory");
      m2Only.CalculationMethod.ShouldBeEqualTo("Method_M2Only");
   }

   [Observation]
   public void the_dimension_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.Dimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
   }

   [Observation]
   public void the_display_unit_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.DisplayUnit.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION.DefaultUnit);
   }

   [Observation]
   public void the_is_floating_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.IsFloating.ShouldBeFalse();
   }

   [Observation]
   public void the_is_xenobiotic_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.IsXenobiotic.ShouldBeFalse();
   }

   [Observation]
   public void the_quantity_type_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.QuantityType.ShouldBeEqualTo(QuantityType.Enzyme);
   }
}

internal class When_extending_molecules_with_parameters_and_formula_reference : concern_for_SimulationBuilder
{
   private IFormula _m1Formula;
   private MoleculeBuilder _molecule1;
   private MoleculeBuilder _molecule2;
   private IFormula _m2Formula;

   protected override void Context()
   {
      base.Context();
      _m1Formula = new ExplicitFormula("k1");
      _molecule1 = new MoleculeBuilder()
         .WithName("R1")
         .WithDimension(_modelHelper.AmountPerTimeDimension);
      _molecule1.DefaultStartFormula = _m1Formula;
      _molecule1.AddParameter(_modelHelper.NewConstantParameter("shared_param", 1));
      _molecule1.AddParameter(_modelHelper.NewConstantParameter("m1_only_param", 10));
      _molecule1.IsFloating = true;
      _molecule1.IsXenobiotic = true;
      _molecule1.QuantityType = QuantityType.Drug;
      _molecule1.AddUsedCalculationMethod(new UsedCalculationMethod("SharedCategory", "Method_M1"));
      _molecule1.AddUsedCalculationMethod(new UsedCalculationMethod("M1OnlyCategory", "Method_M1Only"));

      var buildingBlock = new MoleculeBuildingBlock { _molecule1 };
      var module1 = new Module { buildingBlock };

      _m2Formula = new ExplicitFormula("k2");
      _molecule2 = new MoleculeBuilder()
         .WithName("R1")
         .WithDimension(Constants.Dimension.NO_DIMENSION);
      _molecule2.DefaultStartFormula = _m2Formula;
      _molecule2.AddParameter(_modelHelper.NewConstantParameter("shared_param", 2));
      _molecule2.AddParameter(_modelHelper.NewConstantParameter("m2_only_param", 20));
      _molecule2.IsFloating = false;
      _molecule2.IsXenobiotic = false;
      _molecule2.QuantityType = QuantityType.Enzyme;
      _molecule2.DisplayUnit = Constants.Dimension.NO_DIMENSION.DefaultUnit;
      _molecule2.AddUsedCalculationMethod(new UsedCalculationMethod("SharedCategory", "Method_M2"));
      _molecule2.AddUsedCalculationMethod(new UsedCalculationMethod("M2OnlyCategory", "Method_M2Only"));

      var buildingBlock2 = new MoleculeBuildingBlock { _molecule2 };
      var module2 = new Module { buildingBlock2 };
      module2.MergeBehavior = MergeBehavior.Extend;

      _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module1));
      _simulationConfiguration.AddModuleConfiguration(new ModuleConfiguration(module2));
      sut = new SimulationBuilderForSpecs(_simulationConfiguration);
   }

   [Observation]
   public void the_formula_is_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.DefaultStartFormula.ShouldBeEqualTo(_m2Formula);
   }

   [Observation]
   public void the_parameters_should_be_extended_in_the_molecule()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      var sharedParam = moleculeBuilder.GetSingleChildByName<Parameter>("shared_param");
      var m1OnlyParam = moleculeBuilder.GetSingleChildByName<Parameter>("m1_only_param");
      var m2OnlyParam = moleculeBuilder.GetSingleChildByName<Parameter>("m2_only_param");

      sharedParam.Value.ShouldBeEqualTo(2);
      m1OnlyParam.Value.ShouldBeEqualTo(10);
      m2OnlyParam.Value.ShouldBeEqualTo(20);
   }

   [Observation]
   public void the_shared_category_calculation_method_should_be_replaced_by_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      var sharedMethod = moleculeBuilder.UsedCalculationMethods.Single(x => x.Category == "SharedCategory");
      sharedMethod.CalculationMethod.ShouldBeEqualTo("Method_M2");
   }

   [Observation]
   public void the_distinct_category_calculation_methods_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.UsedCalculationMethods.SingleOrDefault(x => x.Category == "M1OnlyCategory").ShouldBeNull();

      var m2Only = moleculeBuilder.UsedCalculationMethods.Single(x => x.Category == "M2OnlyCategory");
      m2Only.CalculationMethod.ShouldBeEqualTo("Method_M2Only");
   }

   [Observation]
   public void the_dimension_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.Dimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
   }

   [Observation]
   public void the_display_unit_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.DisplayUnit.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION.DefaultUnit);
   }

   [Observation]
   public void the_is_floating_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.IsFloating.ShouldBeFalse();
   }

   [Observation]
   public void the_is_xenobiotic_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.IsXenobiotic.ShouldBeFalse();
   }

   [Observation]
   public void the_quantity_type_should_be_from_module_2()
   {
      var moleculeBuilder = sut.Molecules.Single(x => x.Name == "R1");
      moleculeBuilder.QuantityType.ShouldBeEqualTo(QuantityType.Enzyme);
   }
}

internal class When_merging_reaction_with_parameters_and_formula_reference_in_integration : concern_for_SimulationBuilder
{
   private ModuleConfiguration _moduleConfigurationA;
   private IDimension _amountPerTimeDimension;

   private ReactionBuilder _r2;
   private IParameter _k1;
   private IParameter _k2;
   private IParameter _k3;
   private IFormula _r1Formula;
   private IFormula _r1K3Formula;

   protected override void Context()
   {
      base.Context();
      var factory = IoC.Resolve<IObjectBaseFactory>();
      _moduleConfigurationA = new ModuleConfiguration(new Module());
      var reactionsA = new ReactionBuildingBlock();
      _moduleConfigurationA.Module.Add(reactionsA);
      _moduleConfigurationA.Module.MergeBehavior = MergeBehavior.Extend; 
      _r1Formula = factory.Create<ExplicitFormula>().WithFormulaString("k1");
      _r1K3Formula = factory.Create<ExplicitFormula>().WithFormulaString("k3");
      var helpers = IoC.Resolve<ModelHelperForSpecs>();
      _amountPerTimeDimension = helpers.AmountPerTimeDimension;
         
      _r2 = new ReactionBuilder()
         .WithName("R2")
         .WithKinetic(_r1Formula)
         .WithDimension(_amountPerTimeDimension);

      _k1 = helpers.NewConstantParameter("k1", 22);
      _k1.BuildMode = ParameterBuildMode.Local;
      _r2.AddParameter(_k1);

      _k2 = helpers.NewConstantParameter("k2", 1);
      _k2.BuildMode = ParameterBuildMode.Global;
      _r2.AddParameter(_k2);

      _k3 = helpers.NewConstantParameter("k3", 1).WithFormula(_r1K3Formula);
      _k3.BuildMode = ParameterBuildMode.Global;
      _r2.AddParameter(_k3);

      _r2.AddEduct(new ReactionPartnerBuilder("A", 1));
      _r2.AddProduct(new ReactionPartnerBuilder("C", 1));

      reactionsA.Add(_r2);

      _simulationConfiguration.AddModuleConfiguration(_moduleConfigurationA);

      sut = new SimulationBuilderForSpecs(_simulationConfiguration);
   }

   [Observation]
   public void should_preserve_reaction_parameters_and_build_modes()
   {
      var rxn = sut.Reactions.Single(x => x.Name == "R2");

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
      var rxn = sut.Reactions.Single(x => x.Name == "R2");
      rxn.Formula.ShouldBeEqualTo(_r1Formula);
      rxn.Dimension.ShouldBeEqualTo(_amountPerTimeDimension);
   }

   [Observation]
   public void should_preserve_reaction_partners()
   {
      var rxn = sut.Reactions.Single(x => x.Name == "R2");
      rxn.EductBy("A").StoichiometricCoefficient.ShouldBeEqualTo(1);
      rxn.ProductBy("C").StoichiometricCoefficient.ShouldBeEqualTo(1);
   }
}

internal class When_merging_reaction_with_overwrite_in_integration : concern_for_SimulationBuilder
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

      sut = new SimulationBuilderForSpecs(_simulationConfiguration);
   }

   [Observation]
   public void should_use_overwrite_reaction_as_final()
   {
      var rxn = sut.Reactions.Single(x => x.Name == "R2");

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