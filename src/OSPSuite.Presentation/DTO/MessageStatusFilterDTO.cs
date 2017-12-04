using Microsoft.Extensions.Logging;

namespace OSPSuite.Presentation.DTO
{
   public class MessageStatusFilterDTO
   {
      public LogLevel LogLevel { get; set; } = LogLevel.Information;
   }
}