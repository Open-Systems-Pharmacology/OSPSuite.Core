namespace OSPSuite.Core.Services
{
   public interface ICanExportCharts
   {
      void ExportToPng();
      void CopyToClipboard();
   }
}