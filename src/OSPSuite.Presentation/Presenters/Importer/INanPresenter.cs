using System;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class NanSettingsChangedEventArgs : EventArgs
   {
      public NanSettings SettingsForNan { get; set; }
   }

   public interface INanPresenter : IPresenter<INanView>
   {
      NanSettings Settings { get; }
      void NaNSettingsChanged();

      event EventHandler<NanSettingsChangedEventArgs> OnNanSettingsChanged;
   }
}
