using FakeItEasy;
using NHibernate;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Serialization.ORM.MetaData;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_ObservedDataMetaData : ContextSpecification<ObservedDataMetaData>
   {
      protected DataRepositoryMetaData _dataRepository;
      protected ISession _session;
      protected DataRepositoryMetaData _newDataRepository;
      protected ObservedDataMetaData _newObservedDataMetaData;

      protected override void Context()
      {
         _session= A.Fake<ISession>();
         _dataRepository = new DataRepositoryMetaData();
         _dataRepository.Content.Data = new byte[]{125, 154};
         _dataRepository.Content.Id = 10;
         sut = new ObservedDataMetaData {DataRepository = _dataRepository};
         _newDataRepository = new DataRepositoryMetaData { Name = "NEW" };
         _newDataRepository.Content.Data = new byte[] { 150, 25 };
         _newObservedDataMetaData = new ObservedDataMetaData { DataRepository = _newDataRepository };

      }
   }

   public class When_updating_an_observed_meta_data_from_another_observed_data_meta_data : concern_for_ObservedDataMetaData
   {

  
      protected override void Because()
      {
         sut.UpdateFrom(_newObservedDataMetaData, _session);
      }

      [Observation]
      public void should_update_the_data_repository()
      {
         sut.DataRepository.Name.ShouldBeEqualTo("NEW");
         sut.DataRepository.Content.Data.ShouldBeEqualTo(_newDataRepository.Content.Data);
      }

      [Observation]
      public void should_not_change_the_data_repository_reference()
      {
         Assert.AreNotSame(sut.DataRepository, _newDataRepository);
      }
   }

   public class When_updating_an_observed_meta_data_that_is_not_initialized_from_another_observed_data_meta_data : concern_for_ObservedDataMetaData
   {
      protected override void Context()
      {
         base.Context();
         sut = new ObservedDataMetaData();
      }

      protected override void Because()
      {
         sut.UpdateFrom(_newObservedDataMetaData, _session);
      }

      [Observation]
      public void should_update_the_data_repository()
      {
         sut.DataRepository.Name.ShouldBeEqualTo("NEW");
         sut.DataRepository.Content.Data.ShouldBeEqualTo(_newDataRepository.Content.Data);
      }

      [Observation]
      public void should_use_the_source_reference_for_data_repository()
      {
         Assert.AreSame(sut.DataRepository, _newDataRepository);
      }
   }

}