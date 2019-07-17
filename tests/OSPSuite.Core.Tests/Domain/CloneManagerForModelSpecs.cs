using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CloneManagerForModel : ContextSpecification<ICloneManagerForModel>
   {
      private IObjectBaseFactory _objectBaseFactory;
      protected IContainer _objectToClone;
      protected IContainer _result;
      private IDimensionFactory _dimensionRepository;
      private IDataRepositoryTask _dataRepositoryTask;
      private IModelFinalizer _modelFinalizer;

      protected override void Context()
      {
         _dimensionRepository = A.Fake<IDimensionFactory>();
         A.CallTo(() => _dimensionRepository.NoDimension).Returns(new Dimension());
         _objectBaseFactory = new ObjectBaseFactoryForSpecs(_dimensionRepository);
         _objectToClone = createEntityToClone();
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _modelFinalizer = A.Fake<IModelFinalizer>();
         sut = new CloneManagerForModel(_objectBaseFactory, _dataRepositoryTask, _modelFinalizer);
      }

      protected override void Because()
      {
         _result = sut.Clone(_objectToClone);
      }

      private IContainer createEntityToClone()
      {
         var individual = new Container().WithName("individual");
         var organism = new Container().WithName("organism");
         var organ = new Container().WithName("organ");
         var parameterOrganism = new Parameter().WithName("parameterOrganism");
         var parameterOrgan1 = new Parameter().WithName("parameterOrgan1");
         var parameterOrgan2 = new Parameter().WithName("parameterOrgan2");
         parameterOrganism.Formula = new ConstantFormula(1).WithId("1");
         parameterOrganism.RHSFormula = new ConstantFormula(1).WithId("4");

         //organ parameters have the same formula
         parameterOrgan1.Formula = new ConstantFormula(2).WithId("2");
         parameterOrgan2.Formula = parameterOrgan1.Formula;

         organ.Add(parameterOrgan1);
         organ.Add(parameterOrgan2);
         organism.Add(parameterOrganism);
         organism.Add(organ);
         individual.Add(organism);

         return individual;
      }
   }

   public class When_asked_to_clone_an_object : concern_for_CloneManagerForModel
   {
      [Observation]
      public void should_return_an_entity_of_same_type_as_the_entity_to_clone()
      {
         _result.ShouldBeAnInstanceOf<IContainer>();
      }

      [Observation]
      public void should_have_updated_the_properties_of_the_cloned_object_from_the_source_object()
      {
         _result.Name.ShouldBeEqualTo(_objectToClone.Name);
      }
   }

   public class When_asked_to_clone_an_undefined_object : concern_for_CloneManagerForModel
   {
      [Observation]
      public void should_return_null()
      {
         sut.Clone<IContainer>(null).ShouldBeNull();
      }
   }

   public class When_asked_to_clone_a_container : concern_for_CloneManagerForModel
   {
      [Observation]
      public void should_return_a_container_with_the_same_number_of_chidren()
      {
         _result.Children.Count().ShouldBeEqualTo(_objectToClone.Children.Count());
      }

      [Observation]
      public void the_children_should_have_the_same_type_as_the_orginal_children()
      {
         _result.Children.ElementAt(0).ShouldBeAnInstanceOf<IContainer>();
      }
   }

   public class When_cloning_two_parameters_sharing_the_same_formula : concern_for_CloneManagerForModel
   {
      [Observation]
      public void the_cloned_parameters_should_not_shared_the_same_formula()
      {
         IParameter parameterOrgan1 = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan1"));
         IParameter parameterOrgan2 = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrgan2"));
         parameterOrgan1.Formula.ShouldNotBeNull();
         parameterOrgan2.Formula.ShouldNotBeNull();
         parameterOrgan1.Formula.ShouldNotBeEqualTo(parameterOrgan2.Formula);
      }
   }

   public class When_cloning_a_parameter_containing_a_rhs_formula : concern_for_CloneManagerForModel
   {
      [Observation]
      public void the_cloned_parameters_should_also_have_an_rhs_formula()
      {
         IParameter parameterOrganism = _result.GetAllChildren<IParameter>().First(x => x.Name.Equals("parameterOrganism"));
         parameterOrganism.RHSFormula.ShouldNotBeNull();
      }
   }

   public class When_cloning_a_distributed_parameter_whose_value_was_set_using_a_percentile : concern_for_CloneManagerForModel
   {
      private IDistributedParameter _distributedParameter;
      private IDistributedParameter _cloneDistributedParameter;

      protected override void Context()
      {
         base.Context();
         var pathFactory = new ObjectPathFactory(new AliasCreator());
         _distributedParameter = new DistributedParameter();
         var meanParameter = new Parameter {Name = Constants.Distribution.MEAN}.WithFormula(new ExplicitFormula("0"));
         var stdParameter = new Parameter {Name = Constants.Distribution.DEVIATION}.WithFormula(new ExplicitFormula("1"));
         var percentileParameter = new Parameter {Name = Constants.Distribution.PERCENTILE}.WithFormula(new ExplicitFormula("0.7"));
         _distributedParameter.Add(meanParameter);
         _distributedParameter.Add(stdParameter);
         _distributedParameter.Add(percentileParameter);
         _distributedParameter.Formula = new NormalDistributionFormula();
         _distributedParameter.Formula.AddObjectPath(pathFactory.CreateRelativeFormulaUsablePath(_distributedParameter, meanParameter));
         _distributedParameter.Formula.AddObjectPath(pathFactory.CreateRelativeFormulaUsablePath(_distributedParameter, stdParameter));
      }

      protected override void Because()
      {
         _cloneDistributedParameter = sut.Clone(_distributedParameter);
      }

      [Observation]
      public void should_return_a_parameter_whose_value_is_set_accordingly()
      {
         _cloneDistributedParameter.Percentile.ShouldBeEqualTo(_distributedParameter.Percentile);
         _cloneDistributedParameter.Value.ShouldBeEqualTo(_distributedParameter.Value);
      }
   }
}