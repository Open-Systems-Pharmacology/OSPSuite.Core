using System;
using OSPSuite.Utility.Format;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation.Services
{
   public class TimeSpanFormatter : IFormatter<TimeSpan>
   {
      public string Format(TimeSpan timeSpan)
      {
         return timeSpan.ToDisplay();
      }
   }
}