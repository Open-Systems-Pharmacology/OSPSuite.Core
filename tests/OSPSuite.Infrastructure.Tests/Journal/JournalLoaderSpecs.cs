using System;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Journal;
using OSPSuite.Infrastructure.Journal.Queries;

namespace OSPSuite.Infrastructure.Journal
{
   public abstract class concern_for_JournalLoader : ContextSpecification<IJournalLoader>
   {
      protected IJournalSession _journalSession;
      protected IDatabaseMediator _databaseMediator;
      protected IJournalDiagramFactory _diagramFactory;

      protected override void Context()
      {
         _databaseMediator = A.Fake<IDatabaseMediator>();
         _journalSession = A.Fake<IJournalSession>();
         _diagramFactory = A.Fake<IJournalDiagramFactory>();
         sut = new JournalLoader(_journalSession, _databaseMediator, _diagramFactory);
      }
   }

   public class When_the_loading_of_a_journal_by_path_is_possible : concern_for_JournalLoader
   {
      private string _fullPath;
      private Core.Journal.Journal _result;
      private JournalPage _wji1;
      private JournalPage _wji2;
      private JournalDiagram _diagram1;
      private JournalDiagram _diagram2;

      protected override void Context()
      {
         base.Context();
         _fullPath = "AA";
         _wji1 = new JournalPage {UpdatedAt = DateTime.Today};
         _wji2 = new JournalPage {UpdatedAt = DateTime.Today + TimeSpan.FromDays(1)};
         _diagram1 = new JournalDiagram();
         _diagram2 = new JournalDiagram();

         A.CallTo(() => _journalSession.TryOpen(_fullPath)).Returns(true);
         A.CallTo(() => _databaseMediator.ExecuteQuery(A<AllJournalPages>._)).Returns(new[] {_wji1, _wji2});
         A.CallTo(() => _databaseMediator.ExecuteQuery(A<AllJournalDiagrams>._)).Returns(new[] {_diagram1, _diagram2});
         A.CallTo(() => _journalSession.CurrentJournalPath).Returns(_fullPath);
      }

      protected override void Because()
      {
         _result = sut.Load(_fullPath);
      }

      [Observation]
      public void should_return_the_loaded_journal()
      {
         _result.ShouldNotBeNull();
         _result.JournalPages.ShouldOnlyContain(_wji1, _wji2);
         _result.Diagrams.ShouldOnlyContain(_diagram1, _diagram2);
      }

      [Observation]
      public void should_have_set_the_full_path_of_the_journal()
      {
         _result.FullPath.ShouldBeEqualTo(_fullPath);
      }

      [Observation]
      public void should_have_set_the_edited_journal_item_to_the_one_last_updated()
      {
         _result.Edited.ShouldBeEqualTo(_wji2);
      }
   }

   public class When_loading_a_journal_by_relative_path : concern_for_JournalLoader
   {
      private string _fullPath;
      private Core.Journal.Journal _result;
      private string _projectFullPath;
      private string _relativePath;

      protected override void Context()
      {
         base.Context();
         _relativePath = @"..\journal.sbj";
         _projectFullPath = @"C:\A\B\C\tata.pksim";
         _fullPath = @"C:\A\B\journal.sbj";
         A.CallTo(() => _journalSession.TryOpen(_fullPath)).Returns(true);
         A.CallTo(() => _journalSession.CurrentJournalPath).Returns(_fullPath);
      }

      protected override void Because()
      {
         _result = sut.Load(_relativePath, _projectFullPath);
      }

      [Observation]
      public void should_have_set_the_full_path_of_the_journal()
      {
         _result.FullPath.ShouldBeEqualTo(_fullPath);
      }
   }

   public class When_loading_a_journal_by_corrupted_relative_path : concern_for_JournalLoader
   {
      private Core.Journal.Journal _result;
      private string _projectFullPath;
      private string _relativePath;

      protected override void Context()
      {
         base.Context();
         _relativePath = @"..\..\..\..\journal.sbj";
         _projectFullPath = @"C:\A\tata.pksim";
         A.CallTo(() => _journalSession.TryOpen(A<string>._)).Returns(false);
      }

      protected override void Because()
      {
         _result = sut.Load(_relativePath, _projectFullPath);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_the_loading_of_a_journal_by_path_is_not_possible : concern_for_JournalLoader
   {
      private string _fullPath;
      private Core.Journal.Journal _result;

      protected override void Context()
      {
         base.Context();
         _fullPath = "AA";
         A.CallTo(() => _journalSession.TryOpen(_fullPath)).Returns(false);
      }

      protected override void Because()
      {
         _result = sut.Load(_fullPath);
      }

      [Observation]
      public void should_return_null()
      {
         _result.ShouldBeNull();
      }
   }

   public class When_loading_a_journal_for_a_brand_new_database_without_diagram : concern_for_JournalLoader
   {
      private string _fullPath;
      private Core.Journal.Journal _result;
      private JournalDiagram _defaultDiagram;

      protected override void Context()
      {
         base.Context();
         _fullPath = "AA";
         _defaultDiagram = new JournalDiagram();
         A.CallTo(() => _diagramFactory.CreateDefault()).Returns(_defaultDiagram);
         A.CallTo(() => _journalSession.TryOpen(_fullPath)).Returns(true);
         A.CallTo(() => _databaseMediator.ExecuteQuery(A<AllJournalDiagrams>._)).Returns(Enumerable.Empty<JournalDiagram>());
      }

      protected override void Because()
      {
         _result = sut.Load(_fullPath);
      }

      [Observation]
      public void should_create_a_default_diagram()
      {
         _result.Diagrams.ShouldContain(_defaultDiagram);
      }
   }
}