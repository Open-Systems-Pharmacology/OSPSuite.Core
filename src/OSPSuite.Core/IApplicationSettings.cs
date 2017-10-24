using OSPSuite.Core.Domain;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core
{
   /// <summary>
   ///    User independent settings - valid for the installation
   /// </summary>
   public interface IApplicationSettings
   {
      /// <summary>
      ///    Specifies whether the watermak text should be shown in the app.  A value of null specifies that the option was never
      ///    set
      /// </summary>
      bool? UseWatermark { get; set; }

      /// <summary>
      ///    The text displayed when the <c>UseWatermak</c> option is set to <c>true</c>
      /// </summary>
      string WatermarkText { get; set; }

      /// <summary>
      ///    Returns null if the flag UseWatermark is <c>false</c> or if the WatermarkText is empty
      /// </summary>
      string WatermarkTextToUse { get; }
   }

   /// <summary>
   /// Common implementation of ApplicationSettins
   /// </summary>
   public abstract class ApplicationSettings : Notifier, IApplicationSettings
   {
      private bool? _useWatermark;
      private string _watermarkText;

      protected ApplicationSettings()
      {
         _watermarkText = Constants.DEFAULT_WATERMARK_TEXT;
      }

      public bool? UseWatermark
      {
         get => _useWatermark;
         set => SetProperty(ref _useWatermark, value);
      }

      public string WatermarkText
      {
         get => _watermarkText;
         set => SetProperty(ref _watermarkText, value);
      }

      public string WatermarkTextToUse => UseWatermark.GetValueOrDefault(false) ? WatermarkText : null;
   }
}