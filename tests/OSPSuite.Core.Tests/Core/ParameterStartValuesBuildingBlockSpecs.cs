using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core
{
   public abstract class concern_for_ParameterStartValuesBuildingBlock : ContextSpecification<IParameterStartValuesBuildingBlock>
   {
      protected override void Context()
      {
         sut = new ParameterStartValuesBuildingBlock();
      }
   }

   public class When_settting_a_parameter_start_value_by_object_path : concern_for_ParameterStartValuesBuildingBlock
   {
      IParameterStartValue _psv;

      protected override void Context()
      {
         base.Context();
         _psv = new ParameterStartValue {Path = new ObjectPath("A, B, C")};
      }

      protected override void Because()
      {
         sut[_psv.Path] = _psv;
      }

      [Observation]
      public void should_be_able_to_retrieve_the_same_parameter_start_value_by_path()
      {
         sut[_psv.Path].ShouldBeEqualTo(_psv);
      }
   }
}