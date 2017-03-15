using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Services
{
   public interface ILogger
   {
      void AddToLog(string message, NotificationType messageStatus = NotificationType.None);
   }
}