using OSPSuite.Assets;

namespace OSPSuite.Core.Services
{
   public interface IWatermarkStatusChecker
   {
      void CheckWatermarkStatus();
   }

   public class WatermarkStatusChecker : IWatermarkStatusChecker
   {
      private readonly IApplicationSettings _applicationSettings;
      private readonly IDialogCreator _dialogCreator;
      private readonly IApplicationConfiguration _applicationConfiguration;

      public WatermarkStatusChecker(IApplicationSettings applicationSettings, IDialogCreator dialogCreator, IApplicationConfiguration applicationConfiguration)
      {
         _applicationSettings = applicationSettings;
         _dialogCreator = dialogCreator;
         _applicationConfiguration = applicationConfiguration;
      }

      public void CheckWatermarkStatus()
      {
         //Value defined, nothing to do
         if (_applicationSettings.UseWatermark.HasValue)
            return;
         
         var useWatermark = _dialogCreator.MessageBoxYesNo(
            Captions.ShouldWatermarkBeUsedForChartExportToClipboard(_applicationConfiguration.ProductName, _applicationConfiguration.WatermarkOptionLocation),
            defaultButton:ViewResult.No);

         _applicationSettings.UseWatermark = (useWatermark == ViewResult.Yes);
      }
   }
}