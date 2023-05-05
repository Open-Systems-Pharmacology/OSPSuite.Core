using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterValuesCreator : ContextSpecification<IParameterValuesCreator>
   {
      protected IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         sut = new ParameterValuesCreator(_objectBaseFactory,new ObjectPathFactoryForSpecs(), new IdGenerator());
      }
   }

   public class When_creating_a_parameter_value_for_a_parameter_and_object_path : concern_for_ParameterValuesCreator
   {
      private ObjectPath _objectPath;
      private IParameter _parameter;
      private ParameterValue _psv;

      protected override void Context()
      {
         base.Context();
         _objectPath =new ObjectPath("A", "B", "C");
         _parameter = DomainHelperForSpecs.ConstantParameterWithValue(5).WithDimension(DomainHelperForSpecs.FractionDimensionForSpecs());
      }
      protected override void Because()
      {
         _psv = sut.CreateParameterValue(_objectPath, _parameter);
      }

      [Observation]
      public void should_return_a_parameter_value_using_the_provided_path_as_well_as_the_dimension_and_the_value_of_the_parameter()
      {
         _psv.Path.ToString().ShouldBeEqualTo(_objectPath.ToString());
         _psv.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _psv.Value.ShouldBeEqualTo(_parameter.Value);
      }
   }

}	