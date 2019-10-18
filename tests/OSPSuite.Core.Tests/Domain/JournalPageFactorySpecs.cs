using System;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_JournalPageFactory : ContextSpecification<IJournalPageFactory>
   {
      protected IApplicationConfiguration _applicationConfiguration;

      protected override void Context()
      {
         _applicationConfiguration = A.Fake<IApplicationConfiguration>();
         sut = new JournalPageFactory(_applicationConfiguration);
      }
   }

   public class When_creating_a_new_journal_page : concern_for_JournalPageFactory
   {
      private JournalPage _result;
      private Func<string> _oldUserName;
      private Func<DateTime> _oldUtcNow;
      private DateTime _utcNow;
      private Origin _defaultSource;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _oldUserName = EnvironmentHelper.UserName;
         _oldUtcNow = SystemTime.UtcNow;
         EnvironmentHelper.UserName = () => "TOTO";
         _utcNow = new DateTime(123133123);
         SystemTime.UtcNow = () => _utcNow;
      }

      protected override void Context()
      {
         base.Context();
         _defaultSource = Origins.Matlab;
         A.CallTo(() => _applicationConfiguration.Product).Returns(_defaultSource);
      }

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void should_return_a_transient_journal_page()
      {
         _result.IsTransient.ShouldBeTrue();
      }

      [Observation]
      public void should_set_the_current_user_name()
      {
         _result.CreatedBy.ShouldBeEqualTo("TOTO");
      }

      [Observation]
      public void should_set_the_source_to_the_default_source()
      {
         _result.Origin.ShouldBeEqualTo(_defaultSource);
      }

      [Observation]
      public void should_set_the_current_utc_time()
      {
         _result.CreatedAt.Date.ShouldBeEqualTo(_utcNow.Date);
      }

      [Observation]
      public void should_have_set_a_default_title()
      {
         _result.Title.IsNullOrEmpty().ShouldBeFalse();
      }

      [Observation]
      public void should_create_an_empty_content()
      {
         _result.Content.ShouldNotBeNull();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         EnvironmentHelper.UserName = _oldUserName;
         SystemTime.UtcNow = _oldUtcNow;
      }
   }

}	