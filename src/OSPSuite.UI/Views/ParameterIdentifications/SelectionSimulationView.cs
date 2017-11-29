using System.Collections.Generic;
using System.Linq;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class SelectionSimulationView : BaseModalView, ISelectionSimulationView
   {
      private ISelectionSimulationPresenter _presenter;
      private readonly GridViewBinder<SimulationSelectionDTO> _gridViewBinder;
      private readonly UxRepositoryItemCheckEdit _selectEditor;
      private IEnumerable<SimulationSelectionDTO> _dtos;

      public SelectionSimulationView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<SimulationSelectionDTO>(gridView);
         _selectEditor = new UxRepositoryItemCheckEdit(gridView);
         gridView.AllowsFiltering = false;
         gridView.ShowRowIndicator = false;
         gridView.ShowColumnHeaders = false;
      }

      public void AttachPresenter(ISelectionSimulationPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<SimulationSelectionDTO> simulationSelectionDTOs)
      {
         _dtos = simulationSelectionDTOs;
         _gridViewBinder.BindToSource(_dtos);
         NotifyViewChanged();
      }

      public bool AllowMultiSelect { get; set; }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.Selected)
          .WithRepository(selectRepository)
          .WithOnValueUpdating(onValueSet)
          .WithFixedWidth(UIConstants.Size.EMBEDDED_CHECK_BOX_WIDTH);

         _gridViewBinder.Bind(x => x.Name)
            .AsReadOnly();

         _gridViewBinder.Changed += NotifyViewChanged;
      }

      private void onValueSet(SimulationSelectionDTO simulationSelectionDTO, PropertyValueSetEventArgs<bool> args)
      {
         if(!AllowMultiSelect)
            deselectAll(simulationSelectionDTO);
      }

      private void deselectAll(SimulationSelectionDTO simulationSelectionDTO)
      {
         _dtos.Except(new[] {simulationSelectionDTO}).Where(dto => dto.Selected).Each(dto => dto.Selected = false);
      }

      private RepositoryItem selectRepository(SimulationSelectionDTO dto)
      {
         return _selectEditor;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemGridSimulations.TextVisible = false;
         Caption = Captions.SelectSimulations;
      }

      public override bool HasError => _gridViewBinder.HasError;
   }
}