namespace OSPSuite.Core.Import
{
   public class NanSettings
   {
      public enum ActionType
      {
         IgnoreRow,
         Throw
      }
      public string Indicator { get; set; }
      public ActionType Action { get; set; } 
   }
}
