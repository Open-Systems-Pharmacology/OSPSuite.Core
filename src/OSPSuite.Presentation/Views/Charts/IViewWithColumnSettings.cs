using System;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IViewWithColumnSettings
   {
      void ApplyAllColumnSettings();
      void Refresh();
      void DoWithoutColumnSettingsUpdateNotification(Action action);
   }
}