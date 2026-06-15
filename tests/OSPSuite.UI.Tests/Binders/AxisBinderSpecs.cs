using System.Drawing;
using DevExpress.XtraCharts;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
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

   public class When_refreshing_adapter_and_a_linear_axis_has_a_manual_major_interval_and_minor_count : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.MajorInterval = 2F;
         _axis.MinorCount = 3;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void should_disable_the_auto_grid_and_apply_the_major_interval_as_grid_spacing()
      {
         _uxChartControl.XYDiagram.AxisY.NumericScaleOptions.AutoGrid.ShouldBeFalse();
         _uxChartControl.XYDiagram.AxisY.NumericScaleOptions.GridSpacing.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_apply_the_minor_count()
      {
         _uxChartControl.XYDiagram.AxisY.MinorCount.ShouldBeEqualTo(3);
      }
   }

   public class When_refreshing_adapter_and_a_linear_axis_has_no_manual_tick_settings : concern_for_AxisBinder
   {
      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void should_let_the_axis_calculate_the_grid_automatically()
      {
         _uxChartControl.XYDiagram.AxisY.NumericScaleOptions.AutoGrid.ShouldBeTrue();
      }
   }

   public class When_refreshing_adapter_and_a_linear_axis_has_an_out_of_range_minor_count : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.MajorInterval = 2F;
         _axis.MinorCount = 0;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void should_fall_back_to_the_default_minor_count_instead_of_throwing()
      {
         _uxChartControl.XYDiagram.AxisY.MinorCount.ShouldBeEqualTo(4);
      }
   }

   public class When_refreshing_adapter_and_a_logarithmic_axis_has_manual_tick_settings : concern_for_AxisBinder
   {
      protected override void Context()
      {
         base.Context();
         _axis.Scaling = Scalings.Log;
         _axis.MajorInterval = 2F;
         _axis.MinorCount = 3;
      }

      protected override void Because()
      {
         sut.RefreshRange(sideMarginsEnabled: true, diagramSize: new Size(100, 100));
      }

      [Observation]
      public void should_ignore_the_manual_tick_settings_and_keep_the_logarithmic_minor_count()
      {
         _uxChartControl.XYDiagram.AxisY.MinorCount.ShouldBeEqualTo(8);
      }
   }
}
