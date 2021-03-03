using System;
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
         sut.AddUnit("KM", 0.01, 0);
         _unitWithSynonym = sut.AddUnit("dm", 0.1, 0);
         _unitWithSynonym.AddUnitSynonym("dm_2");
         _unitWithSynonym.AddUnitSynonym("DM_3");
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

   public class When_checking_if_a_dimension_supports_a_unit : concern_for_Dimension
   {
      [Observation]
      public void should_return_true_if_the_unit_by_name_exists()
      {
         sut.SupportsUnit("KM").ShouldBeTrue();
         sut.SupportsUnit("DM_3").ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_unit_by_name_exists_in_and_the_case_is_ignored()
      {
         sut.SupportsUnit("km", ignoreCase: true).ShouldBeTrue();
         sut.SupportsUnit("dM_3", ignoreCase: true).ShouldBeTrue();
      }
   }

   public class When_finding_a_unit_by_name : concern_for_Dimension
   {
      [Observation]
      public void should_return_the_unit_if_the_unit_by_name_exists()
      {
         sut.FindUnit("KM").ShouldNotBeNull();
         sut.FindUnit("DM_3").ShouldBeEqualTo(_unitWithSynonym);
      }

      [Observation]
      public void should_return_the_unit_if_the_unit_by_name_exists_and_the_case_is_ignored()
      {
         sut.FindUnit("kM", ignoreCase: true).ShouldNotBeNull();
         sut.FindUnit("dM_3", ignoreCase: true).ShouldBeEqualTo(_unitWithSynonym);
      }

      [Observation]
      public void should_return_null_otherwise()
      {
         sut.FindUnit("toto").ShouldBeNull();
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
      public void should_throw_an_exception_if_it_does_not_exist()
      {
         The.Action(() => sut.Unit("toto")).ShouldThrowAn<Exception>();
      }
   }

   public class When_adding_a_unit_when_a_unit_with_the_same_synonym_already_exists : concern_for_Dimension
   {
      [Observation]
      public void should_throw_an_error()
      {
         The.Action(() => sut.AddUnit("dm_2", 1, 0)).ShouldThrowAn<NotUniqueIdException>();
      }
   }
}