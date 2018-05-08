using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_CreationMetaDataFactory : ContextSpecification<ICreationMetaDataFactory>
   {
      protected IApplicationConfiguration _applicationConfiguration;
      protected Origin _product;
      protected string _version;

      protected override void Context()
      {
         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         sut = new CreationMetaDataFactory(_applicationConfiguration);

         _product = Origins.PKSim;
         _version = "123";
         A.CallTo(() => _applicationConfiguration.Version).Returns(_version);
         A.CallTo(() => _applicationConfiguration.Product).Returns(_product);
         A.CallTo(() => _applicationConfiguration.InternalVersion).Returns(66);
      }
   }

   public class When_creating_a_new_creation_meta_data : concern_for_CreationMetaDataFactory
   {
      private CreationMetaData _creationMetaData;
      private string _currentUser;
      private Func<string> _oldCurrentUser;
      private DateTime _utcNow;
      private Func<DateTime> _oldUtcNow;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _currentUser = "TOTO";
         _oldCurrentUser = EnvironmentHelper.UserName;
         _utcNow = DateTime.UtcNow;
         _oldUtcNow = SystemTime.UtcNow;
         EnvironmentHelper.UserName = () => _currentUser;
         SystemTime.UtcNow = () => _utcNow;
      }

      protected override void Because()
      {
         _creationMetaData = sut.Create();
      }

      [Observation]
      public void should_set_the_created_by_to_the_current_user()
      {
         _creationMetaData.CreatedBy.ShouldBeEqualTo(_currentUser);
      }

      [Observation]
      public void should_set_the_created_at_to_the_current_utc_date()
      {
         _creationMetaData.CreatedAt.Date.ShouldBeEqualTo(_utcNow.Date);
      }

      [Observation]
      public void should_set_the_creation_mode_to_new()
      {
         _creationMetaData.CreationMode.ShouldBeEqualTo(CreationMode.New);
      }

      [Observation]
      public void should_set_the_origin_to_the_product_defined_in_the_configuration()
      {
         _creationMetaData.Origin.ShouldBeEqualTo(_product);
      }

      [Observation]
      public void should_set_the_version_to_the_product_version_defined_in_the_configuration()
      {
         _creationMetaData.Version.ShouldBeEqualTo(_version);
      }

      [Observation]
      public void should_set_the_internal_version_to_the_internal_version_defined_in_the_configuration()
      {
         _creationMetaData.InternalVersion.ShouldBeEqualTo(_applicationConfiguration.InternalVersion);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         EnvironmentHelper.UserName = _oldCurrentUser;
         SystemTime.UtcNow = _oldUtcNow;
      }
   }
}