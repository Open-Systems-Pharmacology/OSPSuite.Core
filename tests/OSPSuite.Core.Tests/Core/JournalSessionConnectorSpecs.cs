using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Journal;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Journal;

namespace OSPSuite.Core
{
   public abstract class concern_for_JournalSessionConnector : ContextSpecification<IJournalSessionConnector>
   {
      protected IJournalSession _journalSession;
      protected IJournalLoader _journalLoader;
      protected IDialogCreator _dialogCreator;
      protected IProjectRetriever _projectRetriever;
      protected string _projectFullPath =  @"c:\toto\project.proj";
      protected override void Context()
      {
         _journalSession = A.Fake<IJournalSession>();
         _journalLoader = A.Fake<IJournalLoader>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _projectRetriever = A.Fake<IProjectRetriever>();
       
         sut = new JournalSessionConnector(_dialogCreator, _journalLoader, _journalSession, _projectRetriever);
      }
   }

   public class When_connecting_to_a_non_existing_journal_and_the_users_decides_to_create_a_new_journal : concern_for_JournalSessionConnector
   {
      private string _filePath;

      protected override void Context()
      {
         base.Context();
         _filePath = "WorkingJournalPath";
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_filePath);
         //no means create
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
         A.CallTo(() => _journalSession.IsOpen).Returns(false);
         A.CallTo(() => _projectRetriever.ProjectFullPath).Returns(_projectFullPath);
      }

      protected override void Because()
      {
         sut.ConnectToJournal(string.Empty);
      }

      [Observation]
      public void should_ask_the_user_to_select_a_new_journal_page_file()
      {
         A.CallTo(() => _dialogCreator.AskForFileToSave(Captions.Journal.CreateJournal, Constants.Filter.JOURNAL_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, @"c:\toto")).MustHaveHappened();
      }

      [Observation]
      public void should_create_a_new_journal()
      {
         A.CallTo(() => _journalSession.Create(_filePath)).MustHaveHappened();
      }
   }

   public class When_selecting_an_existing_journal_and_a_journal_is_already_connected : concern_for_JournalSessionConnector
   {
      private string _filePath;

      protected override void Context()
      {
         base.Context();
         _filePath = @"C:\test\toto.txt";
         A.CallTo(() => _journalSession.CurrentJournalPath).Returns(_filePath);
      }

      protected override void Because()
      {
         sut.SelectJournal();
      }

      [Observation]
      public void should_use_the_name_of_the_journal_as_default_file()
      {
         A.CallTo(() => _dialogCreator.AskForFileToOpen(Captions.Journal.OpenJournal, Constants.Filter.JOURNAL_FILE_FILTER, Constants.DirectoryKey.PROJECT, "toto", @"C:\test")).MustHaveHappened();
      }
   }

   public class When_connecting_to_a_non_existing_journal_and_the_users_decides_to_open_an_existing_journal : concern_for_JournalSessionConnector
   {
      private string _filePath;

      protected override void Context()
      {
         base.Context();
         _filePath = "WorkingJournalPath";
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(_filePath);
         //yes means open
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
         A.CallTo(() => _journalSession.IsOpen).Returns(false);
         A.CallTo(() => _projectRetriever.ProjectFullPath).Returns(_projectFullPath);
      }

      protected override void Because()
      {
         sut.ConnectToJournal(string.Empty);
      }

      [Observation]
      public void should_ask_the_user_to_select_a_journal_page_file_to_open()
      {
         A.CallTo(() => _dialogCreator.AskForFileToOpen(Captions.Journal.OpenJournal, Constants.Filter.JOURNAL_FILE_FILTER, Constants.DirectoryKey.PROJECT, null, @"c:\toto")).MustHaveHappened();
      }

      [Observation]
      public void should_open_the_existing_journal()
      {
         A.CallTo(() => _journalLoader.Load(_filePath, null)).MustHaveHappened();
      }
   }
}	