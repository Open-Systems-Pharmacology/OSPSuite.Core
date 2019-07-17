using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   internal abstract class concern_for_MoleculeStartValuesCreator : ContextSpecification<MoleculeStartValuesCreator>
   {
      private IObjectBaseFactory _baseFactory;
      protected ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;

      protected override void Context()
      {
         base.Context();
         _baseFactory = A.Fake<IObjectBaseFactory>();
         A.CallTo(() => _baseFactory.Create<IMoleculeStartValuesBuildingBlock>()).ReturnsLazily(() => new MoleculeStartValuesBuildingBlock());
         _cloneManagerForBuildingBlock = A.Fake<ICloneManagerForBuildingBlock>();
         sut = new MoleculeStartValuesCreator(_baseFactory, new ObjectPathFactory(new AliasCreator()),
            new IdGenerator(), _cloneManagerForBuildingBlock);
      }
   }

   internal class when_generating_new_start_values_building_block : concern_for_MoleculeStartValuesCreator
   {
      private IMoleculeStartValuesBuildingBlock _result;

      protected override void Because()
      {
         _result = sut.CreateFrom(new SpatialStructure(), new MoleculeBuildingBlock());
      }

      [Observation]
      public void start_values_building_block_should_contain_no_start_values()
      {
         _result.Count().ShouldBeEqualTo(0);
      }
   }

   internal class when_generating_new_non_empty_start_values_building_block : concern_for_MoleculeStartValuesCreator
   {
      private IMoleculeStartValuesBuildingBlock _result;
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
         _result = sut.CreateFrom(_spatialStructure, _moleculeBuildingBlock);
      }

      [Observation]
      public void number_of_start_values_must_be_correct()
      {
         _result.Count().ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_have_set_each_start_values_as_not_allowing_negative_values()
      {
         _result.Each(msv => msv.NegativeValuesAllowed.ShouldBeFalse());
      }

  
      [Observation]
      public void explicit_formula_cloned_in_start_value()
      {
         _result[new ObjectPath(_rootContainerString, _childContainerString, "M2")].Formula.ToString().ShouldBeEqualTo("M/V");
         _result[new ObjectPath(_rootContainerString, "M2")].Formula.ToString().ShouldBeEqualTo("M/V");
      }

      [Observation]
      public void constant_formula_molecules_have_double_start_value()
      {
         _result[new ObjectPath(_rootContainerString, _childContainerString, M1)].StartValue.ShouldBeEqualTo(5.0);
         _result[new ObjectPath(_rootContainerString, M1)].StartValue.ShouldBeEqualTo(5.0);

         _result[new ObjectPath(_rootContainerString, _childContainerString, M2)].StartValue.ShouldBeNull();
         _result[new ObjectPath(_rootContainerString, M2)].StartValue.ShouldBeNull();


      }
      
      [Observation]
      public void constant_formula_molecules_have_null_formula_start_value()
      {
         _result[new ObjectPath(_rootContainerString, M1)].Formula.ShouldBeNull();
         _result[new ObjectPath(_rootContainerString, _childContainerString, M1)].Formula.ShouldBeNull();
      }
   }
}
