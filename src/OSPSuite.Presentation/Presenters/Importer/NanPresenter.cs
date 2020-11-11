using System;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class NanPresenter : AbstractPresenter<INanView, INanPresenter>, INanPresenter
   {
      public NanPresenter
      (
         INanView view
      ) : base(view)
      {
      }

      public event EventHandler<NanSettingsChangedEventArgs> OnNanSettingsChanged = delegate { };
      public NanSettings Settings { get; } = new NanSettings();
      public void NaNSettingsChanged()
      {
         OnNanSettingsChanged.Invoke(this, new NanSettingsChangedEventArgs {SettingsForNan = Settings});
      }
   }
}
