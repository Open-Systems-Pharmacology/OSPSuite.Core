using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_MetaDataDTO : ContextSpecification<MetaDataDTO>
   {
      protected override void Context()
      {
         sut = new MetaDataDTO();
      }
   }

   public class When_creating_a_meta_data_dto_with_an_empty_name_or_value : concern_for_MetaDataDTO
   {
      [Observation]
      public void should_not_be_valid()
      {
         sut.Name = string.Empty;
         sut.IsValid().ShouldBeFalse();

         sut.Value = string.Empty;
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_creating_a_meta_data_with_defined_name_and_value : concern_for_MetaDataDTO
   {
      [Observation]
      public void should_be_valid()
      {
         sut.Name = "A";
         sut.Value = "B";
         sut.IsValid().ShouldBeTrue();
      }
   }

   public class When_renaming_a_meta_data_to_a_name_already_used_in_the_data_repository : concern_for_MetaDataDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.Name = "TEST";
         sut.Value = "A value";
         sut.DataRepositories = new List<DataRepository> { new DataRepository() };
         sut.DataRepositories.Each(x => x.ExtendedProperties.Add("TOTO", A.Fake<IExtendedProperty>()));
      }

      [Observation]
      public void should_not_be_valid()
      {
         sut.Validate(x => x.Name, "TOTO").IsEmpty.ShouldBeFalse();
      }
   }

   public class When_editing_the_value_of_a_subject_count_meta_data : concern_for_MetaDataDTO
   {
      protected override void Context()
      {
         base.Context();
         sut.Name = Constants.ObservedData.SUBJECT_COUNT;
      }

      [Observation]
      public void should_be_valid_for_a_whole_number_greater_than_or_equal_to_one()
      {
         sut.Value = "12";
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_not_be_valid_for_a_number_smaller_than_one()
      {
         sut.Value = "0";
         sut.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void should_not_be_valid_for_a_value_that_is_not_a_whole_number()
      {
         sut.Value = "2.5";
         sut.IsValid().ShouldBeFalse();
      }
   }

   public class When_editing_the_value_of_another_meta_data : concern_for_MetaDataDTO
   {
      [Observation]
      public void should_not_check_the_value_as_a_subject_count()
      {
         sut.Name = Constants.ObservedData.STUDY_ID;
         sut.Value = "abc";
         sut.IsValid().ShouldBeTrue();
      }
   }
}