using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using OSPSuite.Core.Domain;

namespace OSPSuite.Infrastructure.Services
{
   public static class LoggingBuilderExtensions
   {
      public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string logFileNameFileFullPath, LogLevel level, bool shared= true,
         string outputTemplate = Core.Domain.Constants.LoggerConstants.DEFAULT_LOG_ENTRY_TEMPLATE)
      {
         return builder.AddFile(new string[] { logFileNameFileFullPath }, level, shared, outputTemplate);
      }

      public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string[] logFileFullPaths, LogLevel level, bool shared = true,
         string outputTemplate = Core.Domain.Constants.LoggerConstants.DEFAULT_LOG_ENTRY_TEMPLATE)
      {
         var serilogLevel = LogEventLevel.Verbose;
         switch (level)
         {
            case LogLevel.Debug:
               serilogLevel = LogEventLevel.Debug;
               break;
            case LogLevel.Information:
               serilogLevel = LogEventLevel.Information;
               break;
            case LogLevel.Warning:
               serilogLevel = LogEventLevel.Warning;
               break;
            case LogLevel.Error:
               serilogLevel = LogEventLevel.Error;
               break;
            case LogLevel.Critical:
               serilogLevel = LogEventLevel.Fatal;
               break;
         }

         var loggerConfiguration = new LoggerConfiguration().MinimumLevel.ControlledBy(new LoggingLevelSwitch { MinimumLevel = serilogLevel });

         foreach (var logFile in logFileFullPaths)
         {
            //this is a workaround to avoid having the date automatically appended to the log file name
            //(eg log_20210921.txt, when input name is simply log.txt).
            //Setting rollOnFileSizeLimit disables that, since the first log is log.txt, the next log_001.txt and so on
            //since we do not expect to ever reach the log limit (in the qualificationRunner fe), this results in one file
            //1073741824 is as far as I understand the maximum possible fileSizeLimitBytes
            loggerConfiguration.WriteTo.File(logFile, fileSizeLimitBytes: 1073741824, rollOnFileSizeLimit: true,
               restrictedToMinimumLevel: serilogLevel,
               shared: shared, outputTemplate: outputTemplate);
         }

         builder.AddSerilog(loggerConfiguration.CreateLogger());
         return builder;
      }

   }
}
