using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Services
{
   public class LogEntry
   {
      public NotificationType MessageStatus { get; private set; }
      public string Message { get; private set; }

      public LogEntry(NotificationType messageStatus, string message)
      {
         MessageStatus = messageStatus;
         Message = message;
      }

      public override string ToString()
      {
         return Display;
      }

      public string Display
      {
         get
         {
            if (MessageStatus == NotificationType.None)
               return Message;

            return string.Format("{0}: {1}", MessageStatus, Message);
         }
      }
   }
}