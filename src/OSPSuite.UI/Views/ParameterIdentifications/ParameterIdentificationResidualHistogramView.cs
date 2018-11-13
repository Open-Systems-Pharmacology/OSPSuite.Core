using System.Data;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraCharts;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationResidualHistogramView : BaseUserControl, IParameterIdentificationResidualHistogramView
   {
      private ICanCopyToClipboard _clipboardManager;

      public ParameterIdentificationResidualHistogramView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         InitializeComponent();
         chart.Initialize(imageListRetriever, toolTipCreator);
      }

      public void BindTo(DataTable gaussData, ContinuousDistributionData distributionData, DistributionSettings settings)
      {
         chart.Series.Clear();
         var gaussSeries = createGaussSeriesFor(gaussData);

         //need to add it before creating the seconday axis. Need to be added BEFORE the distribution to be put in the front
         chart.Series.Add(gaussSeries);

         var secondaryAxis = getOrCreateSecondaryAxis();
         setGaussSeriesOptions(gaussSeries, secondaryAxis);

         chart.Plot(distributionData, settings);

         //need to add it once a again to actually see it
         chart.Series.Add(gaussSeries);
      }

      public ICanCopyToClipboard CopyToClipboardManager
      {
         get => _clipboardManager;
         set
         {
            _clipboardManager = value;
            chart.AddCopyToClipboardPopupMenu(value);
         }
      }

      private static Series createGaussSeriesFor(DataTable gaussData)
      {
         var gaussSeries = new Series("Line Series", ViewType.Line);

         foreach (DataRow row in gaussData.Rows)
         {
            gaussSeries.Points.Add(new SeriesPoint(row.ValueAt<double>(Constants.X), row.ValueAt<double>(Constants.Y)));
         }
         return gaussSeries;
      }

      private SecondaryAxisY getOrCreateSecondaryAxis()
      {
         if (chart.XYDiagram.SecondaryAxesY.Count == 0)
         {
            var secondaryAxis = new SecondaryAxisY("Y2");
            chart.XYDiagram.SecondaryAxesY.Add(secondaryAxis);
         }
         return chart.XYDiagram.SecondaryAxesY[0];
      }

      private void setGaussSeriesOptions(Series gaussSeries, SecondaryAxisY secondaryAxis)
      {
         var lineSeriesView = gaussSeries.View.DowncastTo<LineSeriesView>();
         lineSeriesView.Color = Color.Black;
         lineSeriesView.AxisY = secondaryAxis;
      }

      public void CopyToClipboard(string watermark)
      {
         chart.CopyToClipboard(watermark);
      }

   }
}