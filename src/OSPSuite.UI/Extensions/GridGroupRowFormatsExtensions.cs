using DevExpress.XtraEditors;
using OSPSuite.Assets;
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
            default:
               return Captions.Chart.GroupRowFormat.GridGroupingRowDefaultFormat; 
         }
      }
   }
}