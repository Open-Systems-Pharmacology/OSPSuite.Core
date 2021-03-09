using System;
using OSPSuite.Core.Import;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface INanPresenter : IPresenter<INanView>
   {
      NanSettings Settings { get; }
      void NewNaNSettings();

      event EventHandler OnNaNSettingsChanged;
   }
}
