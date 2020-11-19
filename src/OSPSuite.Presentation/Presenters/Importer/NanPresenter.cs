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

      public NanSettings Settings { get; } = new NanSettings();
      public void NewNaNSettings()
      {
         OnNaNSettingsChanged.Invoke(this, new EventArgs());
      }

      public event EventHandler OnNaNSettingsChanged = delegate { };
   }
}
