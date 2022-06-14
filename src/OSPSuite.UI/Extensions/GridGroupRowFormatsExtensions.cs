using System;
using OSPSuite.Core.Chart;

namespace OSPSuite.UI.Extensions
{
   public static class GridGroupRowFormatsExtensions
   {
      public static string GetFormatString(this GridGroupRowFormats formatName)
      {
         switch (formatName)
         {
            case GridGroupRowFormats.HideColumnName:
               return "[#image]{1} {2}";
            case GridGroupRowFormats.Default:
               return "{0}: [#image]{1} {2}";
            default:
               throw new ArgumentOutOfRangeException(nameof(formatName), formatName, null);
         }
      }
   }
}