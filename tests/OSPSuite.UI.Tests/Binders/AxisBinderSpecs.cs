using System.Drawing;
using DevExpress.XtraCharts;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Format;
using Axis = OSPSuite.Core.Chart.Axis;

namespace OSPSuite.UI.Binders
{
   public abstract class concern_for_AxisBinder : ContextSpecification<AxisBinder>
   {
      protected UxChartControl _uxChartControl;
      protected Axis _axis;
      private Series _series;

      protected override void Context()
      {
         base.Context();
         _uxChartControl = new UxChartControl();
         _series = new Series("dummySeries", ViewType.ScatterLine);
         _uxChartControl.Series.Add(_series);
         _axis = new Axis(AxisTypes.Y);
         _uxChartControl.XYDiagram.AxisY.VisualRange.Auto = true;
         _uxChartControl.XYDiagram.AxisY.WholeRange.Auto = true;
         sut = new AxisBinder(_axis, _uxChartControl, new NumericFormatterOptions());
      }

      protected float RangeMax()
      {
         return System.Convert.ToSingle(_uxChartControl.XYDiagram.AxisY.WholeRange.MaxValue);
      }

      protected float RangeMin()
      {
         return System.Convert.ToSingle(_uxChartControl.XYDiagram.AxisY.WholeRange.MinValue);
      }
   }

   public class When_refreshing_adapter_and_an_axis_has_max_set_within_series_range : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.Max = 1.5F;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void the_min_should_be_the_range_min()
      {
         _axis.Min.ShouldBeEqualTo(RangeMin());
      }
   }

   public class When_refreshing_adapter_and_an_axis_has_min_set_within_series_range : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.Min = 1.5F;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void the_max_should_be_the_range_max()
      {
         _axis.Max.ShouldBeEqualTo(RangeMax());
      }
   }

   public class When_refreshing_adapter_and_an_axis_has_min_set_higher_than_max_of_series : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.Min = RangeMax() + 1.0F;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void the_min_should_be_set_to_equal_the_max()
      {
         _axis.Max.ShouldBeEqualTo(_axis.Min);
      }
   }

   public class When_refreshing_adapter_and_an_axis_has_max_set_below_min_of_series : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.Max = RangeMin() - 1.0F;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled:true, diagramSize:new Size(100,100));
      }

      [Observation]
      public void the_min_should_be_set_to_equal_the_max()
      {
         _axis.Min.ShouldBeEqualTo(_axis.Max);
      }
   }
}
