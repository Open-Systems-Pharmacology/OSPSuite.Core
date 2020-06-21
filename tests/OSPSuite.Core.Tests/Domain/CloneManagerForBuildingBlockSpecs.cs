using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CloneManagerForBuildingBlock : ContextSpecification<ICloneManagerForBuildingBlock>
   {
      private IObjectBaseFactory _objectBaseFactory;
      protected IContainer _objectToClone;
      protected IFormulaCache _formulaCache;
      protected IContainer _result;
      protected IParameter _parameterOrganism;
      private IDimensionFactory _dimensionRepository;
      protected IDataRepositoryTask _dataRepositoryTask;

      protected override void Context()
      {
         _dimensionRepository = A.Fake<IDimensionFactory>();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         A.CallTo(() => _dimensionRepository.NoDimension).Returns(new Dimension());
         _objectBaseFactory = new ObjectBaseFactoryForSpecs(_dimensionRepository);
         _objectToClone = createEntityToClone();
         _formulaCache = new FormulaCache();
         sut = new CloneManagerForBuildingBlock(_objectBaseFactory, _dataRepositoryTask);
      }

      protected override void Because()
      {
         _result = sut.Clone(_objectToClone, _formulaCache);
      }

      private IContainer createEntityToClone()
      {
         var individual = new Container().WithName("individual");
         var organism = new Container().WithName("organism");
         var organ = new Container().WithName("organ");
         _parameterOrganism = new Parameter().WithName("parameterOrganism");
         var parameterOrgan1 = new Parameter().WithName("parameterOrgan1");
         var parameterOrgan2 = new Parameter().WithName("parameterOrgan2");
         _parameterOrganism.Formula = new ExplicitFormula("1").WithId("1");

         //organ parameters have the same formula
         parameterOrgan1.Formula = new ExplicitFormula("2").WithId("2");
         parameterOrgan2.Formula = parameterOrgan1.Formula;

         organ.Add(parameterOrgan1);
         organ.Add(parameterOrgan2);
         organism.Add(_parameterOrganism);
         organism.Add(organ);
         individual.Add(organism);

         return individual;
      }
   }

   public class When_cloning_data_repository : concern_for_CloneManagerForBuildingBlock
   {
      private DataRepository _dataRepository;
      protected override void Context()
      {
         base.Context();
         _dataRepository = A.Fake<DataRepository>();
      }

      protected override void Because()
      {
         sut.Clone(_dataRepository);
      }

      [Observation]
      public void results_in_the_data_repository_task_being_used_to_clone_the_object()
      {
         A.CallTo(() => _dataRepositoryTask.Clone(_dataRepository)).MustHaveHappened();
      }
   }

   public class When_cloning_two_parameters_having_the_same_formula : concern_for_CloneManagerForBuildingBlock
   {
      [Observation]
      public void the_cloned_parameters_should_share_the_same_formula()
      {
         var parameterOrgan1 = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan1"));
         var parameterOrgan2 = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan2"));
         parameterOrgan1.Formula.ShouldNotBeNull();
         parameterOrgan2.Formula.ShouldNotBeNull();
         parameterOrgan1.Formula.ShouldBeEqualTo(parameterOrgan2.Formula);
      }

      [Observation]
      public void should_have_inserted_the_new_formula_into()
      {
         var parameterOrgan1 = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan1"));
         _formulaCache.Contains(parameterOrgan1.Formula.Id).ShouldBeTrue();
      }
   }

   public class When_cloning_a_parameter_with_a_formula_cache_already_containing_the_formula_to_clone : concern_for_CloneManagerForBuildingBlock
   {
      protected override void Context()
      {
         base.Context();
         _formulaCache.Add(_parameterOrganism.Formula);
      }

      [Observation]
      public void should_not_clone_the_formula_but_instead_set_the_original_formula_into_the_cloned_parameter()
      {
         var parameterOrganismClone = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals(_parameterOrganism.Name));
         parameterOrganismClone.Formula.ShouldBeEqualTo(_parameterOrganism.Formula);
      }
   }

   public class When_the_clone_manager_for_building_block_is_called_twice_with_another_formula_cache : concern_for_CloneManagerForBuildingBlock
   {
      private IFormula _firstFormula;

      protected override void Context()
      {
         base.Context();
         //clone once and save the formula reference
         var clone = sut.Clone(_objectToClone, new FormulaCache());
         var parameterOrgan1 = clone.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan1"));
         _firstFormula = parameterOrgan1.Formula;
      }

      [Observation]
      public void should_not_return_references_to_formula_that_were_called_during_the_first_clone_operation()
      {
         var parameterOrgan1 = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan1"));
         _firstFormula.ShouldNotBeEqualTo(parameterOrgan1.Formula);
      }
   }

   public class When_cloning_a_building_block : concern_for_CloneManagerForBuildingBlock
   {
      private ISpatialStructure _buildingBlock;
      private IContainer _container;
      private ISpatialStructure _clone;

      protected override void Context()
      {
         base.Context();
         _container=new Container();
         _buildingBlock= new SpatialStructure();
         _buildingBlock.AddTopContainer(_container);
         _container.Add(_parameterOrganism);
         _buildingBlock.AddFormula(_parameterOrganism.Formula);
      }

      protected override void Because()
      {
         _clone = sut.CloneBuildingBlock(_buildingBlock);
      }

      [Observation]
      public void should_have_added_the_formula_to_the_cache_of_the_resulting_clone()
      {
         _clone.FormulaCache.Count.ShouldBeEqualTo(1);
      }
   }
}