namespace OSPSuite.Core.Chart
{
   public interface IChartManagement
   {
      /// <summary>
      ///    Use the export settings to update the chart view as a kind of preview
      /// </summary>
      bool PreviewSettings { get; set; }

      ChartFontAndSizeSettings FontAndSize { get; }
      bool IncludeOriginData { set; get; }

      ChartSettings ChartSettings { get; }
   }
}