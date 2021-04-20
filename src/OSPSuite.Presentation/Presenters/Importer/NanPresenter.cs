using System;
using OSPSuite.Core.Import;
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

      public NanSettings Settings { get; set; } = new NanSettings();
      public void NewNaNSettings()
      {
         OnNaNSettingsChanged.Invoke(this, new EventArgs());
      }

      public void FillNaNSettings()
      {
         _view.FillNanSettings(Settings);
      }

      public event EventHandler OnNaNSettingsChanged = delegate { };
   }
}
