
using Microsoft.Extensions.Logging;
using Serilog;
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
         builder.AddSerilog(
           new LoggerConfiguration()
             .WriteTo.File( logFileFullPath, rollOnFileSizeLimit: true, restrictedToMinimumLevel: serilogLevel, outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff} {SourceContext:l} {Level:u}] {Message:l} {Exception}")
             .CreateLogger()
         );
         return builder;
      }
   }
}
