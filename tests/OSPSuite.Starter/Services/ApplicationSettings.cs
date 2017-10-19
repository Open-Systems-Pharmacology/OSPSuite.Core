namespace OSPSuite.Starter.Services
{
   public class ApplicationSettings : Core.ApplicationSettings
   {
      public ApplicationSettings()
      {
         UseWatermark = true;
         WatermarkText = "SUPER DRAFT";
      }
   }
}