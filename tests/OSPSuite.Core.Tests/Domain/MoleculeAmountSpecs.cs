using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MoleculeAmount : ContextSpecification<IMoleculeAmount>
   {
      protected override void Context()
      {
         sut = new MoleculeAmount {Name = "TOTO"};
      }
   }

   public class A_molecule_amount_with_a_scale_factor : concern_for_MoleculeAmount
   {
      [Observation]
      public void strict_bigger_than_zero_should_be_valid()
      {
         sut.ScaleDivisor = 1;
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void smaller_or_equal_to_zeroro_should_be_invalid()
      {
         sut.ScaleDivisor = 0;
         sut.IsValid().ShouldBeFalse();
      }
   }
}