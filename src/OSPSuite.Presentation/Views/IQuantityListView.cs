using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IQuantityListView : IView<IQuantityListPresenter>
   {
      /// <summary>
      ///    <see cref="PathElementId"/> of column by which the view should grouped
      /// </summary>
      PathElementId GroupPathElementId { get; set; }

      /// <summary>
      ///    <see cref="PathElementId"/> of column that should be sorted according to the sequence column
      /// </summary>
      PathElementId SortedPathElementId { get; set; }

      IEnumerable<QuantitySelectionDTO> SelectedQuantities { get; }
    

      void SetVisibility(QuantityColumn column, bool visible);
      void SetVisibility(PathElementId pathElementId, bool visible);

      void SetCaption(PathElementId pathElementId, string caption);

      void BindTo(IEnumerable<QuantitySelectionDTO> quantitySelectionDTOs);
   }
}