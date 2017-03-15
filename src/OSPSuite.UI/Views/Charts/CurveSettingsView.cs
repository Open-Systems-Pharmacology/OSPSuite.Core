using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.UI.Services;

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
      private GridViewBinder<ICurve> _gridBinderCurves;
      private ICurveSettingsPresenter _presenter;

      private GridHitInfo _downHitInfo; //Drag & Drop
      private IGridViewColumn _xDataColumn;
      private IGridViewColumn _yDataColumn;
      private IGridViewColumn _colName;
      private readonly RepositoryItemCheckEdit _showLowerLimitOfQuantificationRepository;

      public CurveSettingsView(IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();

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
         // initialize RepositoryItems
         _interpolationModeRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<InterpolationModes>());
         _interpolationModeRepository.TextEditStyle = TextEditStyles.DisableTextEditor;

         _lineStyleRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<LineStyles>());
         _lineThicknessRepository.FillComboBoxRepositoryWith(new[] {1, 2, 3});
         _symbolRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<Symbols>());
         _axisTypeRepository.FillComboBoxRepositoryWith(EnumHelper.AllValuesFor<AxisTypes>());
         _axisTypeRepository.Items.Remove(AxisTypes.X);

         _gridBinderCurves = new GridViewBinder<ICurve>(gridView) {BindingMode = BindingMode.TwoWay};


         _colName = _gridBinderCurves.AutoBind(curve => curve.Name)
            .WithShowInColumnChooser(true);
         _colName.XtraColumn.Tag = CurveOptionsColumns.Name.ToString();

         _xDataColumn = _gridBinderCurves.AutoBind(curve => curve.xData)
            .WithShowInColumnChooser(true)
            .AsReadOnly();
         _xDataColumn.XtraColumn.Tag = CurveOptionsColumns.xData.ToString();

         _yDataColumn = _gridBinderCurves.AutoBind(curve => curve.yData)
            .WithShowInColumnChooser(true)
            .WithFormat(c => new DataColumnFormatter(_presenter))
            .AsReadOnly();
         _yDataColumn.XtraColumn.Tag = CurveOptionsColumns.yData.ToString();

         _gridBinderCurves.AutoBind(curve => curve.yAxisType)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _axisTypeRepository)
            .WithEditorConfiguration(configureAxisTypeEditor)
            .XtraColumn.Tag = CurveOptionsColumns.yAxisType.ToString();

         _gridBinderCurves.AutoBind(curve => curve.InterpolationMode)
            .WithRepository(curve => _interpolationModeRepository)
            .XtraColumn.Tag = CurveOptionsColumns.InterpolationMode.ToString();

         _gridBinderCurves.AutoBind(curve => curve.Color)
            .WithShowInColumnChooser(true).WithRepository(curve => _colorRepository)
            .XtraColumn.Tag = CurveOptionsColumns.Color.ToString();

         _gridBinderCurves.AutoBind(curve => curve.LineStyle)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _lineStyleRepository)
            .XtraColumn.Tag = CurveOptionsColumns.LineStyle.ToString();

         _gridBinderCurves.AutoBind(curve => curve.Symbol)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _symbolRepository)
            .XtraColumn.Tag = CurveOptionsColumns.Symbol.ToString();

         _gridBinderCurves.AutoBind(curve => curve.LineThickness)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _lineThicknessRepository)
            .XtraColumn.Tag = CurveOptionsColumns.LineThickness.ToString();

         _gridBinderCurves.Bind(curve => curve.Visible)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _visibleRepository)
            .XtraColumn.Tag = CurveOptionsColumns.Visible.ToString();

         _gridBinderCurves.Bind(curve => curve.VisibleInLegend)
            .WithShowInColumnChooser(true)
            .WithRepository(curve => _visibleRepository)
            .XtraColumn.Tag = CurveOptionsColumns.VisibleInLegend.ToString();

         var lloqColumn = _gridBinderCurves.Bind(curve => curve.ShowLLOQ);
         lloqColumn.WithShowInColumnChooser(true)
            .WithRepository(curve => _showLowerLimitOfQuantificationRepository)
            .XtraColumn.Tag = CurveOptionsColumns.ShowLowerLimitOfQuantification.ToString();

         var legendIndexColumn = _gridBinderCurves.Bind(curve => curve.LegendIndex);
         legendIndexColumn.Visible = false;
         lloqColumn.Visible = false;
         gridView.Columns[legendIndexColumn.ColumnName].SortOrder = ColumnSortOrder.Ascending;

         //Delete-column
         _gridBinderCurves.AddUnboundColumn()
            .WithCaption(UIConstants.EMPTY_COLUMN)
            .WithShowButton(ShowButtonModeEnum.ShowAlways)
            .WithRepository(curve => _deleteButtonRepository)
            .WithFixedWidth(UIConstants.Size.EMBEDDED_BUTTON_WIDTH);
      }

      public override void Refresh()
      {
         base.Refresh();
         _gridBinderCurves.Rebind();
      }

      private void onToolTipControllerGetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != gridControl) return;

         var hi = gridView.HitInfoAt(e.ControlMousePosition);
         if (hi == null) return;

         if (!Equals(hi.Column, _colName.XtraColumn))
            return;

         var curve = _gridBinderCurves.ElementAt(e);
         if (curve == null) return;

         var toolTip = _toolTipCreator.CreateToolTip(_presenter.ToolTipFor(curve));
         e.Info = _toolTipCreator.ToolTipControlInfoFor(curve, toolTip);
      }

      private void configureAxisTypeEditor(BaseEdit baseEdit, ICurve curve)
      {
         baseEdit.FillComboBoxEditorWith(_presenter.GetAxisTypes());
      }

      public void AttachPresenter(ICurveSettingsPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

      public void BindToSource(IEnumerable<ICurve> curves)
      {
         _gridBinderCurves.BindToSource(curves);
         Refresh();
      }

      public void RefreshData()
      {
         gridControl.RefreshDataSource();
      }

      private void initializeDragDrop()
      {
         gridControl.AllowDrop = true;
         // for Drag&Drop of Colors
         gridView.MouseDown += viewMouseDown; 
         _colorRepository.MouseDown += viewMouseDown;

         gridView.MouseMove += viewMouseMove;
         _colorRepository.MouseMove += viewMouseMove;

         // for Drop of Colors and (DataColumns to x/yData)
         gridControl.DragOver += (o, e) => OnEvent(gridDragOver, e);
         gridControl.DragDrop += (o, e) => OnEvent(gridDragDrop, e);
         gridControl.ProcessGridKey += (o, e) => OnEvent(gridProcessGridKey, e);
      }

      private void deleteButtonClick(object sender, ButtonPressedEventArgs e)
      {
         var focusedCurve = _gridBinderCurves.FocusedElement;
         if (focusedCurve == null) return;
         _presenter.RemoveCurve(focusedCurve.Id);
      }

      private void viewMouseDown(object sender, MouseEventArgs e)
      {
         this.DoWithinExceptionHandler(() =>
         {
            _downHitInfo = null;
            if (ModifierKeys != Keys.None) return;

            var cursorLocation = e.Location;
            var colorEdit = sender as ColorEdit;
            if (colorEdit != null)
            {
               // convert internal coordinates of color edit to gridview coordinates
               cursorLocation.Offset(colorEdit.Location);
            }

            _downHitInfo = gridView.CalcHitInfo(cursorLocation);
         });
      }

      private void viewMouseMove(object sender, MouseEventArgs e)
      {
         if (_downHitInfo == null) return;

         this.DoWithinExceptionHandler(() =>
         {
            var cursorLocation = getCursorLocationRelativeToGrid(sender, e);

            if (isNotValidMouseMove(e, cursorLocation)) return;

            var curve = _gridBinderCurves.ElementAt(_downHitInfo.RowHandle);
            if (hitInColorCell(_downHitInfo))
            {
               var colorEdit = sender as ColorEdit;
               handleAsDraggingColor(colorEdit, curve);
               DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
            else if (hitInRowIndicator(_downHitInfo))
            {
               handleAsDraggingCurve(curve);
               DXMouseEventArgs.GetMouseArgs(e).Handled = true;
            }
         });
      }

      private bool isNotValidMouseMove(MouseEventArgs e, Point cursorLocation)
      {
         return !cursorHasMoved(cursorLocation) || (e.Button != MouseButtons.Left);
      }

      private void handleAsDraggingCurve(ICurve curve)
      {
         gridControl.DoDragDrop(curve, DragDropEffects.Move);

         _downHitInfo = null;
      }

      private void handleAsDraggingColor(ColorEdit colorEdit, ICurve curve)
      {
         var color = curve.Color;

         if (colorEdit != null)
            colorEdit.ClosePopup();

         gridControl.DoDragDrop(color, DragDropEffects.Copy);

         _downHitInfo = null;
      }

      private static Point getCursorLocationRelativeToGrid(object sender, MouseEventArgs e)
      {
         var cursorLocation = e.Location;
         var colorEdit = sender as ColorEdit;
         if (colorEdit != null)
         {
            // convert internal coordinates of color edit to gridview coordinates
            cursorLocation.Offset(colorEdit.Location);
         }
         return cursorLocation;
      }

      private bool hitInRowIndicator(GridHitInfo downHitInfo)
      {
         return downHitInfo.HitTest == GridHitTest.RowIndicator;
      }

      private bool cursorHasMoved(Point cursorLocation)
      {
         return !calculateDragRectangle().Contains(cursorLocation);
      }

      private Rectangle calculateDragRectangle()
      {
         var dragSize = SystemInformation.DragSize;
         var dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
            _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);
         return dragRect;
      }

      private bool hitInColorCell(GridHitInfo hitInfo)
      {
         return (hitInfo.RowHandle >= 0 && hitInfo.HitTest == GridHitTest.RowCell && hitInfo.Column.Tag as String == CurveOptionsColumns.Color.ToString());
      }

      private bool hitInXYDataCell(GridHitInfo hitInfo)
      {
         return (hitInfo.RowHandle >= 0 && hitInfo.HitTest == GridHitTest.RowCell
                 && (hitInXColumn(hitInfo) || hitInYColumn(hitInfo)));
      }

      private void gridDragOver(DragEventArgs e)
      {
         var targetPoint = PointToClient(new Point(e.X, e.Y));
         var hitInfo = gridView.CalcHitInfo(targetPoint);

         if (e.TypeBeingDraggedIs<List<string>>())
            e.Effect = DragDropEffects.Move;
         else if (e.TypeBeingDraggedIs<Color>() && hitInColorCell(hitInfo))
            e.Effect = DragDropEffects.Copy;
         else if (e.TypeBeingDraggedIs<Curve>() && hitInRowIndicator(hitInfo))
            e.Effect = DragDropEffects.Move;
         else
            e.Effect = DragDropEffects.None;
      }

      private void gridDragDrop(DragEventArgs e)
      {
         // Retrieve the client coordinates of the mouse position.
         var targetPoint = PointToClient(new Point(e.X, e.Y));
         var hitInfo = gridView.CalcHitInfo(targetPoint);

         var columnIdList = e.Data<List<string>>();
         if (columnIdList != null)
            dropColumnIdList(hitInfo, columnIdList);

         else if (e.TypeBeingDraggedIs<Color>())
         {
            var color = e.Data<Color>();
            dropColor(hitInfo, color);
         }
         else if (e.TypeBeingDraggedIs<Curve>())
         {
            dropCurve(hitInfo, e.Data<Curve>());
         }
      }

      private void dropColumnIdList(GridHitInfo hitInfo, IReadOnlyList<string> columnIdList)
      {
         if (hitInXYDataCell(hitInfo) && columnIdList.Count() == 1)
         {
            var curve = _gridBinderCurves.ElementAt(hitInfo.RowHandle);
            var columnId = columnIdList.First();

            if (hitInXColumn(hitInfo))
            {
               _presenter.SetCurveXData(curve, columnId);
            }
            else if (hitInYColumn(hitInfo))
            {
               _presenter.SetCurveYData(curve, columnId);
            }
         }
         else if (hitInfo.HitTest == GridHitTest.EmptyRow)
         {
            foreach (var columnId in columnIdList)
            {
               _presenter.AddCurveForColumn(columnId);
            }
         }
      }

      private bool hitInYColumn(GridHitInfo hitInfo)
      {
         return Equals(hitInfo.Column.Tag, _yDataColumn.XtraColumn.Tag);
      }

      private bool hitInXColumn(GridHitInfo hitInfo)
      {
         return Equals(hitInfo.Column.Tag, _xDataColumn.XtraColumn.Tag);
      }

      private void dropCurve(GridHitInfo hitInfo, ICurve curveBeingMoved)
      {
         if (hitInRowIndicator(hitInfo))
         {
            var targetCurve = _gridBinderCurves.ElementAt(hitInfo.RowHandle);
            _presenter.MoveSeriesInLegend(curveBeingMoved, targetCurve);
         }
      }

      private void dropColor(GridHitInfo hitInfo, Color color)
      {
         if (hitInColorCell(hitInfo))
            _gridBinderCurves.ElementAt(hitInfo.RowHandle).Color = color;
      }

      private void gridProcessGridKey(KeyEventArgs e)
      {
         if (e.KeyCode != Keys.Delete)
            return;

         var curve = _gridBinderCurves.FocusedElement;
         if (curve != null && gridView.ActiveEditor == null)
         {
            _presenter.RemoveCurve(curve.Id);
         }
      }
   }
}