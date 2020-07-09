using System.Collections.Generic;
using OSPSuite.Presentation.DeprecatedImporter.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.DeprecatedImporter.View
{
   public interface INamingPatternView : IView<INamingPatternPresenter>
   {
      void UpdateNamingPatterns(List<string> patterns);
      void SetNamingPatternDescriptiveText();
   }
}