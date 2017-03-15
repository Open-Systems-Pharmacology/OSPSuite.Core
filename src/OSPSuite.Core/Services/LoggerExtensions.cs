using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Services
{
   public static class LoggerExtensions
   {
      public static void AddError(this ILogger importLogger, string message)
      {
         importLogger.AddToLog(message, NotificationType.Error);
      }

      public static void AddInfo(this ILogger importLogger, string message)
      {
         importLogger.AddToLog(message, NotificationType.Info);
      }

      public static void AddWarning(this ILogger importLogger, string message)
      {
         importLogger.AddToLog(message, NotificationType.Warning);
      }

      public static void AddDebug(this ILogger importLogger, string message)
      {
         importLogger.AddToLog(message, NotificationType.Debug);
      }

      public static void AddSeparator(this ILogger importLogger)
      {
         importLogger.AddToLog("-------------------------------------------------------------------------------------------------------------------");
      }

      public static void AddInSeparator(this ILogger importLogger, string message, NotificationType messageStatus = NotificationType.None)
      {
         importLogger.AddSeparator();
         importLogger.AddToLog(message, messageStatus);
         importLogger.AddSeparator();
      }
   }
}