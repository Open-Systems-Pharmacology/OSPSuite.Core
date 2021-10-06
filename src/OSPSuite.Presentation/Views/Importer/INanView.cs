using OSPSuite.Core.Import;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface INanView : IView<INanPresenter>
   {
      void FillNanSettings(NanSettings settings);
   }
}
