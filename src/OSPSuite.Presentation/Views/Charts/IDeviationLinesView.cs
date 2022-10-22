using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IDeviationLinesView : IModalView<IDeviationLinesPresenter>
   { 
      void BindTo(FoldValueDTO foldValue);
   }
}
