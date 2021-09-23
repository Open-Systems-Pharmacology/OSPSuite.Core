
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace OSPSuite.Infrastructure.Services
{
   public static class LoggingBuilderExtensions
   {
      public static ILoggingBuilder AddFile(this ILoggingBuilder builder, string logFileFullPath, LogLevel level)
      {
         LogEventLevel serilogLevel = LogEventLevel.Verbose;
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

         //this is a workaround to avoid having the date automatically appended to the log file name
         //(eg log_20210921.txt, when input name is simply log.txt).
         //Setting rollOnFileSizeLimit disables that, since the first log is log.txt, the next log_001.txt and so on
         //since we do not expect to ever reach the log limit (in the qualificationRunner fe), this results in one file
         //1073741824 is as far as I understand the maximum possible fileSizeLimitBytes
         builder.AddSerilog(
           new LoggerConfiguration()
              .MinimumLevel.ControlledBy(new LoggingLevelSwitch { MinimumLevel = serilogLevel })
              .WriteTo.File( logFileFullPath, fileSizeLimitBytes: 1073741824, rollOnFileSizeLimit: true, restrictedToMinimumLevel: serilogLevel, shared: true
                 , outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext:l} {Level:u}] {Message:l} {NewLine:l} {Exception}")
             .CreateLogger()
         );
         return builder;
      }
   }
}
