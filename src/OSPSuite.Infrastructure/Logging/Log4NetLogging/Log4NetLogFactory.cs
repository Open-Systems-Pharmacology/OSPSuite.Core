using System;
using System.IO;
using System.Linq;
using log4net;
using log4net.Appender;
using log4net.Config;
using OSPSuite.Utility.Logging;

namespace OSPSuite.Infrastructure.Logging.Log4NetLogging
{
   public class Log4NetLogFactory : ILogFactory
   {
      public ILogger CreateFor(Type typeThatWantsToLog)
      {
         return new Log4NetLogger(LogManager.GetLogger(typeThatWantsToLog));
      }

      /// <summary>
      /// This function configure the log4net factory with the default config file located in "app.path\log4net.config.xml" and should only be called once
      /// </summary>
      public void Configure()
      {
         Configure(defaultPathToConfigFile());
      }

      /// <summary>
      /// This function configure the log4net factory with the given config file and should only be called once
      /// </summary>
      public void Configure(FileInfo pathToConfigFile)
      {
         XmlConfigurator.Configure(pathToConfigFile);
      }

      /// <summary>
      /// Read the default location in the configuration for all FileAppenders and update
      /// the location of the corresponding log file at runtime. This is usefull when the application is installed
      /// in a standard location where a user does not have write access.
      /// This function should be called after configure
      /// </summary>
      /// <example>
      /// If the config file as a FileAppender with a path set to logs\log.txt calling this function
      /// with <paramref name="logDirectory"/> set to c:\temp would create the log in c:\temp\logs\log.txt
      /// </example>
      /// <param name="logDirectory">Directory where the logfile should be created </param>
      public void UpdateLogFileLocation(string logDirectory)
      {
         //get the current logging repository for this application
         var repository = LogManager.GetRepository();

         foreach (var fileAppender in repository.GetAppenders().OfType<FileAppender>())
         {
            //set the path to your logDirectory using the original file name defined in configuration
            var fileInConfiguration = Path.GetFileName(fileAppender.File);
            if(!string.IsNullOrEmpty(fileInConfiguration))
               fileAppender.File = Path.Combine(logDirectory, fileInConfiguration);

            //make sure to call fileAppender.ActivateOptions() to notify the logging
            //sub system that the configuration for this appender has changed.
            fileAppender.ActivateOptions();
         } 
      }

      private FileInfo defaultPathToConfigFile()
      {
         return new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config.xml"));
      }
   }
}