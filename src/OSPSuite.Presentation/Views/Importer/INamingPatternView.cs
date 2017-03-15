using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Importer;

namespace OSPSuite.Presentation.Views.Importer
{
   public interface INamingPatternView : IView<INamingPatternPresenter>
   {
      void UpdateNamingPatterns(List<string> patterns);
      void SetNamingPatternDescriptiveText();
   }
}