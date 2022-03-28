using System.Collections.Generic;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ICurveColorGroupingView : IView<ICurveColorGroupingPresenter>
   {
      void SetMetadata(IReadOnlyList<string> metaDataCategories);
   }
}
