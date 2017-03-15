using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public class QuantitySelectionChangedEventArgs : EventArgs
   {
      public IReadOnlyList<QuantitySelectionDTO> QuantitySelections { get; private set; }
      public bool Selected { get; private set; }

      public QuantitySelectionChangedEventArgs(IReadOnlyList<QuantitySelectionDTO> quantitySelections, bool selected)
      {
         QuantitySelections = quantitySelections;
         Selected = selected;
      }
   }

   public class QuantityDoubleClickedEventArgs : EventArgs
   {
      public QuantitySelectionDTO Quantity { get; private set; }

      public QuantityDoubleClickedEventArgs(QuantitySelectionDTO quantity)
      {
         Quantity = quantity;
      }
   }

   public interface IQuantityListPresenter : IPresenter<IQuantityListView>, IQuantityPresenter
   {
      /// <summary>
      ///    Is called whenever the user selects/deselects a quantity
      /// </summary>
      void UpdateSelection(QuantitySelectionDTO quantitySelectionDTO);

      /// <summary>
      ///    Select or deselect the quantity according to the given value
      /// </summary>
      void UpdateSelection(IReadOnlyList<QuantitySelectionDTO> quantitySelections, bool selected);

      void Edit(IEnumerable<IQuantity> quantities);
      void Edit(IEnumerable<QuantitySelectionDTO> quantities);

      QuantitySelectionDTO QuantityDTOByPath(string quantityPath);

      IEnumerable<QuantitySelectionDTO> AllQuantityDTOs { get; }

      void Add(QuantitySelectionDTO quantitySelectionDTO);
      void Remove(QuantitySelectionDTO quantitySelectionDTO);

      event EventHandler<QuantitySelectionChangedEventArgs> SelectionChanged;

      event EventHandler<QuantityDoubleClickedEventArgs> QuantityDoubleClicked;

      IEnumerable<IQuantity> SelectedQuantities { get; }
      IEnumerable<QuantitySelectionDTO> SelectedQuantitiesDTO { get; }
      void QuantityDTODoubleClicked(QuantitySelectionDTO quantitySelectionDTO);

      bool ExpandAllGroups { get; set; }

      /// <summary>
      ///    If sets to true, empty path columns will be hidden automatically. Default is false
      /// </summary>
      bool AutomaticallyHideEmptyColumns { get; set; }

      /// <summary>
      ///    Returns true if the column for the path with element <paramref name="pathElement" /> contains only empty value
      /// </summary>
      bool PathElementIsEmpty(PathElement pathElement);

      /// <summary>
      ///    If <see cref="AutomaticallyHideEmptyColumns" /> is set to true, hide or show the column automatically based on the
      ///    value defined
      ///    in the path columns
      /// </summary>
      void UpdatePathColumnsVisibility();
   }

   public class QuantityListPresenter : AbstractPresenter<IQuantityListView, IQuantityListPresenter>, IQuantityListPresenter, ILatchable
   {
      private readonly IQuantityToQuantitySelectionDTOMapper _quantitySelectionDTOMapper;
      private readonly BindingList<QuantitySelectionDTO> _quantitySelectionDTOList;
      public event EventHandler<QuantitySelectionChangedEventArgs> SelectionChanged = delegate { };
      public event EventHandler<QuantityDoubleClickedEventArgs> QuantityDoubleClicked = delegate { };
      public bool IsLatched { get; set; }
      public bool ExpandAllGroups { get; set; }
      public bool AutomaticallyHideEmptyColumns { get; set; }

      public QuantityListPresenter(IQuantityListView view, IQuantityToQuantitySelectionDTOMapper quantitySelectionDTOMapper) : base(view)
      {
         _quantitySelectionDTOMapper = quantitySelectionDTOMapper;
         _quantitySelectionDTOList = new BindingList<QuantitySelectionDTO>();
         ExpandAllGroups = false;
      }

      public void UpdateSelection(QuantitySelectionDTO quantitySelectionDTO)
      {
         if (IsLatched) return;
         UpdateSelection(new[] {quantitySelectionDTO}, quantitySelectionDTO.Selected);
      }

      public void UpdateSelection(IReadOnlyList<QuantitySelectionDTO> quantitySelections, bool selected)
      {
         this.DoWithinLatch(() =>
         {
            SelectionChanged(this, new QuantitySelectionChangedEventArgs(quantitySelections, selected));
            quantitySelections.Each(x => x.Selected = selected);
         });
      }

      public IEnumerable<IQuantity> SelectedQuantities
      {
         get { return SelectedQuantitiesDTO.Select(x => x.Quantity); }
      }

      public IEnumerable<QuantitySelectionDTO> SelectedQuantitiesDTO => View.SelectedQuantities;

      public void QuantityDTODoubleClicked(QuantitySelectionDTO quantitySelectionDTO)
      {
         if (quantitySelectionDTO == null) return;
         QuantityDoubleClicked(this, new QuantityDoubleClickedEventArgs(quantitySelectionDTO));
      }

      public void Edit(IEnumerable<IQuantity> quantities)
      {
         Edit(quantities.Select((quantity, i) => _quantitySelectionDTOMapper.MapFrom(quantity, i)));
      }

      public void Edit(IEnumerable<QuantitySelectionDTO> quantities)
      {
         _quantitySelectionDTOList.Clear();
         quantities.Each(_quantitySelectionDTOList.Add);
         _view.BindTo(_quantitySelectionDTOList);
         UpdatePathColumnsVisibility();
      }

      public bool PathElementIsEmpty(PathElement pathElement)
      {
         return _quantitySelectionDTOList.HasOnlyEmptyValuesAt(pathElement);
      }

      public void UpdatePathColumnsVisibility()
      {
         if (!AutomaticallyHideEmptyColumns) return;
         EnumHelper.AllValuesFor<PathElement>().Each(updateColumnVisibility);
      }

      private void updateColumnVisibility(PathElement pathElement)
      {
         _view.SetVisibility(pathElement, !PathElementIsEmpty(pathElement));
      }

      public QuantitySelectionDTO QuantityDTOByPath(string quantityPath)
      {
         return _quantitySelectionDTOList.Find(x => string.Equals(x.QuantityPath, quantityPath));
      }

      public IEnumerable<QuantitySelectionDTO> AllQuantityDTOs => _quantitySelectionDTOList;

      public void Add(QuantitySelectionDTO quantitySelectionDTO)
      {
         if(_quantitySelectionDTOList.Contains(quantitySelectionDTO))
            return;

         _quantitySelectionDTOList.Add(quantitySelectionDTO);
      }

      public void Remove(QuantitySelectionDTO quantitySelectionDTO)
      {
         _quantitySelectionDTOList.Remove(quantitySelectionDTO);
      }

      public void GroupBy(PathElement pathElement)
      {
         View.GroupPathElement = pathElement;
      }

      public void SortColumn(PathElement pathElement)
      {
         View.SortedPathElement = pathElement;
      }

      public void Show(PathElement pathElement)
      {
         View.SetVisibility(pathElement, visible: true);
      }

      public void Hide(PathElement pathElement)
      {
         View.SetVisibility(pathElement, visible: false);
      }

      public void SetCaption(PathElement pathElement, string caption)
      {
         View.SetCaption(pathElement, caption);
      }

      public void Show(QuantityColumn column)
      {
         View.SetVisibility(column, visible: true);
      }

      public void Hide(QuantityColumn column)
      {
         View.SetVisibility(column, visible: false);
      }
   }
}