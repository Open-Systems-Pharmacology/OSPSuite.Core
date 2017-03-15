using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Skins;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationMatrixView : BaseUserControl, IParameterIdentificationMatrixView
   {
      private const int NUMBER_OF_LEGEND_CELLS = 19;
      private const int FIRST_CELL = 0;
      private const int LAST_CELL = NUMBER_OF_LEGEND_CELLS - 1;
      private const int QUARTER_CELL = (NUMBER_OF_LEGEND_CELLS + 1) / 4 - 1;
      private const int HALF_CELL = (NUMBER_OF_LEGEND_CELLS + 1) / 2 - 1;
      private const int THREE_QUARTER_CELL = 3 * ((NUMBER_OF_LEGEND_CELLS + 1) / 4) - 1;

      private double _maxValue;
      private IParameterIdentificationMatrixPresenter _presenter;
      private readonly Color _notCalculatedColor = Color.LightGray;
      public IFormatter<double> NumberFormatter { get; set; } = new DoubleFormatter();

      public ParameterIdentificationMatrixView()
      {
         InitializeComponent();
         initializeGridView(matrixGridView);
         initializeMatrixView();
         initializeGridView(legendGridView);
         initializeLegendView();

         _maxValue = 1;
         layoutItemLegendGrid.TextVisible = false;
         layoutItemMatrixGrid.TextVisible = false;
      }

      private void matrixGridLoading()
      {
         matrixGridView.PopulateColumns();
         matrixGridView.BestFitColumns();

         matrixGridView.ShowRowIndicator = false;
         if (!matrixGridView.Columns.Any())
            return;

         var anchorColumn = matrixGridView.Columns[0];
         anchorColumn.Fixed = FixedStyle.Left;
         anchorColumn.Caption = Captions.EmptyColumn;

         setBestColumnWidth();
      }

      private int calculateAverageColumnWidth(IEnumerable<GridColumn> columns)
      {
         return (int) columns.Average(column => column.VisibleWidth);
      }

      private int calculateMaximumColumnBestWidth(IEnumerable<GridColumn> columns)
      {
         var result = 0;

         columns.Each(column => result = Math.Max(result, matrixGridView.CalcColumnBestWidth(column)));
         return result;
      }

      private void initializeMatrixView()
      {
         matrixGridView.RowCellStyle += (o, e) => OnEvent(() => onMatrixRowCellStyle(e));
         matrixGridView.RowHeight = 30;
         matrixGridView.OptionsView.ColumnAutoWidth = false;
         matrixGridView.CustomDrawEmptyForeground += (o, e) => OnEvent(addMessageInEmptyArea, e);
         matrixGridControl.Load += (o, e) => OnEvent(matrixGridLoading);
         matrixGridView.CustomDrawCell += (o, e) => OnEvent(() => drawCell(o, e));
         matrixGridControl.Resize += (o, e) => OnEvent(gridResized);
      }

      private void gridResized()
      {
         setBestColumnWidth();
      }

      private void setBestColumnWidth()
      {
         if (!matrixGridView.Columns.Any())
            return;

         var columnsExceptAnchorColumn = matrixGridView.Columns.Except(new[] {matrixGridView.Columns[0]}).ToList();

         var bestWidth = calculateMaximumColumnBestWidth(columnsExceptAnchorColumn);

         columnsExceptAnchorColumn.Each(column => column.Width = 100);

         matrixGridView.OptionsView.ColumnAutoWidth = true;

         var averageWidth = calculateAverageColumnWidth(columnsExceptAnchorColumn);

         if (bestWidth < averageWidth)
         {
            matrixGridView.OptionsView.ColumnAutoWidth = false;
            columnsExceptAnchorColumn.Each(x => x.Width = bestWidth);
         }
      }

      private void drawCell(object sender, RowCellCustomDrawEventArgs e)
      {
         if (!Equals(e.Column, matrixGridView.Columns[0])) return;
         drawLikeRowIndicator(sender, e);
      }

      private static void drawLikeRowIndicator(object sender, RowCellCustomDrawEventArgs e)
      {
         var boundaryRectangle = e.Bounds;
         boundaryRectangle.Inflate(2, 2);
         var gridSkinElementsPainter = new GridSkinElementsPainter(sender as GridView);
         var headerObjectInfoArgs = new HeaderObjectInfoArgs();
         headerObjectInfoArgs.Assign(new ObjectInfoArgs(e.Cache, boundaryRectangle, ObjectState.Normal));
         headerObjectInfoArgs.Graphics = e.Graphics;
         gridSkinElementsPainter.Column.DrawObject(headerObjectInfoArgs);

         e.Graphics.SetClip(boundaryRectangle);
         e.Graphics.ResetClip();
      }

      private void setDisplayTextforColumn(CustomColumnDisplayTextEventArgs e)
      {
         double doubleValue;
         if (double.TryParse(e.Value.ToString(), out doubleValue))
            e.DisplayText = NumberFormatter.Format(doubleValue);
      }

      private void onLegendRowCellStyle(RowCellStyleEventArgs e)
      {
         applyGradientColoring(e);
         setColumnHorizontalAlignment(e);
      }

      private static void setColumnHorizontalAlignment(RowCellStyleEventArgs e)
      {
         if (e.Column.AbsoluteIndex > 0)
            e.Appearance.TextOptions.HAlignment = HorzAlignment.Far;
      }

      private void applyGradientColoring(RowCellStyleEventArgs e)
      {
         e.Appearance.BackColor = getColor(getStartValueForCell(e.Column.AbsoluteIndex));
         e.Appearance.BackColor2 = getColor(getStartValueForCell(e.Column.AbsoluteIndex + 1));
         e.Appearance.GradientMode = LinearGradientMode.Horizontal;
      }

      private double getStartValueForCell(int columnIndex)
      {
         return (-1.0 + columnIndex / CellRange) * _maxValue;
      }

      public double CellRange => (NUMBER_OF_LEGEND_CELLS + 1) / 2.0;

      private void initializeLegendView()
      {
         legendGridView.OptionsView.ShowHorizontalLines = DefaultBoolean.False;
         legendGridView.OptionsView.ShowVerticalLines = DefaultBoolean.False;
         legendGridView.ShowColumnHeaders = false;
         legendGridView.CustomDrawCell += (o, e) => OnEvent(() => customDrawLegendCell(e));
         legendGridView.RowCellStyle += (o, e) => OnEvent(() => onLegendRowCellStyle(e));
         legendGridView.ShowRowIndicator = false;
      }

      private void initializeGridView(UxGridView gridView)
      {
         gridView.ShowColumnChooser = false;
         gridView.AllowsFiltering = false;
         gridView.OptionsBehavior.ReadOnly = true;
         gridView.CustomColumnDisplayText += (o, e) => OnEvent(() => setDisplayTextforColumn(e));
      }

      private void customDrawLegendCell(RowCellCustomDrawEventArgs e)
      {
         if (!isMarkerColumn(e.Column.AbsoluteIndex))
            return;

         drawMarker(e);
      }

      private static bool isMarkerColumn(int columnAbsoluteIndex)
      {
         return columnAbsoluteIndex.IsOneOf(QUARTER_CELL, HALF_CELL, THREE_QUARTER_CELL);
      }

      private static void drawMarker(RowCellCustomDrawEventArgs e)
      {
         if (e.RowHandle != 0)
            return;

         var brush = Brushes.Black;
         e.Graphics.DrawLine(new Pen(brush), e.Bounds.Right, e.Bounds.Y, e.Bounds.Right, e.Bounds.Y + e.Bounds.Height);
      }

      private static Color calculateColor(double value, int alpha)
      {
         if (value < 0)
            return Color.FromArgb(alpha, Colors.NegativeCorrelation);

         if (value > 0)
            return Color.FromArgb(alpha, Colors.PositiveCorrelation);

         return Color.White;
      }

      private int calculateAlphaChannel(double value)
      {
         const double fadeLowValues = 3.0;
         var absoluteRatio = Math.Abs(value / _maxValue);

         // calculate saturation/alpha value not linear, because only values near to max, -max are important
         var fadedAbsoluteRatio = Math.Pow(absoluteRatio, fadeLowValues);

         return fadedAbsoluteRatio > 1.0 ? 255 : Convert.ToInt32(fadedAbsoluteRatio * 255);
      }

      private Color getColor(double value)
      {
         if (double.IsNaN(value))
            return _notCalculatedColor;

         try
         {
            var alpha = calculateAlphaChannel(value);
            return calculateColor(value, alpha);
         }
         catch (OverflowException)
         {
            return _notCalculatedColor;
         }
      }

      private static bool columnIsHeaderColumn(GridColumn gridColumn)
      {
         return gridColumn.AbsoluteIndex == 0;
      }

      private void onMatrixRowCellStyle(RowCellStyleEventArgs e)
      {
         var gridColumn = e.Column;
         if (columnIsHeaderColumn(gridColumn)) return;

         var value = Convert.ToDouble(e.CellValue);
         e.Appearance.BackColor = getColor(value);
      }

      public void BindTo(DataTable dataTable, double maxValue)
      {
         _maxValue = maxValue;
         matrixGridControl.DataSource = dataTable;
         refreshLegend();
      }

      private void refreshLegend()
      {
         var legendDataTable = new DataTable();

         for (var i = 0; i < NUMBER_OF_LEGEND_CELLS; i++)
         {
            legendDataTable.AddColumn(i.ToString());
         }

         var row = legendDataTable.NewRow();
         row[FIRST_CELL] = -_maxValue;
         row[QUARTER_CELL] = -_maxValue / 2;
         row[HALF_CELL] = 0;
         row[THREE_QUARTER_CELL] = _maxValue / 2;
         row[LAST_CELL] = _maxValue;
         legendDataTable.Rows.Add(row);

         legendGridControl.DataSource = legendDataTable;
      }

      public void DeleteBinding()
      {
         matrixGridControl.DataSource = null;
         legendGridControl.DataSource = null;
         matrixGridView.PopulateColumns();
      }


      private void addMessageInEmptyArea(CustomDrawEventArgs e)
      {
         matrixGridView.AddMessageInEmptyArea(e, _presenter.NotificationMessage);
      }

      public void AttachPresenter(IParameterIdentificationMatrixPresenter presenter)
      {
         _presenter = presenter;
      }
   }
}