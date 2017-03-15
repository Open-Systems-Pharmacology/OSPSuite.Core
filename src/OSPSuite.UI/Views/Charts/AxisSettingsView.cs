using System;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility.Collections;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class AxisSettingsView : BaseUserControlWithColumnSettings, IAxisSettingsView
   {
      private IAxisSettingsPresenter _presenter;
      private readonly RepositoryItemComboBox _dimensionRepository;
      private readonly RepositoryItemCheckEdit _gridLinesRepository;
      private readonly RepositoryItemComboBox _numberModeRepository;
      private readonly RepositoryItemComboBox _scalingRepository;
      private readonly RepositoryItemComboBox _unitRepository;
      private readonly RepositoryItemComboBox _lineStyleRepository; 
      private readonly RepositoryItemColorEdit _colorRepository;
      private readonly RepositoryItemButtonEdit _deleteButtonRepository;
      private readonly RepositoryItemButtonEdit _addButtonRepository;
      private GridViewBinder<IAxis> _gridBinderAxes;

      public AxisSettingsView()
      {
         InitializeComponent();
         gridView.AllowsFiltering = true;
         gridView.OptionsCustomization.AllowFilter = false;
         gridView.OptionsCustomization.AllowSort = false;
         gridView.OptionsCustomization.AllowGroup = false;
         gridView.OptionsView.ShowGroupPanel = false;
         gridView.ShowColumnChooser = true;
         gridView.ShouldUseColorForDisabledCell = false;

         _dimensionRepository = new UxRepositoryItemComboBox(gridView);
         _unitRepository = new UxRepositoryItemComboBox(gridView);
         _scalingRepository = new UxRepositoryItemScalings(gridView);
         _numberModeRepository = new UxRepositoryItemComboBox(gridView);
         _gridLinesRepository = new UxRepositoryItemCheckEdit(gridView);
         _lineStyleRepository = new UxRepositoryItemComboBox(gridView);
         _colorRepository = new UxRepositoryItemColorPickEditWithHistory(gridView);

         _deleteButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
         _deleteButtonRepository.ButtonClick += (o, e) => OnEvent(deleteButtonClick);

         _addButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Plus);
         _addButtonRepository.ButtonClick += (o, e) => OnEvent(addButtonClick);

         InitializeWith(gridView);
      }

      public override void InitializeBinding()
      {
         // initialize RepositoryItems
         _numberModeRepository.Items.AddRange(Enum.GetValues(typeof (NumberModes)));
         _lineStyleRepository.Items.AddRange(Enum.GetValues(typeof(LineStyles)));

         _gridBinderAxes = new GridViewBinder<IAxis>(gridView) {BindingMode = BindingMode.TwoWay};

         _gridBinderAxes.Bind(axis => axis.AxisType)
            .AsReadOnly()
            .XtraColumn.Tag = AxisOptionsColumns.AxisType.ToString();

         _gridBinderAxes.Bind(axis => axis.Caption)
            .WithShowInColumnChooser(true)
            .XtraColumn.Tag = AxisOptionsColumns.Caption.ToString();

         _gridBinderAxes.Bind(axis => axis.Dimension)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _dimensionRepository)
            .WithEditorConfiguration(configureDimensionRepository)
            .XtraColumn.Tag = AxisOptionsColumns.Dimension.ToString();

         _gridBinderAxes.Bind(axis => axis.UnitName)
            .WithShowInColumnChooser(true)
            .WithRepository(axis => _unitRepository)
            .WithEditorConfiguration(configureUnitEditor)
            // reset axis, if unit is changed
            .WithOnChanged(axis => axis.ResetRange())
            .XtraColumn.Tag = AxisOptionsColumns.UnitName.ToString();

         _gridBinderAxes.Bind(axis => axis.Scaling)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _scalingRepository)
            .XtraColumn.Tag = AxisOptionsColumns.Scaling.ToString();

         _gridBinderAxes.Bind(axis => axis.NumberMode)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _numberModeRepository)
            .XtraColumn.Tag = AxisOptionsColumns.NumberMode.ToString();

         _gridBinderAxes.Bind(axis => axis.Min)
            .WithShowInColumnChooser(true)
            .XtraColumn.Tag = AxisOptionsColumns.Min.ToString();

         _gridBinderAxes.Bind(axis => axis.Max)
            .WithShowInColumnChooser(true)
            .XtraColumn.Tag = AxisOptionsColumns.Max.ToString();

         _gridBinderAxes.Bind(axis => axis.GridLines)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _gridLinesRepository)
            .XtraColumn.Tag = AxisOptionsColumns.GridLines.ToString();

         _gridBinderAxes.AutoBind(axis => axis.DefaultColor)
            .WithShowInColumnChooser(true)
            .WithRepository(axis => _colorRepository)
            .WithToolTip(ToolTips.DefaultCurveColor)
            .WithEditorConfiguration(disableForXAxis)
            .XtraColumn.Tag = AxisOptionsColumns.DefaultColor.ToString();

         _gridBinderAxes.AutoBind(axis => axis.DefaultLineStyle)
            .WithShowInColumnChooser(true)
            .WithRepository(axis => _lineStyleRepository)
            .WithToolTip(ToolTips.DefaultLineStyle)
            .WithEditorConfiguration(disableForXAxis)
            .XtraColumn.Tag = AxisOptionsColumns.DefaultLineStyle.ToString();

         //Add/Delete-column
         _gridBinderAxes.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .WithRepository(getButton);

         _gridBinderAxes.Changed += NotifyViewChanged;
      }

      private static void disableForXAxis(BaseEdit baseEdit, IAxis axis)
      {
         if (axis.AxisType.IsYAxis()) return;
         baseEdit.Enabled = false;
      }

      private void configureDimensionRepository(BaseEdit baseEdit, IAxis axis)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.GetDimensions(axis.Dimension));
      }

      public void AttachPresenter(IAxisSettingsPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

      private void configureUnitEditor(BaseEdit baseEdit, IAxis axis)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.GetUnitNamesFor(axis.Dimension));
      }

      private RepositoryItem getButton(IAxis axis)
      {
         switch (axis.AxisType)
         {
            case AxisTypes.X:
               return new RepositoryItem();
            case AxisTypes.Y:
               return _addButtonRepository;
            default:
               return _deleteButtonRepository;
         }
      }

      public override void Refresh()
      {
         base.Refresh();
         _gridBinderAxes.Rebind();
      }

      public void BindToSource(ICache<AxisTypes, IAxis> axes)
      {
         _gridBinderAxes.BindToSource(axes);
         Refresh();
      }

      public void DeleteBinding()
      {
         _gridBinderAxes.DeleteBinding();
      }

      private void deleteButtonClick()
      {
         var focusedAxis = _gridBinderAxes.FocusedElement;
         _presenter.RemoveAxis(focusedAxis.AxisType);
      }

      private void addButtonClick()
      {
         _presenter.AddYAxis();
      }
   }
}