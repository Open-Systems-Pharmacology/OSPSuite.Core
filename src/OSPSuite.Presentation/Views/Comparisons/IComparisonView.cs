using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Comparisons;

namespace OSPSuite.Presentation.Views.Comparisons
{
   public interface IComparisonView : IView<IComparisonPresenter>
   {
      void BindTo(IEnumerable<DiffItemDTO> diffItemsDTOs);
      string LeftCaption { get; set; }
      string RightCaption { get; set; }
      bool DifferenceTableVisible { get;set; }
      void SetVisibility(PathElement pathElement, bool visible);
      void ClearBinding();
   }
}