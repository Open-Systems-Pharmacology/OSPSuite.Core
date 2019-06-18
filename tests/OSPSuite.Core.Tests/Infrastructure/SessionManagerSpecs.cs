using System;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility;
using FakeItEasy;
using NHibernate;
using OSPSuite.Assets;
using OSPSuite.Infrastructure.Services;

namespace OSPSuite.Infrastructure
{
   public abstract class concern_for_session_manager : ContextSpecification<ISessionManager>
   {
      protected ISessionFactoryProvider _sessionFactoryProvider;
      private string _templateFileName;
      protected string _fileNameToCreate;
      protected ISessionFactory _sessionFactory;

      protected override void Context()
      {
         _sessionFactoryProvider = A.Fake<ISessionFactoryProvider>();
         _templateFileName = FileHelper.GenerateTemporaryFileName();
         CreateFile(_templateFileName);
         _fileNameToCreate = FileHelper.GenerateTemporaryFileName();
         _sessionFactory = A.Fake<ISessionFactory>();
         A.CallTo(() => _sessionFactoryProvider.InitalizeSessionFactoryFor(_fileNameToCreate)).Returns(_sessionFactory);
         sut = new SessionManager(_sessionFactoryProvider);
      }

      protected void CreateFile(string file)
      {
         using (var sw = new StreamWriter(file))
         {
            sw.WriteLine("Tralalal");
         }
      }

      public override void Cleanup()
      {
         FileHelper.DeleteFile(_templateFileName);
         FileHelper.DeleteFile(_fileNameToCreate);
      }
   }


   public class When_asked_to_open_a_session_with_a_factory_that_is_not_initialized : concern_for_session_manager
   {
      private bool _exceptionRaised;
      private string _error;

      protected override void Because()
      {
         try
         {
            sut.OpenSession();
         }
         catch (Exception e)
         {
            _exceptionRaised = true;
            _error = e.Message;
         }
      }

      [Observation]
      public void should_throw_an_exception()
      {
         _exceptionRaised.ShouldBeTrue();
         _error.ShouldBeEqualTo(Error.SessionFactoryNotInitialized);
      }
   }


   public class When_creating_a_factory_for_a_given_file_name_that_was_not_already_open : concern_for_session_manager
   {
      protected override void Because()
      {
         sut.CreateFactoryFor(_fileNameToCreate);
      }

      [Observation]
      public void should_open_a_new_session_factory_for_the_given_file()
      {
         A.CallTo(() => _sessionFactoryProvider.InitalizeSessionFactoryFor(_fileNameToCreate)).MustHaveHappened();
      }

   }


   public class When_asked_to_create_a_factory_for_a_project_that_is_already_open : concern_for_session_manager
   {
      protected override void Context()
      {
         base.Context();
         sut.CreateFactoryFor(_fileNameToCreate);
      }

      protected override void Because()
      {
         sut.CreateFactoryFor(_fileNameToCreate);
      }

      [Observation]
      public void should_not_call_the_initalize_session_again()
      {
         A.CallTo(() => _sessionFactoryProvider.InitalizeSessionFactoryFor(_fileNameToCreate)).MustHaveHappened();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_fileNameToCreate);
      }
   }


   public class When_opening_a_factory_when_the_factory_is_already_open_but_with_another_file_name : concern_for_session_manager
   {
      private string _anotherFile;

      protected override void Context()
      {
         base.Context();
         sut.CreateFactoryFor(_fileNameToCreate);
         CreateFile(_fileNameToCreate);
         _anotherFile = FileHelper.GenerateTemporaryFileName();
      }

      protected override void Because()
      {
         sut.CreateFactoryFor(_anotherFile);
      }

      [Observation]
      public void should_close_the_current_factory()
      {
         A.CallTo(() => _sessionFactory.Close()).MustHaveHappened();
      }

      [Observation]
      public void should_use_the_file_already_open_as_source_for_the_new_session_factory()
      {
         FileHelper.AreFilesEqual(_fileNameToCreate, _anotherFile).ShouldBeTrue();
      }

      [Observation]
      public void should_call_the_open_session_for_the_given_file()
      {
         A.CallTo(() => _sessionFactoryProvider.OpenSessionFactoryFor(_anotherFile)).MustHaveHappened();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_fileNameToCreate);
         FileHelper.DeleteFile(_anotherFile);
      }
   }
}
