using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation
{
   internal abstract class concern_for_CurveSettingsPresenter : ContextSpecification<CurveSettingsPresenter>
   {
      protected IDimensionFactory _dimensionFactory;
      private ICurveSettingsView _view;
      protected ICurveChart _chart;

      protected override void Context()
      {
         _view = A.Fake<ICurveSettingsView>();
         _dimensionFactory = DimensionFactoryForSpecs.Factory;
         _chart = new CurveChart().WithAxes();
         
         sut = new CurveSettingsPresenter(_view, _dimensionFactory);
         sut.SetDatasource(_chart);

         var dataColumn = new DataColumn
         {
            Id = "id",
            BaseGrid = new BaseGrid("time", DimensionFactoryForSpecs.TimeDimension),
            Dimension = DimensionFactoryForSpecs.ConcentrationDimension
         };

         dataColumn.DataInfo.Origin = ColumnOrigins.Observation;

         sut.DataColumns.Add(dataColumn);

         _chart.Axes.Each(axis => axis.DefaultLineStyle = LineStyles.None);
      }
   }

   internal class When_adding_a_curve_for_a_column_already_in_chart_and_default_settings_are_specified : concern_for_CurveSettingsPresenter
   {
      private CurveOptions _defaultCurveOptions;
      private CurveOptions _firstPlotCurveOptions;
      private ICurve _curve;

      protected override void Context()
      {
         base.Context();
         _firstPlotCurveOptions = new CurveOptions
         {
            Color = Color.Black,
            LineThickness = 1,
            VisibleInLegend = true
         };
         
         _defaultCurveOptions = new CurveOptions
         {
            Color = Color.Fuchsia,
            LineThickness = 2,
            VisibleInLegend = false
         };

         sut.AddCurveForColumn("id", _firstPlotCurveOptions);
      }

      protected override void Because()
      {
         _curve = sut.AddCurveForColumn("id", _defaultCurveOptions);
      }

      [Observation]
      public void the_curve_options_should_not_have_been_updated_to_the_second_specified_values()
      {
         _curve.CurveOptions.Color.ShouldBeEqualTo(Color.Black);
         _curve.CurveOptions.LineThickness.ShouldBeEqualTo(1);
         _curve.CurveOptions.VisibleInLegend.ShouldBeEqualTo(true);
      }
   }

   internal class When_adding_a_new_curve_for_column_id_not_in_chart_and_default_settings_are_specified : concern_for_CurveSettingsPresenter
   {
      private CurveOptions _defaultCurveOptions;
      private ICurve _newCurve;

      protected override void Context()
      {
         base.Context();
         _defaultCurveOptions = new CurveOptions
         {
            Color = Color.Fuchsia,
            LineThickness = 2,
            VisibleInLegend = false,
            Symbol = Symbols.Diamond,
            LineStyle = LineStyles.DashDot
         };
      }

      protected override void Because()
      {
         _newCurve = sut.AddCurveForColumn("id", _defaultCurveOptions);
      }

      [Observation]
      public void curve_options_should_be_updated_to_defaults()
      {
         _newCurve.CurveOptions.Color.ShouldBeEqualTo(Color.Fuchsia);
         _newCurve.CurveOptions.LineThickness.ShouldBeEqualTo(2);
         _newCurve.CurveOptions.VisibleInLegend.ShouldBeEqualTo(false);
         _newCurve.CurveOptions.Symbol.ShouldBeEqualTo(Symbols.Diamond);
         _newCurve.CurveOptions.LineStyle.ShouldBeEqualTo(LineStyles.DashDot);
      }
   }

   internal class When_adding_a_curve_to_the_chart : concern_for_CurveSettingsPresenter
   {
      private ICurve _newCurve;

      protected override void Because()
      {
         _newCurve = sut.AddCurveForColumn("id");
      }

      [Observation]
      public void should_set_the_display_unit_of_the_underlying_data_to_the_selected_axis_units()
      {
         _newCurve.xData.DisplayUnit.ShouldBeEqualTo(_chart.Axes[AxisTypes.X].Unit);   
         _newCurve.yData.DisplayUnit.ShouldBeEqualTo(_chart.Axes[AxisTypes.Y].Unit);   
      }
   }
}
