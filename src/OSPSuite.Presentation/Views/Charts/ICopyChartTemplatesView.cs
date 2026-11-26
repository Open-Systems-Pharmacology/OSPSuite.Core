using System.Collections.Generic;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICopyChartTemplatesView : IModalView<ICopyChartTemplatesPresenter>
   {
      /// <summary>
      ///    Binds the view to the <paramref name="sources" /> from which templates can be copied. The first source is selected
      /// </summary>
      void BindToSources(IReadOnlyList<ChartTemplateSourceDTO> sources);

      /// <summary>
      ///    Binds the view to the <paramref name="templates" /> of the selected source
      /// </summary>
      void BindToTemplates(IReadOnlyList<ChartTemplateSelectionDTO> templates);
   }
}
