using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Infrastructure.Import.Core
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
