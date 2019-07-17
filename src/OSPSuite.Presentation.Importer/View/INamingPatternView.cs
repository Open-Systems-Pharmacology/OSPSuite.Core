using System.Collections.Generic;
using OSPSuite.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.View
{
   public interface INamingPatternView : IView<INamingPatternPresenter>
   {
      void UpdateNamingPatterns(List<string> patterns);
      void SetNamingPatternDescriptiveText();
   }
}