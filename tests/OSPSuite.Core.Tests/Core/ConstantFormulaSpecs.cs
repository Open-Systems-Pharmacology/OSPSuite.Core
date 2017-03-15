using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core
{
   public abstract class concern_for_ConstantFormula : ContextSpecification<ConstantFormula>
   {
      protected override void Context()
      {
         sut = new ConstantFormula();
      }
   }
   
   public class When_Setting_The_Value_Property_of_a_constant_Formula  : concern_for_ConstantFormula
   {
      private double _value;

      protected override void Context()
      {
         base.Context();
         _value = 2.3;
      }
      protected override void Because()
      {
         sut.Value = _value;
      }
      [Observation]
      public void should_calculate_value()
      {
         sut.Calculate(A.Fake<IUsingFormula>()).ShouldBeEqualTo(_value);
      }
   }

   
   public class When_constructed_with_a_value : concern_for_ConstantFormula
   {
      private double _value;

      protected override void Context()
      {
         base.Context();
         _value = 2.3;
      }
      protected override void Because()
      {
         sut = new ConstantFormula(_value);
      }
      [Observation]
      public void should_calculate_value()
      {
         sut.Calculate(A.Fake<IUsingFormula>()).ShouldBeEqualTo(_value);
      }
   }
}	