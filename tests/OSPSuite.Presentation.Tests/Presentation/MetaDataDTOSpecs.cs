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
}