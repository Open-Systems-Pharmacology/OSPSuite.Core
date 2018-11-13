using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class SensitivityAnalysisPKParameterAnalysisView : BaseUserControl, ISensitivityAnalysisPKParameterAnalysisView
   {
      private ISensitivityAnalysisPKParameterAnalysisPresenter _presenter;
      private readonly ScreenBinder<ISensitivityAnalysisPKParameterAnalysisPresenter> _screenBinder;
      private Series _series;
      private XYDiagram xyDiagram => chartControl.Diagram as XYDiagram;

      public SensitivityAnalysisPKParameterAnalysisView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ISensitivityAnalysisPKParameterAnalysisPresenter>();
         chartControl.Images = imageListRetriever.AllImages16x16;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.ActivePKParameter)
            .To(cbPKParameter)
            .WithValues(x => x.AllPKParameters)
            .AndDisplays(pkParameterName => _presenter.DisplayValueForPKParameter(pkParameterName));

         _screenBinder.Bind(x => x.ActiveOutput)
            .To(cbOutputPathSelection)
            .WithValues(x => x.AllOutputPaths);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.PKParameterSensitivityAnalysis;
         pKParameterSelectionLayoutItem.Text = Captions.SensitivityAnalysis.PKParameter.FormatForLabel(checkCase: false);
         outputPathSelectionLayoutItem.Text = Captions.SensitivityAnalysis.OutputPath.FormatForLabel(checkCase: false);
         chartLayoutControlItem.TextVisible = false;
         initializeSensitivityCalculationLabel();

         lblSensitivityNotCalculated.Text = Captions.SensitivityAnalysis.SensitivityAnalysisCouldNotBeCalculated;
         lblSensitivityNotCalculated.AutoSizeMode = LabelAutoSizeMode.Vertical;
         lblSensitivityNotCalculated.AsDescription();
         lblSensitivityNotCalculated.BackColor = Color.Transparent;
         lblSensitivityNotCalculated.Left = 200;
         lblSensitivityNotCalculated.Font = new Font(lblSensitivityNotCalculated.Font.Name, 15.0f);

         initializeChartControlSeries();

         var view = _series.View as PointSeriesView;
         if (view == null) return;

         initializeSeriesView(view);

         chartControl.Legend.LegendPosition(LegendPositions.RightInside);
         configureXAxisGridLines();

         configureYAxisGridLines();
      }

      private void initializeSensitivityCalculationLabel()
      {
         labelLayoutControlItem.TextVisible = false;
         labelLayoutControlItem.SizeConstraintsType = SizeConstraintsType.Custom;
         labelLayoutControlItem.ControlMaxSize = new Size(0, 0);
         labelLayoutControlItem.FillControlToClientArea = false;
         lblSensitivityNotCalculated.MaximumSize = new Size(400, 0);
         labelLayoutControlItem.ControlAlignment = ContentAlignment.MiddleCenter;
         showLabel(false);
      }

      private static void initializeSeriesView(PointSeriesView view)
      {
         view.PointMarkerOptions.Kind = MarkerKind.Circle;
         view.PointMarkerOptions.Size = 10;
      }

      private void initializeChartControlSeries()
      {
         chartControl.Series.Clear();
         _series = new Series("analysis", ViewType.Point)
         {
            ShowInLegend = true,
            LabelsVisibility = DefaultBoolean.False,
            CrosshairEnabled = DefaultBoolean.False,
            ToolTipPointPattern = "{HINT}"
         };

         chartControl.Series.Add(_series);
         chartControl.ToolTipEnabled = DefaultBoolean.True;
      }

      private void configureYAxisGridLines()
      {
         xyDiagram.AxisY.Tickmarks.MinorVisible = false;
         xyDiagram.AxisY.Label.ResolveOverlappingOptions.AllowHide = false;
      }

      private void configureXAxisGridLines()
      {
         xyDiagram.AxisX.GridLines.Visible = true;
         xyDiagram.AxisX.GridLines.LineStyle.Thickness = 2;
         xyDiagram.AxisX.GridLines.MinorVisible = true;
         xyDiagram.AxisX.GridLines.MinorLineStyle.DashStyle = DashStyle.Dash;
         xyDiagram.AxisX.GridLines.MinorLineStyle.Thickness = 2;
         xyDiagram.AxisX.MinorCount = 1;
      }

      public void AttachPresenter(ISensitivityAnalysisPKParameterAnalysisPresenter presenter)
      {
         _presenter = presenter;
         chartControl.AddCopyToClipboardPopupMenu(presenter);
      }

      public void BindTo(SensitivityAnalysisPKParameterAnalysisPresenter sensitivityAnalysisPKParameterAnalysisPresenter)
      {
         _screenBinder.BindToSource(sensitivityAnalysisPKParameterAnalysisPresenter);
      }

      public void UpdateChart(IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivities, string pkParameterName)
      {
         updateLegend(pkParameterName);
         clearChart();
         addPointsToSeries(allPKParameterSensitivities);

         updateYAxisRanges();

         if (!allPKParameterSensitivities.Any())
         {
            showLabel(true);
            return;
         }
         showLabel(false);
         updateXAxisRanges(allPKParameterSensitivities);
      }

      private void showLabel(bool showLabel)
      {
         labelLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(showLabel);
         chartLayoutControlItem.Visibility = LayoutVisibilityConvertor.FromBoolean(!showLabel);
      }

      private void updateXAxisRanges(IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivities)
      {
         var xMax = allPKParameterSensitivities.Max(x => Math.Abs(x.Value));
         var xMaxInt = Math.Ceiling(xMax);
         xyDiagram.AxisX.WholeRange.SetMinMaxValues(-xMaxInt, xMaxInt);
         xyDiagram.AxisX.VisualRange.SetMinMaxValues(-xMaxInt, xMaxInt);

         xyDiagram.AxisX.NumericScaleOptions.AutoGrid = false;
         xyDiagram.AxisX.NumericScaleOptions.GridSpacing = Math.Max(Math.Round(xMax) * 2 / 4, 1.0);
      }

      private void updateYAxisRanges()
      {
         xyDiagram.AxisY.WholeRange.SetMinMaxValues(-1, _series.Points.Count);
         xyDiagram.AxisY.VisualRange.SetMinMaxValues(-1, _series.Points.Count);
      }

      private void addPointsToSeries(IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivities)
      {
         _series.Points.AddRange(mapPointsFromSensitivityParameters(allPKParameterSensitivities));
      }

      private void updateLegend(string pkParameterSelection)
      {
         _series.Name = pkParameterSelection;
      }

      private void clearChart()
      {
         _series.Points.Clear();
         xyDiagram.AxisY.CustomLabels.Clear();
      }

      private SeriesPoint[] mapPointsFromSensitivityParameters(IReadOnlyList<PKParameterSensitivity> allPKParameterSensitivities)
      {
         var points = new List<SeriesPoint>();

         allPKParameterSensitivities.Each((x, i) =>
         {
            // -1 so that the lowest sensitivity is yValue = 0
            var yValue = allPKParameterSensitivities.Count - i - 1;
            var seriesPoint = new SeriesPoint(x.Value, yValue) {ToolTipHint = createPointLabel(x.Value, x.ParameterName)};
            points.Add(seriesPoint);
            xyDiagram.AxisY.CustomLabels.Add(new CustomAxisLabel(x.ParameterName) {AxisValue = yValue});
         });

         return points.ToArray();
      }

      public void CopyToClipboard(string watermark)
      {
         chartControl.CopyToClipboard(watermark);
      }

      private string createPointLabel(double value, string parameterName) => $"{parameterName}: {value}";

   }
}