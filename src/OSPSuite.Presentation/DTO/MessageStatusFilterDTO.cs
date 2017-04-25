using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class MessageStatusFilterDTO
   {
      public MessageStatusFilterDTO()
      {
         Info = true;
         Error = true;
         Warning = true;
         Debug = false;
      }

      public bool Info { get; set; }
      public bool Error { get; set; }
      public bool Warning { get; set; }
      public bool Debug { get; set; }

      public NotificationType Status
      {
         get
         {
            var status = NotificationType.None;
            if (Info)
               status |= NotificationType.Info;
            if (Error)
               status |= NotificationType.Error;
            if (Warning)
               status |= NotificationType.Warning;
            if (Debug)
               status |= NotificationType.Debug;

            return status;
         }
      }
   }
}