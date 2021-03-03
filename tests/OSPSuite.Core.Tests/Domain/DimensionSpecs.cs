using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Dimension : ContextSpecification<IDimension>
   {
      protected Unit _unitWithSynonym;

      protected override void Context()
      {
         sut = new Dimension(new BaseDimensionRepresentation(), "DimForTest", "m");
         sut.AddUnit("cm", 0.01, 0);
         _unitWithSynonym = sut.AddUnit("dm", 0.1, 0);
         _unitWithSynonym.AddUnitSynonym("dm_2");
      }
   }

   public class When_checking_if_a_dimension_has_a_unit_by_name : concern_for_Dimension
   {
      [Observation]
      public void should_return_true_if_the_unit_by_name_exists()
      {
         sut.HasUnit("m").ShouldBeTrue();
         sut.HasUnit("cm").ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_unit_by_synonym_exists()
      {
         sut.HasUnit("dm_2").ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         sut.HasUnit("toto").ShouldBeFalse();
      }
   }

   public class When_retrieving_a_unit_by_name : concern_for_Dimension
   {
      [Observation]
      public void should_return_the_unit_by_name_if_it_exists()
      {
         sut.Unit("m").ShouldNotBeNull();
         sut.Unit("cm").ShouldNotBeNull();
         sut.Unit("dm").ShouldBeEqualTo(_unitWithSynonym);
      }

      [Observation]
      public void should_return_the_unit_by_with_a_synonyme_if_it_exists()
      {
         sut.Unit("dm_2").ShouldBeEqualTo(_unitWithSynonym);
      }

      [Observation]
      public void should_return_null_if_it_does_not_exist()
      {
         sut.Unit("toto").ShouldBeNull();
      }
   }
}