using System.Collections.Generic;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.Utils;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Formatters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationIdentificationParametersView : BaseUserControl, IParameterIdentificationIdentificationParametersView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private IParameterIdentificationIdentificationParametersPresenter _presenter;
      private readonly GridViewBinder<IdentificationParameterDTO> _gridViewBinder;
      private readonly ComboBoxUnitParameter _comboBoxUnit;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private readonly UxRepositoryItemCheckEdit _booleanRepository;
      private readonly UxRepositoryItemScalings _scalingRepository;
      private readonly UxRepositoryItemButtonEdit _repositoryItemUpdateFromSimulation;
      private IGridViewColumn _colName;

      public ParameterIdentificationIdentificationParametersView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
         gridView.AllowsFiltering = false;
         _gridViewBinder = new GridViewBinder<IdentificationParameterDTO>(gridView) {ValidationMode = ValidationMode.LeavingRow};
         _comboBoxUnit = new ComboBoxUnitParameter(gridControl);
         _booleanRepository = new UxRepositoryItemCheckEdit(gridView);
         _scalingRepository = new UxRepositoryItemScalings(gridView);
         _repositoryItemUpdateFromSimulation = new UxRepositoryItemButtonImage(ApplicationIcons.Refresh, Captions.ParameterIdentification.UpdateStartValuesFromSimulation);
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsSelection.MultiSelect = false;
         gridControl.ToolTipController=  new ToolTipController().Initialize(imageListRetriever);
         gridControl.ToolTipController.GetActiveObjectInfo += (o, e) => OnEvent(()=>onToolTipControllerGetActiveObjectInfo(e));
      }

      private void onToolTipControllerGetActiveObjectInfo(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var column = gridView.ColumnAt(e);
         if (!Equals(_colName.XtraColumn, column)) return;

         var identificationParameterDTO = _gridViewBinder.ElementAt(e);
         if (identificationParameterDTO == null) return;

         var superToolTip = _toolTipCreator.ToolTipFor(identificationParameterDTO);

         e.Info = _toolTipCreator.ToolTipControlInfoFor(identificationParameterDTO, superToolTip);
      }


      public void AttachPresenter(IParameterIdentificationIdentificationParametersPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IEnumerable<IdentificationParameterDTO> allIdentificationParameterDTOs)
      {
         _gridViewBinder.BindToSource(allIdentificationParameterDTOs);
         gridView.RefreshData();
         gridView.CloseEditor();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _colName= _gridViewBinder.Bind(x => x.Name)
            .WithFormat(x => new IdentificationParameterNameFormatter(x))
            .WithCaption(Captions.Name)
            .WithOnValueUpdating((o, e) => OnEvent(() => _presenter.ChangeName(o, e.OldValue, e.NewValue)));

         // Using AutoBind in these cases to force dto validation to occur when binding takes place. 
         // In some cases, the first value can be invalid and requires user attention.
         _gridViewBinder.AutoBind(x => x.StartValue)
            .WithCaption(Captions.ParameterIdentification.StartValue)
            .WithFormat(x => x.StartValueParameter.ParameterFormatter())
            .WithEditorConfiguration((activeEditor, dto) => _comboBoxUnit.UpdateUnitsFor(activeEditor, dto.StartValueParameter))
            .OnValueUpdating += (dto, valueInGuiUnit) => setParameterValue(dto.StartValueParameter, valueInGuiUnit.NewValue);

         _gridViewBinder.AutoBind(x => x.MinValue)
            .WithCaption(Captions.ParameterIdentification.MinValue)
            .WithFormat(x => x.MinValueParameter.ParameterFormatter())
            .WithEditorConfiguration((activeEditor, dto) => _comboBoxUnit.UpdateUnitsFor(activeEditor, dto.MinValueParameter))
            .OnValueUpdating += (dto, valueInGuiUnit) => setParameterValue(dto.MinValueParameter, valueInGuiUnit.NewValue);

         _gridViewBinder.AutoBind(x => x.MaxValue)
            .WithCaption(Captions.ParameterIdentification.MaxValue)
            .WithFormat(x => x.MaxValueParameter.ParameterFormatter())
            .WithEditorConfiguration((activeEditor, dto) => _comboBoxUnit.UpdateUnitsFor(activeEditor, dto.MaxValueParameter))
            .OnValueUpdating += (dto, valueInGuiUnit) => setParameterValue(dto.MaxValueParameter, valueInGuiUnit.NewValue);

         _gridViewBinder.Bind(x => x.Scaling)
            .WithCaption(Captions.Scaling)
            .WithRepository(x => _scalingRepository)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.Bind(x => x.UseAsFactor)
            .WithCaption(Captions.ParameterIdentification.UseAsFactor)
            .WithRepository(x => _booleanRepository)
            .OnChanged += dto => OnEvent(() => _presenter.UseAsFactorChanged(dto));

         _gridViewBinder.Bind(x => x.IsFixed)
            .WithCaption(Captions.ParameterIdentification.IsFixed)
            .WithRepository(x => _booleanRepository);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .WithRepository(x=>_repositoryItemUpdateFromSimulation)
            .WithShowButton(ShowButtonModeEnum.ShowAlways);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(x => _removeButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _comboBoxUnit.ParameterUnitSet += setParameterUnit;
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveIdentificationParameter(_gridViewBinder.FocusedElement));
         _repositoryItemUpdateFromSimulation.ButtonClick += (o, e) => OnEvent(() => _presenter.UpdateStartValueFromSimulation(_gridViewBinder.FocusedElement));

         gridView.HiddenEditor += (o, e) => { _comboBoxUnit.Visible = false; };
         gridView.FocusedRowChanged += (o, e) => OnEvent(gridViewRowChanged, e);

         _gridViewBinder.Changed += NotifyViewChanged;
      }

      private void gridViewRowChanged(FocusedRowChangedEventArgs e)
      {
         var selectedItem = _gridViewBinder.ElementAt(e.FocusedRowHandle);
         if (selectedItem == null) return;
         _presenter.SelectIdentificationParameter(selectedItem);
      }

      public IdentificationParameterDTO SelectedIdentificationParameter
      {
         get { return _gridViewBinder.FocusedElement; }
         set
         {
            var rowHandle = _gridViewBinder.RowHandleFor(value);
            gridView.FocusedRowHandle = rowHandle;
            gridView.SelectRow(rowHandle);
         }
      }

      private void setParameterValue(IParameterDTO parameterDTO, double newValue)
      {
         OnEvent(() => _presenter.SetParameterValue(parameterDTO, newValue));
      }

      private void setParameterUnit(IParameterDTO parameterDTO, Unit newUnit)
      {
         OnEvent(() =>
         {
            gridView.CloseEditor();
            _presenter.SetParameterUnit(parameterDTO, newUnit);
         });
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemIdentificationParameters.TextVisible = false;
      }
   }
}