using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Forms;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   internal partial class CurveSettingsView : BaseUserControlWithColumnSettings, ICurveSettingsView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private readonly RepositoryItemComboBox _axisTypeRepository;
      private readonly RepositoryItemColorEdit _colorRepository;
      private readonly RepositoryItemComboBox _interpolationModeRepository;
      private readonly RepositoryItemComboBox _lineStyleRepository;
      private readonly RepositoryItemComboBox _lineThicknessRepository;
      private readonly RepositoryItemComboBox _symbolRepository;
      private readonly RepositoryItemCheckEdit _visibleRepository;
      private readonly RepositoryItemButtonEdit _deleteButtonRepository;
      private readonly GridViewBinder<CurveDTO> _gridBinderCurves;
      private ICurveSettingsPresenter _presenter;

      private GridHitInfo _downHitInfo; //Drag & Drop
      private IGridViewColumn _xDataColumn;
      private IGridViewColumn _yDataColumn;
      private IGridViewColumn _colName;
      private readonly RepositoryItemCheckEdit _showLowerLimitOfQuantificationRepository;
      private IGridViewColumn _colorColumn;

      public CurveSettingsView(IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();

         _gridBinderCurves = new GridViewBinder<CurveDTO>(gridView) {BindingMode = BindingMode.TwoWay};

         _interpolationModeRepository = new UxRepositoryItemComboBox(gridView);
         _lineStyleRepository = new UxRepositoryItemComboBox(gridView);
         _lineThicknessRepository = new UxRepositoryItemComboBox(gridView);
         _symbolRepository = new UxRepositoryItemComboBox(gridView);
         _axisTypeRepository = new UxRepositoryItemComboBox(gridView);
         _colorRepository = new UxRepositoryItemColorPickEditWithHistory(gridView);
         _visibleRepository = new UxRepositoryItemCheckEdit(gridView);
         _deleteButtonRepository = new UxRepositoryItemButtonEdit(ButtonPredefines.Delete);
         _showLowerLimitOfQuantificationRepository = new UxRepositoryItemCheckEdit(gridView);
         _deleteButtonRepository.ButtonClick += deleteButtonClick;

         gridView.AllowsFiltering = true;
         gridView.ShowColumnChooser = true;
         gridView.ShouldUseColorForDisabledCell = false;
         gridView.OptionsView.ShowGroupPanel = false;

         var toolTipController = new ToolTipController();
         toolTipController.GetActiveObjectInfo += onToolTipControllerGetActiveObjectInfo;

         gridControl.ToolTipController = toolTipController;
         InitializeWith(gridView);
         initializeDragDrop();
      }

      public override void InitializeBinding()
      {
         _interpolationModeRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<InterpolationModes>());
         _interpolationModeRepository.TextEditStyle = TextEditStyles.DisableTextEditor;
         _lineStyleRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<LineStyles>());
         _lineThicknessRepository.FillComboBoxRepositoryWith(new[] {1, 2, 3});
         _symbolRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<Symbols>());
         _axisTypeRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<AxisTypes>());
         _axisTypeRepository.Items.Remove(AxisTypes.X);

         _colName = createFor(curve => curve.Name, CurveOptionsColumns.Name);

         _xDataColumn = createFor(curve => curve.xData, CurveOptionsColumns.xData)
            .AsReadOnly();

         _yDataColumn = createFor(curve => curve.yData, CurveOptionsColumns.yData)
            .WithFormat(c => new DataColumnFormatter(_presenter))
            .AsReadOnly();

         createFor(curve => curve.yAxisType, CurveOptionsColumns.yAxisType, _axisTypeRepository)
            .WithEditorConfiguration(configureAxisTypeEditor);

         createFor(curve => curve.InterpolationMode, CurveOptionsColumns.InterpolationMode, _interpolationModeRepository);

         _colorColumn = createFor(curve => curve.Color, CurveOptionsColumns.Color, _colorRepository);

         createFor(curve => curve.LineStyle, CurveOptionsColumns.LineStyle, _lineStyleRepository);

         createFor(curve => curve.Symbol, CurveOptionsColumns.Symbol, _symbolRepository);

         createFor(curve => curve.LineThickness, CurveOptionsColumns.LineThickness, _lineThicknessRepository);

         createFor(curve => curve.Visible, CurveOptionsColumns.Visible, _visibleRepository);

         createFor(curve => curve.VisibleInLegend, CurveOptionsColumns.VisibleInLegend, _visibleRepository);

         createFor(curve => curve.ShowLLOQ, CurveOptionsColumns.ShowLowerLimitOfQuantification, _showLowerLimitOfQuantificationRepository);

         var legendIndexColumn = _gridBinderCurves.Bind(curve => curve.LegendIndex);
         legendIndexColumn.Visible = false;
         gridView.Columns[legendIndexColumn.ColumnName].SortOrder = ColumnSortOrder.Ascending;

         //Delete-column
         _gridBinderCurves.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(curve => _deleteButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);
      }

      private IGridViewAutoBindColumn<CurveDTO, T> createFor<T>(Expression<Func<CurveDTO, T>> propertyToBindTo, CurveOptionsColumns curveOptionsColumn, RepositoryItem repositoryItem = null, bool showInColumnChooser = true)
      {
         var column = _gridBinderCurves.AutoBind(propertyToBindTo)
            .WithShowInColumnChooser(showInColumnChooser)
            .WithOnValueUpdated((curveDTO, value) => notifyCurvePropertyChange(curveDTO));

         if (repositoryItem != null)
            column.WithRepository(curve => repositoryItem);

         column.XtraColumn.Tag = curveOptionsColumn.ToString();
         return column;
      }

      private void notifyCurvePropertyChange(CurveDTO curveDTO)
      {
         _presenter.NotifyCurvePropertyChange(curveDTO);
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != gridControl) return;

         var hi = gridView.HitInfoAt(e.ControlMousePosition);
         if (hi == null) return;

         if (!Equals(hi.Column, _colName.XtraColumn))
            return;

         var curveDTO = _gridBinderCurves.ElementAt(e);
         if (curveDTO == null) return;

         var toolTip = _toolTipCreator.CreateToolTip(_presenter.ToolTipFor(curveDTO));
         e.Info = _toolTipCreator.ToolTipControlInfoFor(curveDTO, toolTip);
      }

      private void configureAxisTypeEditor(BaseEdit baseEdit, CurveDTO curve)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.AllYAxisTypes);
      }

      public void AttachPresenter(ICurveSettingsPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

      public void BindTo(IEnumerable<CurveDTO> curves)
      {
         DoWithoutColumnSettingsUpdateNotification(() => { _gridBinderCurves.BindToSource(curves.ToBindingList()); });
      }

      private void initializeDragDrop()
      {
         gridControl.AllowDrop = true;
         // for Drag&Drop of Colors
         gridView.MouseDown += (o, e) => OnEvent(viewMouseDown, o, e);
         _colorRepository.MouseDown += (o, e) => OnEvent(viewMouseDown, o, e);

         gridView.MouseMove += (o, e) => OnEvent(viewMouseMove, o, e);
         _colorRepository.MouseMove += (o, e) => OnEvent(viewMouseMove, o, e);

         // for Drop of Colors and (DataColumns to x/yData)
         gridControl.DragOver += (o, e) => OnEvent(gridDragOver, e);
         gridControl.DragDrop += (o, e) => OnEvent(gridDragDrop, e);
         gridControl.ProcessGridKey += (o, e) => OnEvent(gridProcessGridKey, e);
      }

      private void deleteButtonClick(object sender, ButtonPressedEventArgs e)
      {
         var focusedCurve = _gridBinderCurves.FocusedElement;
         if (focusedCurve == null) return;
         _presenter.Remove(focusedCurve);
      }

      private void viewMouseDown(object sender, MouseEventArgs e)
      {
         _downHitInfo = null;
         if (ModifierKeys != Keys.None) return;

         var cursorLocation = getCursorLocationRelativeToGrid(sender, e);
         _downHitInfo = gridView.CalcHitInfo(cursorLocation);
      }

      private static Point getCursorLocationRelativeToGrid(object sender, MouseEventArgs e)
      {
         var cursorLocation = e.Location;
         var colorEdit = sender as ColorEdit;

         // convert internal coordinates of color edit to gridview coordinates
         if (colorEdit != null)
            cursorLocation.Offset(colorEdit.Location);

         return cursorLocation;
      }

      private void viewMouseMove(object sender, MouseEventArgs e)
      {
         if (_downHitInfo == null)
            return;

         var cursorLocation = getCursorLocationRelativeToGrid(sender, e);

         if (isNotValidMouseMove(e, cursorLocation))
            return;

         var curve = _gridBinderCurves.ElementAt(_downHitInfo.RowHandle);
         if (hitInColorCell(_downHitInfo))
         {
            handleAsDraggingColor(sender as ColorEdit, curve);
            DXMouseEventArgs.GetMouseArgs(e).Handled = true;
         }
         else if (hitInRowIndicator(_downHitInfo))
         {
            handleAsDraggingCurve(curve);
            DXMouseEventArgs.GetMouseArgs(e).Handled = true;
         }
      }

      private bool isNotValidMouseMove(MouseEventArgs e, Point cursorLocation)
      {
         return !cursorHasMoved(cursorLocation) || e.Button != MouseButtons.Left;
      }

      private void handleAsDraggingCurve(CurveDTO curveDTO)
      {
         gridControl.DoDragDrop(curveDTO, DragDropEffects.Move);
         _downHitInfo = null;
      }

      private void handleAsDraggingColor(ColorEdit colorEdit, CurveDTO curveDTO)
      {
         var color = curveDTO.Color;
         colorEdit?.ClosePopup();
         gridControl.DoDragDrop(color, DragDropEffects.Copy);
         _downHitInfo = null;
      }

      private bool hitInRowIndicator(GridHitInfo downHitInfo) => downHitInfo.HitTest == GridHitTest.RowIndicator;

      private bool hitInRowCell(GridHitInfo hitInfo) => hitInfo.RowHandle >= 0 && hitInfo.HitTest == GridHitTest.RowCell;

      private bool cursorHasMoved(Point cursorLocation) => !calculateDragRectangle().Contains(cursorLocation);

      private Rectangle calculateDragRectangle()
      {
         var dragSize = SystemInformation.DragSize;
         return new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
                                        _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);
      }

      private bool hitInColorCell(GridHitInfo hitInfo)
      {
         return hitInRowCell(hitInfo) && hitInfo.Column == _colorColumn.XtraColumn;
      }

      private bool hitInXYDataCell(GridHitInfo hitInfo)
      {
         return hitInRowCell(hitInfo) && (hitInXColumn(hitInfo) || hitInYColumn(hitInfo));
      }

      private void gridDragOver(DragEventArgs e)
      {
         var targetPoint = PointToClient(new Point(e.X, e.Y));
         var hitInfo = gridView.CalcHitInfo(targetPoint);

         if (e.TypeBeingDraggedIs<List<DataColumn>>())
            e.Effect = DragDropEffects.Move;

         else if (e.TypeBeingDraggedIs<Color>() && hitInColorCell(hitInfo))
            e.Effect = DragDropEffects.Copy;

         else if (e.TypeBeingDraggedIs<CurveDTO>() && hitInRowIndicator(hitInfo))
            e.Effect = DragDropEffects.Move;

         else
            e.Effect = DragDropEffects.None;
      }

      private void gridDragDrop(DragEventArgs e)
      {
         // Retrieve the client coordinates of the mouse position.
         var targetPoint = PointToClient(new Point(e.X, e.Y));
         var hitInfo = gridView.CalcHitInfo(targetPoint);

         var columns = e.Data<List<DataColumn>>();
         if (columns != null)
            dropColumns(hitInfo, columns);

         else if (e.TypeBeingDraggedIs<Color>())
         {
            var color = e.Data<Color>();
            dropColor(hitInfo, color);
         }
         else if (e.TypeBeingDraggedIs<CurveDTO>())
         {
            dropCurve(hitInfo, e.Data<CurveDTO>());
         }
      }

      private void dropColumns(GridHitInfo hitInfo, IReadOnlyList<DataColumn> columns)
      {
         if (hitInXYDataCell(hitInfo) && columns.Count == 1)
         {
            var curve = _gridBinderCurves.ElementAt(hitInfo.RowHandle);
            var column = columns[0];

            if (hitInXColumn(hitInfo))
               _presenter.SetCurveXData(curve, column);

            else if (hitInYColumn(hitInfo))
               _presenter.SetCurveYData(curve, column);

         }
         else if (hitInfo.HitTest == GridHitTest.EmptyRow)
         {
            _presenter.AddCurvesForColumns(columns);
         }
      }

      private bool hitInYColumn(GridHitInfo hitInfo) => Equals(hitInfo.Column, _yDataColumn.XtraColumn);

      private bool hitInXColumn(GridHitInfo hitInfo) => Equals(hitInfo.Column, _xDataColumn.XtraColumn);

      private void dropCurve(GridHitInfo hitInfo, CurveDTO curveBeingMoved)
      {
         if (!hitInRowIndicator(hitInfo))
            return;

         var targetCurveDTO = _gridBinderCurves.ElementAt(hitInfo.RowHandle);
         _presenter.MoveCurvesInLegend(curveBeingMoved, targetCurveDTO);
      }

      private void dropColor(GridHitInfo hitInfo, Color color)
      {
         if (!hitInColorCell(hitInfo))
            return;

         _presenter.UpdateCurveColor(_gridBinderCurves.ElementAt(hitInfo.RowHandle), color);
      }

      private void gridProcessGridKey(KeyEventArgs e)
      {
         if (e.KeyCode != Keys.Delete)
            return;

         var curve = _gridBinderCurves.FocusedElement;
         if (curve == null || gridView.ActiveEditor != null)
            return;

         _presenter.Remove(curve);
      }
   }
}