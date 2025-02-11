using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_InitialConditionsCreator : ContextSpecification<InitialConditionsCreator>
   {
      private IObjectBaseFactory _baseFactory;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _baseFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _baseFactory.Create<InitialConditionsBuildingBlock>()).ReturnsLazily(() => new InitialConditionsBuildingBlock());
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         sut = new InitialConditionsCreator(_baseFactory, new EntityPathResolver(new ObjectPathFactory(new AliasCreator())),
            new IdGenerator(), _cloneManagerForBuildingBlock);
      }
   }

   internal class When_creating_initial_conditions_from_molecule_without_a_formula : concern_for_InitialConditionsCreator
   {
      private MoleculeBuilder _molecule;
      private Container _container;
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _molecule = new MoleculeBuilder().WithName("moleculeName").WithDimension(Constants.Dimension.NO_DIMENSION);
         _molecule.DefaultStartFormula = new ExplicitFormula("y = mx + b");
         _container = new Container
         {
            ContainerType = ContainerType.Organ,
            Mode = ContainerMode.Physical,
            Name = "topContainer"
         };
      }

      protected override void Because()
      {
         _initialCondition = sut.CreateInitialCondition(_container, _molecule);
      }

      [Observation]
      public void the_formula_used_should_be_the_default_start_formula_of_the_builder()
      {
         _initialCondition.Formula.ShouldBeEqualTo(_molecule.DefaultStartFormula);
      }
   }

   internal class When_creating_initial_conditions_from_molecule_with_a_formula : concern_for_InitialConditionsCreator
   {
      private MoleculeBuilder _molecule;
      private Container _container;
      private ExplicitFormula _formula;
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _molecule = new MoleculeBuilder().WithName("moleculeName").WithDimension(Constants.Dimension.NO_DIMENSION);
         _molecule.DefaultStartFormula = new ExplicitFormula("y = mx + b");
         _formula = new ExplicitFormula("y = mx + b");
         _container = new Container
         {
            ContainerType = ContainerType.Organ,
            Mode = ContainerMode.Physical,
            Name = "topContainer"
         };
      }

      protected override void Because()
      {
         _initialCondition = sut.CreateInitialCondition(_container, _molecule, _formula);
      }

      [Observation]
      public void the_formula_used_should_be_the_formula_supplied_to_the_creator()
      {
         _initialCondition.Formula.ShouldBeEqualTo(_formula);
      }
   }

   internal class When_creating_initial_conditions_in_a_container_with_parent_path_at_root : concern_for_InitialConditionsCreator
   {
      private MoleculeBuilder _molecule;
      private Container _container;
      private InitialCondition _initialCondition;

      protected override void Context()
      {
         base.Context();
         _molecule = new MoleculeBuilder().WithName("moleculeName").WithDimension(Constants.Dimension.NO_DIMENSION);
         _container = new Container
         {
            ContainerType = ContainerType.Organ,
            Mode = ContainerMode.Physical,
            Name = "topContainer",
            ParentPath = new ObjectPath("ItGoesSomewhere", "Else")
         };
      }

      protected override void Because()
      {
         _initialCondition = sut.CreateInitialCondition(_container, _molecule);
      }

      [Observation]
      public void the_initial_condition_should_have_root_container_parent_path()
      {
         _initialCondition.Path.Equals(new ObjectPath("ItGoesSomewhere", "Else", "topContainer", "moleculeName")).ShouldBeTrue();
      }
   }

   internal class When_creating_initial_conditions_from_molecule_amount : concern_for_InitialConditionsCreator
   {
      private InitialCondition _initialCondition;
      private MoleculeAmount _moleculeAmount;
      private ObjectPath _amountPath;

      protected override void Context()
      {
         base.Context();
         _moleculeAmount = new MoleculeAmount().WithName("moleculeName").WithDimension(Constants.Dimension.NO_DIMENSION);
         _moleculeAmount.Value = 10;
         _moleculeAmount.ScaleDivisor = 4;
         _amountPath = new ObjectPath("the", "path");
      }

      protected override void Because()
      {
         _initialCondition = sut.CreateInitialCondition(_amountPath, _moleculeAmount);
      }

      [Observation]
      public void the_properties_of_initial_condition_should_be_taken_from_the_molecule_amount()
      {
         _initialCondition.Path.ToString().ShouldBeEqualTo(_amountPath);
         _initialCondition.ScaleDivisor.ShouldBeEqualTo(_moleculeAmount.ScaleDivisor);
         _initialCondition.Value.ShouldBeEqualTo(_moleculeAmount.Value);
         _initialCondition.Dimension.ShouldBeEqualTo(_moleculeAmount.Dimension);
      }
   }

   internal class when_adding_initial_conditions_to_expression_profile : concern_for_InitialConditionsCreator
   {
      private MoleculeBuilder _molecule;
      private Container _container;
      private ExpressionProfileBuildingBlock _expressionProfile;

      protected override void Context()
      {
         base.Context();
         _molecule = new MoleculeBuilder().WithName("moleculeName").WithDimension(Constants.Dimension.NO_DIMENSION);
         _molecule.DefaultStartFormula = new ExplicitFormula("y = mx + b");
         _expressionProfile = new ExpressionProfileBuildingBlock().WithName("moleculeName");
         var topContainer = new Container
         {
            ContainerType = ContainerType.Organ,
            Mode = ContainerMode.Physical,
            Name = "topContainer"
         };

         _container = new Container
         {
            ContainerType = ContainerType.Organ,
            Mode = ContainerMode.Physical,
            Name = "physicalContainer"
         };

         _container.Add(new Container
         {
            ContainerType = ContainerType.Molecule,
            Name = _expressionProfile.MoleculeName,
            Mode = ContainerMode.Logical
         });
         topContainer.Add(_container);

         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_molecule.DefaultStartFormula, _expressionProfile.FormulaCache)).Returns(new ExplicitFormula("y = mx + b"));
      }

      protected override void Because()
      {
         sut.AddToExpressionProfile(_expressionProfile, new[] { _container }, _molecule);
      }

      [Observation]
      public void initial_conditions_should_be_created_for_physical_containers_with_molecules()
      {
         _expressionProfile.InitialConditions.Count.ShouldBeEqualTo(1);
         _expressionProfile.InitialConditions.First().Path.ToString().ShouldBeEqualTo("topContainer|physicalContainer|moleculeName");
         _expressionProfile.InitialConditions.First().Formula.ToString().ShouldBeEqualTo("y = mx + b");
      }

      [Observation]
      public void the_clone_manager_is_used_to_clone_the_formula_and_add_to_the_cache()
      {
         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_molecule.DefaultStartFormula, _expressionProfile.FormulaCache)).MustHaveHappened();
      }
   }

   internal class when_generating_new_start_values_building_block : concern_for_InitialConditionsCreator
   {
      private InitialConditionsBuildingBlock _result;

      protected override void Because()
      {
         _result = sut.CreateFrom(new SpatialStructure(), new List<MoleculeBuilder>());
      }

      [Observation]
      public void the_building_block_should_have_default_name()
      {
         _result.Name.ShouldBeEqualTo(DefaultNames.InitialConditions);
      }

      [Observation]
      public void initial_conditions_building_block_should_contain_no_initial_conditions()
      {
         _result.Count().ShouldBeEqualTo(0);
      }
   }

   internal class when_generating_new_non_empty_initial_conditions_building_block : concern_for_InitialConditionsCreator
   {
      private InitialConditionsBuildingBlock _result;
      private SpatialStructure _spatialStructure;
      private MoleculeBuildingBlock _moleculeBuildingBlock;
      private IFormula _defaultStartFormula;
      private const string _rootContainerString = "RootContainer";
      private const string _childContainerString = "ChildContainer";
      private const string M1 = "M1";
      private const string M2 = "M2";

      protected override void Context()
      {
         base.Context();

         _spatialStructure = new SpatialStructure();
         var root = new Container { Name = _rootContainerString, Mode = ContainerMode.Physical };

         root.Add(new Container { Name = _childContainerString, Mode = ContainerMode.Physical });
         _spatialStructure.AddTopContainer(root);

         _moleculeBuildingBlock = new MoleculeBuildingBlock { Name = "MoleculeBB1" };
         _moleculeBuildingBlock.Add(new MoleculeBuilder { Name = M1, DefaultStartFormula = new ConstantFormula(5.0), Dimension = DimensionFactoryForSpecs.ConcentrationDimension });
         _defaultStartFormula = new ExplicitFormula("M/V");
         _moleculeBuildingBlock.Add(new MoleculeBuilder { Name = M2, DefaultStartFormula = _defaultStartFormula, Dimension = DimensionFactoryForSpecs.ConcentrationDimension });

         A.CallTo(() => _cloneManagerForBuildingBlock.Clone(_defaultStartFormula, A<FormulaCache>._)).Returns(new ExplicitFormula("M/V"));
      }

      protected override void Because()
      {
         _result = sut.CreateFrom(_spatialStructure, _moleculeBuildingBlock.ToList());
      }

      [Observation]
      public void number_of_initial_conditions_must_be_correct()
      {
         _result.Count().ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_have_set_each_initial_condition_as_not_allowing_negative_values()
      {
         _result.Each(initialCondition => initialCondition.NegativeValuesAllowed.ShouldBeFalse());
      }

      [Observation]
      public void explicit_formula_cloned_in_initial_condition()
      {
         _result[new ObjectPath(_rootContainerString, _childContainerString, "M2")].Formula.ToString().ShouldBeEqualTo("M/V");
         _result[new ObjectPath(_rootContainerString, "M2")].Formula.ToString().ShouldBeEqualTo("M/V");
      }

      [Observation]
      public void constant_formula_molecules_have_double_initial_condition()
      {
         _result[new ObjectPath(_rootContainerString, _childContainerString, M1)].Value.ShouldBeEqualTo(5.0);
         _result[new ObjectPath(_rootContainerString, M1)].Value.ShouldBeEqualTo(5.0);

         _result[new ObjectPath(_rootContainerString, _childContainerString, M2)].Value.ShouldBeNull();
         _result[new ObjectPath(_rootContainerString, M2)].Value.ShouldBeNull();
      }

      [Observation]
      public void constant_formula_molecules_have_null_formula_start_value()
      {
         _result[new ObjectPath(_rootContainerString, M1)].Formula.ShouldBeNull();
         _result[new ObjectPath(_rootContainerString, _childContainerString, M1)].Formula.ShouldBeNull();
      }
   }
}