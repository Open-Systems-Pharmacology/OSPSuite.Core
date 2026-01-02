using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Format;
using PopupMenuShowingEventArgs = DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs;

namespace OSPSuite.UI.Views
{
   public partial class SimulationOutputMappingView : BaseUserControl, ISimulationOutputMappingView
   {
      private ISimulationOutputMappingPresenter _presenter;
      private readonly GridViewBinder<SimulationOutputMappingDTO> _gridViewBinder;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly UxRepositoryItemComboBox _outputRepository;
      private readonly UxRepositoryItemComboBox _singleObservedDataRepository;
      private readonly UxRepositoryItemScalings _scalingRepository;

      public SimulationOutputMappingView()
      {
         InitializeComponent();
         gridView.AllowsFiltering = false;
         _gridViewBinder = new GridViewBinder<SimulationOutputMappingDTO>(gridView);
         _outputRepository = new UxRepositoryItemComboBox(gridView)
         {
            AllowNullInput = DefaultBoolean.True,
            NullText = Captions.SimulationUI.NoneEditorNullText
         };
         _singleObservedDataRepository = new UxRepositoryItemComboBox(gridView);
         _scalingRepository = new UxRepositoryItemScalings(gridView);
         gridView.MultiSelect = true;
         gridView.PopupMenuShowing += onPopupMenuShowing;
         deleteButton.InitWithImage(ApplicationIcons.Delete, "Delete");
         deleteButton.Click += (o, e) => OnEvent(btnDeleteItemClick);
         deleteButtonControlItem.AdjustButtonSize();
      }

      public void AttachPresenter(ISimulationOutputMappingPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<SimulationOutputMappingDTO> outputMappingList)
      {
         _gridViewBinder.BindToSource(outputMappingList);
      }

      public void RefreshGrid()
      {
         gridView.RefreshData();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _gridViewBinder.AutoBind(x => x.ObservedData)
            .WithRepository(singleObservedDataRepository)
            .WithFormat(observedDataDisplay)
            .WithCaption(Captions.SimulationUI.ObservedData)
            .AsReadOnly();

         _gridViewBinder.AutoBind(x => x.Output)
            .WithRepository(allOutputsRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithOnValueUpdated((o, e) => onOutputValueChanged(o))
            .WithCaption(Captions.SimulationUI.Outputs);

         _gridViewBinder.Bind(x => x.Scaling)
            .WithCaption(Captions.Scaling)
            .WithOnValueUpdated((o, e) => onOutputMappingEdited())
            .WithRepository(x => _scalingRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.Bind(x => x.Weight)
            .WithOnValueUpdated((o, e) => onOutputMappingEdited());

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(x => _removeButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.Changed += NotifyViewChanged;
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveObservedData(new List<SimulationOutputMappingDTO> { _gridViewBinder.FocusedElement }));
      }

      private void onOutputMappingEdited()
      {
         _presenter.MarkSimulationAsChanged();
      }

      private void onOutputValueChanged(SimulationOutputMappingDTO simulationOutputMappingDTO)
      {
         _presenter.UpdateSimulationOutputMappings(simulationOutputMappingDTO);
      }

      private RepositoryItemComboBox singleObservedDataRepository(SimulationOutputMappingDTO outputMappingDTO)
      {
         _singleObservedDataRepository.FillComboBoxRepositoryWith(outputMappingDTO.ObservedData);
         return _singleObservedDataRepository;
      }

      private IFormatter<DataRepository> observedDataDisplay(SimulationOutputMappingDTO outputMappingDTO) =>
         new WeightedObservedDataFormatter(outputMappingDTO);

      private RepositoryItem allOutputsRepository(SimulationOutputMappingDTO dto)
      {
         _outputRepository.FillComboBoxRepositoryWith(_presenter.AllAvailableOutputs);
         return _outputRepository;
      }

      public override string Caption => Captions.SimulationUI.ObservedDataSelection;

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.ObservedData;

      private void btnDeleteItemClick()
      {
         var selectedDTOs = gridView.GetSelectedRows().SelectMany(x => _gridViewBinder.SelectedItems(x)).ToList();

         if (!selectedDTOs.Any())
            return;

         _presenter.RemoveObservedData(selectedDTOs);
      }

      private void onPopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
      {
         if (e.Menu == null || e.HitInfo.HitTest != GridHitTest.RowIndicator)
            return;

         var selectedDTOs = gridView.GetSelectedRows().SelectMany(x => _gridViewBinder.SelectedItems(x)).ToList();

         if (!selectedDTOs.Any())
            return;

         var deleteItem = new DXMenuItem(
            Captions.DeleteSelected,
            (o, args) => _presenter.RemoveObservedData(selectedDTOs)
         )
         {
            Shortcut = Shortcut.Del,
            SvgImage = ApplicationIcons.DeleteSelected
         };

         e.Menu.Items.Insert(e.Menu.Items.Count, deleteItem);
      }
   }
}