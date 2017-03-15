using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Journal;

namespace OSPSuite.Presentation.Views.Journal
{
   public interface IRelatedItemComparableView : IView<IRelatedItemComparablePresenter>
   {
      void ShowWarning(string warning);
      void BindTo(IEnumerable<ObjectSelectionDTO> allComparables);
      bool RunComparisonEnabled { get; set; }
   }
}