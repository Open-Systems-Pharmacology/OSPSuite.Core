namespace OSPSuite.Core.Services
{
   public interface ICanCopyToClipboard
   {
      void CopyToClipboard();
   }

   public interface ICanCopyToClipboardWithWatermark
   {
      void CopyToClipboard(string watermark);
   }
}