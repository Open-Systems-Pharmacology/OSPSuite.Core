using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DataRepositoryExtensions : ContextSpecification<List<DataRepository>>
   {
      protected override void Context()
      {
         base.Context();
         sut = new List<DataRepository>();
      }

      public static IExtendedProperty GenerateExtendedProperty(string key, string value)
      {
         return new ExtendedProperty<string> { Name = key, Value = value };
      }
   }

   public class When_retrieving_intersecting_metadata_from_multiple_repositories : concern_for_DataRepositoryExtensions
   {
      private List<IExtendedProperty> _metaDataList;
      private IEnumerable<IExtendedProperty> _result;

      [Observation]
      public void result_must_contain_exactly_intersecting_data()
      {
         _metaDataList.Each(x => _result.First(result => result.Name.Equals(x.Name)).ShouldNotBeNull());
         var names = _result.Select(x => x.Name);

         names.ShouldOnlyContain(new[] { "key1", "key2", "key3", "key4" });
      }

      protected override void Context()
      {
         base.Context();
         var repository1 = new DataRepository();
         var repository2 = new DataRepository();
         var repository3 = new DataRepository();
         _metaDataList = new List<IExtendedProperty>
         {
            GenerateExtendedProperty("key1", "value1"), 
            GenerateExtendedProperty("key2", "value2"), 
            GenerateExtendedProperty("key3", "value3"), 
            GenerateExtendedProperty("key4", "value4")
         };

         _metaDataList.ForEach(x =>
         {
            // Make new extended properties with the same keys to ensure equality test is valid
            repository1.ExtendedProperties.Add(GenerateExtendedProperty(x.Name, x.ValueAsObject.ToString()));
            repository2.ExtendedProperties.Add(GenerateExtendedProperty(x.Name, x.ValueAsObject.ToString()));
            repository3.ExtendedProperties.Add(GenerateExtendedProperty(x.Name, x.ValueAsObject.ToString()));
         });

         repository1.ExtendedProperties.Add(GenerateExtendedProperty("reponame1", "repository1"));
         repository2.ExtendedProperties.Add(GenerateExtendedProperty("reponame2", "repository2"));
         repository3.ExtendedProperties.Add(GenerateExtendedProperty("reponame3", "repository3"));

         sut.Add(repository1);
         sut.Add(repository2);
         sut.Add(repository3);
      }

      protected override void Because()
      {
         _result = sut.IntersectingMetaData();
      }
   }

   public class When_retrieving_intersecting_metadata_with_null_values : concern_for_DataRepositoryExtensions
   {
      private IEnumerable<IExtendedProperty> _result;

      protected override void Context()
      {
         base.Context();
         var repository = new DataRepository();
         repository.ExtendedProperties.Add(GenerateExtendedProperty("key1", null));
         sut.Add(repository);
      }

      protected override void Because()
      {
         _result = sut.IntersectingMetaData();
      }

      [Observation]
      public void should_not_crash()
      {
         _result.ToList().ShouldNotBeNull();
      }
   }

   public class When_retrieving_intersecting_metadata_from_one_repository : concern_for_DataRepositoryExtensions
   {
      private List<IExtendedProperty> _metaDataList;
      private IEnumerable<IExtendedProperty> _result;

      [Observation]
      public void intersection_should_contain_all_metadata_from_repository()
      {
         _result.ContainsAll(_metaDataList);
      }

      protected override void Context()
      {
         base.Context();
         var repository = new DataRepository();
         _metaDataList = new List<IExtendedProperty>
         {
            GenerateExtendedProperty("key1", "value1"), 
            GenerateExtendedProperty("key2", "value2"), 
            GenerateExtendedProperty("key3", "value3"), 
            GenerateExtendedProperty("key4", "value4")
         };

         _metaDataList.ForEach(x => repository.ExtendedProperties.Add(x));
      }

      protected override void Because()
      {
         _result = sut.IntersectingMetaData();
      }
   }

   public class When_retrieving_intersecting_metadata_from_repositories_without_metadata : concern_for_DataRepositoryExtensions
   {
      private IEnumerable<IExtendedProperty> _result;

      [Observation]
      public void data_should_be_the_intersection_of_all_repos()
      {
         _result.ShouldBeEmpty();
      }

      protected override void Because()
      {
         _result = sut.IntersectingMetaData();
      }

      protected override void Context()
      {
         base.Context();
         sut.Add(new DataRepository());
         sut.Add(new DataRepository());
      }
   }

   public class When_checking_if_a_data_repository_is_null : StaticContextSpecification
   {
      [Observation]
      public void should_return_true_if_the_data_repository_is_a_null_reference()
      {
         DataRepository dataRepository = null;
         dataRepository.IsNull().ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_data_repository_is_an_implementation_of_the_null_data_repository()
      {
         DataRepository dataRepository = new NullDataRepository();
         dataRepository.IsNull().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_otherwise()
      {
         DataRepository dataRepository = new DataRepository("tralalal");
         dataRepository.IsNull().ShouldBeFalse();
      }
   }

 
}
namespace OSPSuite.Core.Domain
{
   public class When_validating_a_subject_count : StaticContextSpecification
   {
      [Observation]
      public void should_accept_whole_numbers_greater_than_or_equal_to_one()
      {
         DataRepositoryExtensions.IsValidSubjectCount("1").ShouldBeTrue();
         DataRepositoryExtensions.IsValidSubjectCount("12").ShouldBeTrue();
      }

      [Observation]
      public void should_reject_numbers_smaller_than_one()
      {
         DataRepositoryExtensions.IsValidSubjectCount("0").ShouldBeFalse();
         DataRepositoryExtensions.IsValidSubjectCount("-3").ShouldBeFalse();
      }

      [Observation]
      public void should_reject_values_that_are_not_whole_numbers()
      {
         DataRepositoryExtensions.IsValidSubjectCount("2.5").ShouldBeFalse();
         DataRepositoryExtensions.IsValidSubjectCount("twelve").ShouldBeFalse();
         DataRepositoryExtensions.IsValidSubjectCount(string.Empty).ShouldBeFalse();
         DataRepositoryExtensions.IsValidSubjectCount(null).ShouldBeFalse();
      }
   }

   public class When_setting_and_retrieving_the_subject_count_of_a_data_repository : StaticContextSpecification
   {
      private DataRepository _dataRepository;

      protected override void Context()
      {
         _dataRepository = new DataRepository();
      }

      [Observation]
      public void should_return_null_if_no_subject_count_is_defined()
      {
         _dataRepository.SubjectCount().ShouldBeNull();
      }

      [Observation]
      public void should_store_the_subject_count_as_meta_data()
      {
         _dataRepository.SetSubjectCount(12);
         _dataRepository.ExtendedPropertyValueFor(Constants.ObservedData.SUBJECT_COUNT).ShouldBeEqualTo("12");
         _dataRepository.SubjectCount().ShouldBeEqualTo(12);
      }

      [Observation]
      public void should_overwrite_an_existing_subject_count()
      {
         _dataRepository.SetSubjectCount(12);
         _dataRepository.SetSubjectCount(8);
         _dataRepository.SubjectCount().ShouldBeEqualTo(8);
      }

      [Observation]
      public void should_throw_an_exception_when_setting_a_subject_count_smaller_than_one()
      {
         The.Action(() => _dataRepository.SetSubjectCount(0)).ShouldThrowAn<OSPSuiteException>();
         _dataRepository.ExtendedProperties.Contains(Constants.ObservedData.SUBJECT_COUNT).ShouldBeFalse();
      }

      [Observation]
      public void should_throw_an_exception_when_retrieving_an_invalid_stored_subject_count()
      {
         _dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = Constants.ObservedData.SUBJECT_COUNT, Value = "abc"});
         The.Action(() => _dataRepository.SubjectCount()).ShouldThrowAn<OSPSuiteException>();
      }
   }
}
