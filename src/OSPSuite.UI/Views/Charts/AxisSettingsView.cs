using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;

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
      private GridViewBinder<Axis> _gridBinderAxes;
      private readonly RepositoryItem _disableRepositoryItem = new RepositoryItem {ReadOnly = true, Enabled = false};

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
         _numberModeRepository.Items.AddRange(Enum.GetValues(typeof(NumberModes)));
         _lineStyleRepository.Items.AddRange(Enum.GetValues(typeof(LineStyles)));

         _gridBinderAxes = new GridViewBinder<Axis>(gridView) {BindingMode = BindingMode.TwoWay};

         createColumn(x => x.AxisType, AxisOptionsColumns.AxisType)
            .AsReadOnly();

         createColumn(x => x.Caption, AxisOptionsColumns.Caption);

         createColumn(x => x.Dimension, AxisOptionsColumns.Dimension, _dimensionRepository)
            .WithEditorConfiguration(configureDimensionRepository);

         createColumn(x => x.UnitName, AxisOptionsColumns.UnitName, _unitRepository, beforeNotificationAction: x => _presenter.UnitChanged(x))
            .WithEditorConfiguration(configureUnitEditor);

         createColumn(x => x.Scaling, AxisOptionsColumns.Scaling, _scalingRepository);

         createColumn(x => x.NumberMode, AxisOptionsColumns.NumberMode, _numberModeRepository);

         createColumn(x => x.Min, AxisOptionsColumns.Min);

         createColumn(x => x.Max, AxisOptionsColumns.Max);

         createColumn(x => x.GridLines, AxisOptionsColumns.GridLines, _gridLinesRepository);

         createColumn(x => x.DefaultColor, AxisOptionsColumns.DefaultColor, _colorRepository, toolTip: ToolTips.DefaultCurveColor)
            .WithRepository(defaultColorRepository);

         createColumn(x => x.DefaultLineStyle, AxisOptionsColumns.DefaultLineStyle, _lineStyleRepository, toolTip: ToolTips.DefaultLineStyle)
            .WithRepository(defaultLineStyleRepository);

         //Add/Delete-column
         _gridBinderAxes.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH)
            .WithRepository(getButton);

         _gridBinderAxes.Changed += NotifyViewChanged;
      }

      private RepositoryItem defaultLineStyleRepository(Axis axis)
      {
         if (axis.IsXAxis)
            return _disableRepositoryItem;

         return _lineStyleRepository;
      }

      private RepositoryItem defaultColorRepository(Axis axis)
      {
         if (axis.IsXAxis)
            return _disableRepositoryItem;

         return _colorRepository;
      }

      private IGridViewColumn<Axis> createColumn<T>(Expression<Func<Axis, T>> propertyToBindTo,
         AxisOptionsColumns axisOptionsColumn,
         RepositoryItem repositoryItem = null,
         bool showInColumnChooser = true,
         string toolTip = null,
         Action<Axis> beforeNotificationAction = null)
      {
         var column = _gridBinderAxes.Bind(propertyToBindTo)
            .WithShowInColumnChooser(showInColumnChooser);

         if (beforeNotificationAction != null)
            column.WithOnValueUpdated((axis, value) => beforeNotificationAction(axis));

         //Order is important: needs to be done after the beforeNotification if defined.      
         column.WithOnValueUpdated((axis, value) => notifyAxisPropertyChanged(axis));

         if (repositoryItem != null)
            column.WithRepository(axis => repositoryItem);

         if (toolTip != null)
            column.WithToolTip(ToolTips.DefaultLineStyle);

         column.XtraColumn.Tag = axisOptionsColumn.ToString();

         return column;
      }

      private void notifyAxisPropertyChanged(Axis axis)
      {
         _presenter.NotifyAxisPropertyChanged(axis);
      }

      private void configureDimensionRepository(BaseEdit baseEdit, Axis axis)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.AllDimensions(axis.Dimension));
      }

      public void AttachPresenter(IAxisSettingsPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

      private void configureUnitEditor(BaseEdit baseEdit, Axis axis)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.AllUnitNamesFor(axis.Dimension));
      }

      private RepositoryItem getButton(Axis axis)
      {
         switch (axis.AxisType)
         {
            case AxisTypes.X:
               return _disableRepositoryItem;
            case AxisTypes.Y:
               return _addButtonRepository;
            default:
               return _deleteButtonRepository;
         }
      }

      public void BindTo(IEnumerable<Axis> axes)
      {
         DoWithoutColumnSettingsUpdateNotification(() => { _gridBinderAxes.BindToSource(axes.ToBindingList()); });
      }

      public void DeleteBinding()
      {
         _gridBinderAxes.DeleteBinding();
      }

      private void deleteButtonClick()
      {
         var focusedAxis = _gridBinderAxes.FocusedElement;
         _presenter.RemoveAxis(focusedAxis);
      }

      private void addButtonClick()
      {
         _presenter.AddYAxis();
      }
   }
}