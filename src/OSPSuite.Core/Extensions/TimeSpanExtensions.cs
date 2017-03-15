using System;

namespace OSPSuite.Core.Extensions
{
   public static class TimeSpanExtensions
   {
      /// <summary>
      ///    Returns a string displaying the time span in format {m:s:ms} with two digits each
      /// </summary>
      public static string ToDisplay(this TimeSpan timeSpan)
      {
         var display = $"{timeSpan.Minutes:D2}m:{timeSpan.Seconds:D2}s:{timeSpan.Milliseconds:D2}ms";

         //always add hoyrs if days are defined
         if (timeSpan.Hours > 0 || timeSpan.Days > 0)
            display = $"{timeSpan.Hours:D2}h:{display}";

         if (timeSpan.Days > 0)
            display = $"{timeSpan.Days:D2}d:{display}";

         return display;
      }
   }
}