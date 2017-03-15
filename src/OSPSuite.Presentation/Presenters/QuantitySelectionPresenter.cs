using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IQuantitySelectionPresenter : IPresenter<IQuantitySelectionView>, IQuantityPresenter
   {
      /// <summary>
      ///    Deselect all selected observed data
      /// </summary>
      void DeselectAll();

      /// <summary>
      ///    Start the selection for all <c>persistable</c> quantities definied in the given container. The
      ///    <paramref name="selectedQuantities" />
      ///    is used to pre select the quantities that should be pre selected
      /// </summary>
      void Edit(IContainer container, IEnumerable<QuantitySelection> selectedQuantities);

      /// <summary>
      ///    Start the selection for all quantities definied. The <paramref name="selectedQuantities" />
      ///    is used to pre select the quantities that should be pre selected
      /// </summary>
      void Edit(IEnumerable<IQuantity> quantities, IEnumerable<QuantitySelection> selectedQuantities);

      /// <summary>
      ///    Start the selection for all <c>persistable</c> quantities definied in the given container.No quantitites pre
      ///    selected
      /// </summary>
      void Edit(IContainer container);

      /// <summary>
      ///    Start the selection for all quantities definied.No quantitites pre selected
      /// </summary>
      void Edit(IEnumerable<IQuantity> quantities);

      /// <summary>
      ///    Returns the quantities that were actually selected
      /// </summary>
      /// <returns></returns>
      IEnumerable<QuantitySelection> SelectedQuantities();

      int NumberOfSelectedQuantities { get; }

      bool HasSelection { get; }
      string Info { get; set; }

      /// <summary>
      ///    Description to be set in the control to explain the use of it
      /// </summary>
      string Description { get; set; }

      /// <summary>
      ///    Specifies whether the groups should be expanded or not. Default is <c>false</c>
      /// </summary>
      bool ExpandAllGroups { get; set; }

      /// <summary>
      ///    If sets to true, empty path columns will be hidden automatically. Default is false
      /// </summary>
      bool AutomaticallyHideEmptyColumns { get; set; }
   }

   public class QuantitySelectionPresenter : AbstractPresenter<IQuantitySelectionView, IQuantitySelectionPresenter>, IQuantitySelectionPresenter
   {
      private readonly IQuantityListPresenter _allQuantityListPresenter;
      private readonly IQuantityListPresenter _selectedQuantityListPresenter;

      public QuantitySelectionPresenter(IQuantitySelectionView view, IQuantityListPresenter allQuantityListPresenter, IQuantityListPresenter selectedQuantityListPresenter)
         : base(view)
      {
         _allQuantityListPresenter = allQuantityListPresenter;
         _selectedQuantityListPresenter = selectedQuantityListPresenter;
         _view.SetQuantityListView(_allQuantityListPresenter.View);
         _view.SetSelectedQuantityListView(_selectedQuantityListPresenter.View);
         _allQuantityListPresenter.SelectionChanged += updateSelection;
         _selectedQuantityListPresenter.SelectionChanged += updateSelection;
         AddSubPresenters(_allQuantityListPresenter, _selectedQuantityListPresenter);
         ExpandAllGroups = false;
         //per default, selection always selected
         selectedQuantityListPresenter.ExpandAllGroups = true;
      }

      public void Edit(IContainer container, IEnumerable<QuantitySelection> selectedQuantities)
      {
         Edit(container.GetAllChildren<IQuantity>().Where(x => x.Persistable), selectedQuantities);
      }

      public void Edit(IContainer container)
      {
         Edit(container, Enumerable.Empty<QuantitySelection>());
      }

      public void Edit(IEnumerable<IQuantity> quantities)
      {
         Edit(quantities, Enumerable.Empty<QuantitySelection>());
      }

      public void Edit(IEnumerable<IQuantity> quantities, IEnumerable<QuantitySelection> selectedQuantities)
      {
         _allQuantityListPresenter.Edit(quantities);

         var selectedDTO = selectedQuantities
            .Select(q => q.Path)
            .Select(path => _allQuantityListPresenter.QuantityDTOByPath(path))
            .Where(dto => dto != null)
            .ToList();

         _selectedQuantityListPresenter.Edit(selectedDTO);
         selectedDTO.Each(dto => dto.Selected = true);
      }

      private void updateSelection(object sender, QuantitySelectionChangedEventArgs e)
      {
         var selectedQuantities = e.QuantitySelections.ToList();

         foreach (var quantitySelectionDTO in selectedQuantities)
         {
            if (e.Selected)
               _selectedQuantityListPresenter.Add(quantitySelectionDTO);
            else
               _selectedQuantityListPresenter.Remove(quantitySelectionDTO);
         }

         refreshView();
      }

      private void refreshView()
      {
         bool atLeastOneMoleculeSelected = NumberOfSelectedQuantities > 0;
         _view.InfoError = atLeastOneMoleculeSelected ? string.Empty : Captions.AtLeastOneQuantityNeedsToBeSelected;
         _view.DeselectAllEnabled = atLeastOneMoleculeSelected;
         _selectedQuantityListPresenter.UpdatePathColumnsVisibility();
         ViewChanged();
      }

      public void DeselectAll()
      {
         //cache locally as UpdateSelection will be triggerd
         var allMolecules = _selectedQuantityListPresenter.AllQuantityDTOs.ToList();
         _selectedQuantityListPresenter.UpdateSelection(allMolecules, selected: false);
      }

      public IEnumerable<QuantitySelection> SelectedQuantities()
      {
         return _selectedQuantityListPresenter.AllQuantityDTOs.Select(x => x.ToQuantitySelection());
      }

      public int NumberOfSelectedQuantities => _selectedQuantityListPresenter.AllQuantityDTOs.Count();

      public bool HasSelection => NumberOfSelectedQuantities > 0;

      public string Info
      {
         get { return _view.Info; }
         set { _view.Info = value; }
      }

      public string Description
      {
         get { return View.Description; }
         set { View.Description = value; }
      }

      public void GroupBy(PathElement pathElement)
      {
         _allQuantityListPresenter.GroupBy(pathElement);
         _selectedQuantityListPresenter.GroupBy(pathElement);
      }

      public bool ExpandAllGroups
      {
         get { return _allQuantityListPresenter.ExpandAllGroups; }
         set { _allQuantityListPresenter.ExpandAllGroups = value; }
      }

      public bool AutomaticallyHideEmptyColumns
      {
         get { return _allQuantityListPresenter.AutomaticallyHideEmptyColumns; }
         set
         {
            _allQuantityListPresenter.AutomaticallyHideEmptyColumns = value;
            _selectedQuantityListPresenter.AutomaticallyHideEmptyColumns = value;
         }
      }

      public void SortColumn(PathElement pathElement)
      {
         _allQuantityListPresenter.SortColumn(pathElement);
         _selectedQuantityListPresenter.SortColumn(pathElement);
      }

      public void Show(PathElement pathElement)
      {
         _allQuantityListPresenter.Show(pathElement);
         _selectedQuantityListPresenter.Show(pathElement);
      }

      public void Hide(PathElement pathElement)
      {
         _allQuantityListPresenter.Hide(pathElement);
         _selectedQuantityListPresenter.Hide(pathElement);
      }

      public void SetCaption(PathElement pathElement, string caption)
      {
         _allQuantityListPresenter.SetCaption(pathElement, caption);
         _selectedQuantityListPresenter.SetCaption(pathElement, caption);
      }

      public void Show(QuantityColumn column)
      {
         _allQuantityListPresenter.Show(column);
         _selectedQuantityListPresenter.Show(column);
      }

      public void Hide(QuantityColumn column)
      {
         _allQuantityListPresenter.Hide(column);
         _selectedQuantityListPresenter.Hide(column);
      }
   }
}