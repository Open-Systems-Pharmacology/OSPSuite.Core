using System;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.Core.Services
{
   public class DateTimeFormatter : IFormatter<DateTime>
   {
      private readonly bool _displayTime;

      public DateTimeFormatter(bool displayTime = false)
      {
         _displayTime = displayTime;
      }

      public string Format(DateTime dateTime)
      {
         return dateTime == default ? Captions.NA : dateTime.ToIsoFormat(withTime: _displayTime);
      }
   }
}