using System;
using System.IO;
using NHibernate;
using OSPSuite.Assets;
using OSPSuite.Utility;

namespace OSPSuite.Infrastructure.Services
{
   public interface ISessionManager
   {
      /// <summary>
      /// Create a session factory for the project file located at <para>fileName</para>
      /// </summary>
      /// <param name="fileName">Full path of the file for which a session should be created.
      /// If the path does not exist a new database is created with the accurate database schema</param>
      void CreateFactoryFor(string fileName);

      /// <summary>
      /// Open a session factory for the project file located at <para>fileName</para>
      /// </summary>
      /// <param name="fileName">Full path of the file for which a session should be opened.
      /// If the file does not exist, an exception is thrown</param>
      void OpenFactoryFor(string fileName);

      /// <summary>
      /// Close the current factory if open. Do nothing otherwise
      /// </summary>
      void CloseFactory();

      /// <summary>
      /// Open a session for the current session factory.
      /// </summary>
      /// <returns>a session</returns>
      /// <exception cref="InvalidOperationException">Is thrown if the session if the session factory has not been created.</exception>
      ISession OpenSession();


      /// <summary>
      /// Return the session that was last opened with OpenSession.
      /// </summary>
      /// <exception cref="InvalidOperationException">Is thrown if the session is not opened.</exception>
      ISession CurrentSession { get; }

      /// <summary>
      /// Returns whether a project is available or not
      /// </summary>
      bool IsOpen { get; }
   }

   public class SessionManager : ISessionManager
   {
      private readonly ISessionFactoryProvider _sessionFactoryProvider;
      private string _currentFileName;
      private ISessionFactory _sessionFactory;
      private ISession _session;

      public SessionManager(ISessionFactoryProvider sessionFactoryProvider)
      {
         _sessionFactoryProvider = sessionFactoryProvider;
      }

      public void CloseFactory()
      {
         if (!sessionFactoryIsOpen()) return;
         _sessionFactory.Close();
         _sessionFactory = null;
         _session = null;
         _currentFileName = string.Empty;
      }

      public void CreateFactoryFor(string fileName)
      {
         //Project file is already open for the same filename
         if (sessionFactoryIsAlreadyOpenFor(fileName)) return;

         //Project file open, but for another name => save as
         if (sessionFactoryIsOpen())
         {
            File.Copy(_currentFileName, fileName, true);
            CloseFactory();
            _sessionFactory = _sessionFactoryProvider.OpenSessionFactoryFor(fileName);
         }
         //new project from scratch. save file
         else
         {
            FileHelper.DeleteFile(fileName);
            _sessionFactory = _sessionFactoryProvider.InitalizeSessionFactoryFor(fileName);
         }

         _currentFileName = fileName;
      }

      public void OpenFactoryFor(string fileName)
      {
         if (sessionFactoryIsAlreadyOpenFor(fileName)) return;
         if (sessionFactoryIsOpen())
            CloseFactory();

         _sessionFactory = _sessionFactoryProvider.OpenSessionFactoryFor(fileName);
         _currentFileName = fileName;
      }

      public ISession OpenSession()
      {
         if (!sessionFactoryIsOpen())
            throw new InvalidOperationException(Error.SessionFactoryNotInitialized);

         _session = _sessionFactory.OpenSession();
         return _session;
      }


      public ISession CurrentSession
      {
         get
         {
            if (_session == null)
               throw new InvalidOperationException(Error.SessionNotInitialized);
            if (!_session.IsOpen)
               throw new InvalidOperationException(Error.SessionDisposed);

            return _session;
         }
      }

      public bool IsOpen
      {
         get { return sessionFactoryIsOpen(); }
      }

      private bool sessionFactoryIsOpen()
      {
         return _sessionFactory != null && _sessionFactory.IsClosed == false;
      }

      private bool sessionFactoryIsAlreadyOpenFor(string fileName)
      {
         return string.Equals(_currentFileName, fileName)
                && sessionFactoryIsOpen();
      }
   }
}
