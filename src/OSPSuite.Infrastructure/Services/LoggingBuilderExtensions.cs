
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
             .WriteTo.File( logFileFullPath, fileSizeLimitBytes: 1073741824, rollOnFileSizeLimit: true)
             .CreateLogger()
         );
         return builder;
      }
   }
}
