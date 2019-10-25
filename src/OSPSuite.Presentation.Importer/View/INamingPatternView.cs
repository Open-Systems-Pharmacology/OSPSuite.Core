using System.Collections.Generic;
using OSPSuite.Presentation.Importer.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Importer.View
{
   public interface INamingPatternView : IView<INamingPatternPresenter>
   {
      void UpdateNamingPatterns(List<string> patterns);
      void SetNamingPatternDescriptiveText();
   }
}