using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters
{
   public interface IQuantitySelectionPresenter : IPresenter<IQuantitySelectionView>, IQuantityPresenter
   {
      /// <summary>
      ///    Deselect all selected observed data
      /// </summary>
      void DeselectAll();

      /// <summary>
      ///    Start the selection for all <c>persistable</c> quantities defined in the given container. The
      ///    <paramref name="selectedQuantities" />
      ///    is used to pre select the quantities that should be preselected
      /// </summary>
      void Edit(IContainer container, IEnumerable<QuantitySelection> selectedQuantities);

      /// <summary>
      ///    Start the selection for all quantities defined. The <paramref name="selectedQuantities" />
      ///    is used to pre select the quantities that should be preselected
      /// </summary>
      void Edit(IEnumerable<IQuantity> quantities, IEnumerable<QuantitySelection> selectedQuantities);

      /// <summary>
      ///    Start the selection for all <c>persistable</c> quantities defined in the given container.No quantity is preselected
      /// </summary>
      void Edit(IContainer container);

      /// <summary>
      ///    Start the selection for all quantities defined. No quantity is preselected
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

      /// <summary>
      ///    Hides the simulation column in both lists if true
      /// </summary>
      bool HideSimulationColumn { get; set; }

      /// <summary>
      ///    Clears the current selection and refreshes using any quantities from <paramref name="selections" />
      ///    that are available in the simulation where the paths match
      /// </summary>
      void UpdateSelection(IReadOnlyList<QuantitySelection> selections);
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

      public void UpdateSelection(IReadOnlyList<QuantitySelection> selections)
      {
         DeselectAll();
         selections.Each(select);
         refreshView();
      }

      private void select(QuantitySelection outputSelection)
      {
         var dto = _allQuantityListPresenter.QuantityDTOByPath(outputSelection.Path);

         if (dto == null)
            return;

         _selectedQuantityListPresenter.Add(dto);
         dto.Selected = true;
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
         //cache locally as UpdateSelection will be triggered
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
         get => _view.Info;
         set => _view.Info = value;
      }

      public string Description
      {
         get => View.Description;
         set => View.Description = value;
      }

      public void GroupBy(PathElementId pathElementId)
      {
         _allQuantityListPresenter.GroupBy(pathElementId);
         _selectedQuantityListPresenter.GroupBy(pathElementId);
      }

      public bool ExpandAllGroups
      {
         get => _allQuantityListPresenter.ExpandAllGroups;
         set => _allQuantityListPresenter.ExpandAllGroups = value;
      }

      public bool HideSimulationColumn
      {
         set
         {
            _allQuantityListPresenter.HideSimulationColumn = value;
            _selectedQuantityListPresenter.HideSimulationColumn = value;
         }
         get => _allQuantityListPresenter.HideSimulationColumn;
      }

      public bool AutomaticallyHideEmptyColumns
      {
         get => _allQuantityListPresenter.AutomaticallyHideEmptyColumns;
         set
         {
            _allQuantityListPresenter.AutomaticallyHideEmptyColumns = value;
            _selectedQuantityListPresenter.AutomaticallyHideEmptyColumns = value;
         }
      }

      public void SortColumn(PathElementId pathElementId)
      {
         _allQuantityListPresenter.SortColumn(pathElementId);
         _selectedQuantityListPresenter.SortColumn(pathElementId);
      }

      public void Show(PathElementId pathElementId)
      {
         _allQuantityListPresenter.Show(pathElementId);
         _selectedQuantityListPresenter.Show(pathElementId);
      }

      public void Hide(PathElementId pathElementId)
      {
         _allQuantityListPresenter.Hide(pathElementId);
         _selectedQuantityListPresenter.Hide(pathElementId);
      }

      public void SetCaption(PathElementId pathElementId, string caption)
      {
         _allQuantityListPresenter.SetCaption(pathElementId, caption);
         _selectedQuantityListPresenter.SetCaption(pathElementId, caption);
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