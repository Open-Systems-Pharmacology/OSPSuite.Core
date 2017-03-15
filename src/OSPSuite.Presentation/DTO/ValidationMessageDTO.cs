using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;

namespace OSPSuite.Presentation.DTO
{
   public class ValidationMessageDTO
   {
      public NotificationType Status { get; set; }
      public string Message { get; set; }
      public string ObjectDescription { get; set; }
      public ApplicationIcon Icon { get; set; }
      private readonly List<string> _details = new List<string>();

      public void AddDetails(IEnumerable<string> details)
      {
         _details.AddRange(details);
      }

      public IEnumerable<string> Details => _details;
   }
}