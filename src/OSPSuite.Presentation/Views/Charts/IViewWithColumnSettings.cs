using System;
using OSPSuite.Presentation.Settings;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IViewWithColumnSettings
   {
      void ApplyAllColumnSettings();
      void ApplyColumnSettings(GridColumnSettings columnSettings);
      void Refresh();
   }
}