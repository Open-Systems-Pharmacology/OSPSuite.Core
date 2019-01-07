using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Data;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.SensitivityAnalyses;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class SensitivityAnalysisParametersView : BaseUserControl, ISensitivityAnalysisParametersView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private ISensitivityAnalysisParametersPresenter _presenter;
      private readonly GridViewBinder<SensitivityParameterDTO> _gridViewBinder;
      private readonly ComboBoxUnitParameter _comboBoxUnit;
      private readonly RepositoryItemButtonEdit _removeButtonRepository = new UxRemoveButtonRepository();
      private IGridViewColumn _colName;
      private const int PANEL_WITDH = 250;
      private const int PANEL_HEIGHT = 29;

      public SensitivityAnalysisParametersView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<SensitivityParameterDTO>(gridView);
         _comboBoxUnit = new ComboBoxUnitParameter(gridControl);
         _comboBoxUnit.ParameterUnitSet += setParameterUnit;

         gridView.AllowsFiltering = false;
         gridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
         gridView.OptionsSelection.EnableAppearanceFocusedRow = true;
         gridView.OptionsSelection.MultiSelect = true;

         gridControl.ToolTipController = new ToolTipController().Initialize(imageListRetriever);
         gridControl.ToolTipController.GetActiveObjectInfo += (o, e) => OnEvent(() => onToolTipControllerGetActiveObjectInfo(e));
      }

      public void AttachPresenter(ISensitivityAnalysisParametersPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(IReadOnlyList<SensitivityParameterDTO> allSensitivityParameterDtos)
      {
         _gridViewBinder.BindToSource(allSensitivityParameterDtos.ToBindingList());
         gridView.RefreshData();
         gridView.CloseEditor();
         lblNumberOfParameters.Text = Captions.SensitivityAnalysis.NumberOfSelectedParameters(allSensitivityParameterDtos.Count);
      }

      private void onToolTipControllerGetActiveObjectInfo(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var column = gridView.ColumnAt(e);
         if (!Equals(_colName.XtraColumn, column)) return;

         var sensitivityParameterDTO = _gridViewBinder.ElementAt(e);
         if (sensitivityParameterDTO == null) return;

         var superToolTip = toolTipFor(sensitivityParameterDTO);

         e.Info = _toolTipCreator.ToolTipControlInfoFor(sensitivityParameterDTO, superToolTip);
      }

      private SuperToolTip toolTipFor(SensitivityParameterDTO sensitivityParameterDTO)
      {
         return _toolTipCreator.CreateToolTip(sensitivityParameterDTO.DisplayPath, sensitivityParameterDTO.Name, ApplicationIcons.Parameter);
      }

      public IEnumerable<SensitivityParameterDTO> SelectedParameters()
      {
         for (var i = 0; i < gridView.SelectedRowsCount; i++)
         {
            var row = gridView.GetSelectedRows()[i];
            yield return gridView.GetRow(row).DowncastTo<SensitivityParameterDTO>();
         }
      }

      public void SetNMaxView(IView view)
      {
         panelSetNMax.FillWith(view);
      }

      public void SetRangeView(IView view)
      {
         panelSetRange.FillWith(view);
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _colName = _gridViewBinder.Bind(x => x.Name)
            .WithCaption(Captions.Name)
            .WithOnValueUpdating((o, e) => OnEvent(() => _presenter.ChangeName(o, e.NewValue)));
            
         _colName.XtraColumn.SortMode = ColumnSortMode.Value;
         _colName.XtraColumn.SortOrder = ColumnSortOrder.Ascending;

         _gridViewBinder.Bind(x => x.NumberOfSteps)
            .WithCaption(Captions.SensitivityAnalysis.NumberOfSteps)
            .WithToolTip(Captions.SensitivityAnalysis.NumberOfStepsDescription)
            .WithFormat(x => x.NumberOfStepsParameter.IntParameterFormatter())
            .WithEditorConfiguration((activeEditor, dto) => _comboBoxUnit.UpdateUnitsFor(activeEditor, dto.NumberOfStepsParameter))
            .OnValueUpdating += (dto, valueInGuiUnit) => setParameterValue(dto.NumberOfStepsParameter, valueInGuiUnit.NewValue);

         _gridViewBinder.Bind(x => x.VariationRange)
            .WithCaption(Captions.SensitivityAnalysis.VariationRange)
            .WithToolTip(Captions.SensitivityAnalysis.VariationRangeDescription)
            .WithFormat(x => x.NumberOfStepsParameter.ParameterFormatter())
            .WithEditorConfiguration((activeEditor, dto) => _comboBoxUnit.UpdateUnitsFor(activeEditor, dto.VariationRangeParameter))
            .OnValueUpdating += (dto, valueInGuiUnit) => setParameterValue(dto.VariationRangeParameter, valueInGuiUnit.NewValue);

         _gridViewBinder.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(x => _removeButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);

         _gridViewBinder.Changed += NotifyViewChanged;
         _removeButtonRepository.ButtonClick += (o, e) => OnEvent(() => _presenter.RemoveSensitivityParameter(_gridViewBinder.FocusedElement));
         btnRemoveAll.Click += (o, e) => OnEvent(() => _presenter.RemoveAllParameters());
         gridView.HiddenEditor += (o, e) => { _comboBoxUnit.Visible = false; };
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
         layoutItemParameters.TextVisible = false;
         layoutItemRemoveAll.AdjustLargeButtonSize();
         btnRemoveAll.InitWithImage(ApplicationIcons.Remove, Captions.SensitivityAnalysis.RemoveAll);

         updatePanelSize(panelSetNMax);
         updatePanelSize(panelSetRange);

         layoutControlItemSetNMax.Text = Captions.SensitivityAnalysis.NumberOfSteps.FormatForLabel();
         layoutControlItemSetRange.Text = Captions.SensitivityAnalysis.VariationRange.FormatForLabel();

         multiSetValueControlGroup.Text = Captions.SensitivityAnalysis.UpdateMultipleSensitivityParameters;
         lblNumberOfParameters.AsDescription();
      }

      private void updatePanelSize(PanelControl panelControl)
      {
         var panelSize = new Size(PANEL_WITDH, PANEL_HEIGHT);
         panelControl.MinimumSize = panelSize;
         panelControl.MaximumSize = panelSize;
      }
   }
}
