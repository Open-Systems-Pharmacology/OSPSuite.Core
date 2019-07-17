using System;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.Presentation.Formatters
{
   public class TimeSpanFormatter : IFormatter<TimeSpan>
   {
      public string Format(TimeSpan timeSpan)
      {
         return timeSpan.ToDisplay();
      }
   }
}