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
            if (Info)
               return NotificationType.Info;

            if (Error)
               return NotificationType.Error;

            if (Warning)
               return NotificationType.Warning;

            if (Debug)
               return NotificationType.Debug;

            return NotificationType.None;
         }
      }
   }
}