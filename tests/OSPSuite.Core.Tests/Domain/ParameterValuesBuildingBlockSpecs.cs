using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterValuesBuildingBlock : ContextSpecification<ParameterValuesBuildingBlock>
   {
      protected override void Context()
      {
         sut = new ParameterValuesBuildingBlock();
      }
   }

   public class When_setting_a_parameter_value_by_object_path : concern_for_ParameterValuesBuildingBlock
   {
      ParameterValue _psv;

      protected override void Context()
      {
         base.Context();
         _psv = new ParameterValue {Path = new ObjectPath("A, B, C")};
      }

      protected override void Because()
      {
         sut[_psv.Path] = _psv;
      }

      [Observation]
      public void should_be_able_to_retrieve_the_same_parameter_value_by_path()
      {
         sut[_psv.Path].ShouldBeEqualTo(_psv);
      }
   }
}