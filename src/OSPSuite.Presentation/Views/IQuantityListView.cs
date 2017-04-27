using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IQuantityListView : IView<IQuantityListPresenter>
   {
      /// <summary>
      ///    <see cref="PathElement"/> of column by which the view should grouped
      /// </summary>
      PathElement GroupPathElement { get; set; }

      /// <summary>
      ///    <see cref="PathElement"/> of column that should be sorted according to the sequence column
      /// </summary>
      PathElement SortedPathElement { get; set; }

      IEnumerable<QuantitySelectionDTO> SelectedQuantities { get; }
    

      void SetVisibility(QuantityColumn column, bool visible);
      void SetVisibility(PathElement pathElement, bool visible);

      void SetCaption(PathElement pathElement, string caption);

      void BindTo(IEnumerable<QuantitySelectionDTO> quantitySelectionDTOs);
   }
}