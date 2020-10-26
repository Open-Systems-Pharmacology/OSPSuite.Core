using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Presentation.Views.Importer;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public interface INanPresenter : IPresenter<INanView>
   {
      NanSettings Settings { get; }
      
   }
}
