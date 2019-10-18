using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationAlgorithm : ContextSpecification<OptimizationAlgorithmProperties>
   {
      protected override void Context()
      {
         sut = new OptimizationAlgorithmProperties("AA");
      }
   }

   public class when_adding_a_parameter_to_the_algorithm : concern_for_ParameterIdentificationAlgorithm
   {
      private IExtendedProperty _extendedProperty;

      protected override void Context()
      {
         base.Context();
         _extendedProperty = new ExtendedProperty<bool> {Name = "name", Value = true};
      }

      protected override void Because()
      {
         sut.Add(_extendedProperty);
      }

      [Observation]
      public void the_name_and_value_should_be_equal_to_the_added_property()
      {
         sut.First().Name.ShouldBeEqualTo(_extendedProperty.Name);
         sut.First().ValueAsObject.ShouldBeEqualTo(_extendedProperty.ValueAsObject);
      }
   }
}