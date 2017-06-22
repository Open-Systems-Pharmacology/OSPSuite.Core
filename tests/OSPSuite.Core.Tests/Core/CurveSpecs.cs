using System.Drawing;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_Curve : ContextSpecification<Curve>
   {
      protected IDimensionFactory _dimensionFactory;
      private DataRepository _repository;
      protected DataColumn _timeColumn;
      protected DataColumn _dataColumn;

      protected override void Context()
      {
         sut = new Curve();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _repository = DomainHelperForSpecs.ObservedData();
         _timeColumn = _repository.BaseGrid;
         _dataColumn = _repository.FirstDataColumn();
      }
   }

   public class When_cloning_a_curve : concern_for_Curve
   {
      private Curve _clone;

      protected override void Context()
      {
         base.Context();
         sut.Name = "Toto";
         sut.Description = "Hello";
         sut.SetxData(_timeColumn, _dimensionFactory);
         sut.SetyData(_dataColumn, _dimensionFactory);
         sut.CurveOptions.UpdateFrom(new CurveOptions
         {
            LegendIndex = 5,
            Symbol = Symbols.Diamond,
            VisibleInLegend = true,
            LineThickness = 4,
            LineStyle = LineStyles.DashDot,
            yAxisType = AxisTypes.Y2,
            Color = Color.Aquamarine,
            InterpolationMode = InterpolationModes.xLinear,
            Visible = true,
            ShouldShowLLOQ = false
         });
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_return_a_curve_with_all_identical_properties_except_the_id()
      {
         _clone.Name.ShouldBeEqualTo(sut.Name);
         _clone.Description.ShouldBeEqualTo(sut.Description);
         _clone.xData.ShouldBeEqualTo(sut.xData);
         _clone.yData.ShouldBeEqualTo(sut.yData);
         _clone.xDimension.ShouldBeEqualTo(sut.xDimension);
         _clone.yDimension.ShouldBeEqualTo(sut.yDimension);
         _clone.InterpolationMode.ShouldBeEqualTo(sut.InterpolationMode);
         _clone.yAxisType.ShouldBeEqualTo(sut.yAxisType);
         _clone.Visible.ShouldBeEqualTo(sut.Visible);
         _clone.Color.ShouldBeEqualTo(sut.Color);
         _clone.LineStyle.ShouldBeEqualTo(sut.LineStyle);
         _clone.LineThickness.ShouldBeEqualTo(sut.LineThickness);
         _clone.ShowLLOQ.ShouldBeEqualTo(sut.ShowLLOQ);
         _clone.LegendIndex.ShouldBeEqualTo(sut.LegendIndex);
         _clone.VisibleInLegend.ShouldBeEqualTo(sut.VisibleInLegend);
         _clone.Symbol.ShouldBeEqualTo(sut.Symbol);
      }
   }

   public class When_setting_the_xData : concern_for_Curve
   {
      private IDimension _mergedXDimension;

      protected override void Context()
      {
         base.Context();
         _mergedXDimension = A.Fake<IDimension>();
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_timeColumn)).Returns(_mergedXDimension);
      }

      protected override void Because()
      {
         sut.SetxData(_timeColumn, _dimensionFactory);
      }

      [Observation]
      public void should_set_the_xdata()
      {
         sut.xData.ShouldBeEqualTo(_timeColumn);
      }

      [Observation]
      public void should_also_update_the_xdimension_using_the_corresponding_merge_dimension_in_the_dimension_factory()
      {
         sut.xDimension.ShouldBeEqualTo(_mergedXDimension);
      }
   }

   public class When_setting_the_yData : concern_for_Curve
   {
      private IDimension _mergedYDimension;

      protected override void Context()
      {
         base.Context();
         _mergedYDimension = A.Fake<IDimension>();
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_dataColumn)).Returns(_mergedYDimension);
      }

      protected override void Because()
      {
         sut.SetyData(_dataColumn, _dimensionFactory);
      }

      [Observation]
      public void should_set_the_xdata()
      {
         sut.yData.ShouldBeEqualTo(_dataColumn);
      }

      [Observation]
      public void should_also_update_the_ydimension_using_the_corresponding_merge_dimension_in_the_dimension_factory()
      {
         sut.yDimension.ShouldBeEqualTo(_mergedYDimension);
      }
   }

   public class When_setting_the_yData_using_a_column_containg_a_related_column_for_geometric_mean_pop : concern_for_Curve
   {
      private IDimension _mergedYDimension;
      private DataColumn _geomMeanPop;

      protected override void Context()
      {
         base.Context();
         _mergedYDimension = A.Fake<IDimension>();
         _geomMeanPop = new DataColumn("GeoMeanPop", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _dataColumn.BaseGrid)
         {
            DataInfo = {AuxiliaryType = AuxiliaryType.GeometricMeanPop}
         };
         _dataColumn.AddRelatedColumn(_geomMeanPop);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_geomMeanPop)).Returns(_mergedYDimension);
      }

      protected override void Because()
      {
         sut.SetyData(_dataColumn, _dimensionFactory);
      }

      [Observation]
      public void should_set_the_xdata()
      {
         sut.yData.ShouldBeEqualTo(_dataColumn);
      }

      [Observation]
      public void should_also_update_the_ydimension_using_the_corresponding_merge_dimension_in_the_dimension_factory()
      {
         sut.yDimension.ShouldBeEqualTo(_mergedYDimension);
      }
   }

   public class When_setting_the_yData_using_a_column_containg_a_related_column_for_arithmetic_mean_pop : concern_for_Curve
   {
      private IDimension _mergedYDimension;
      private DataColumn _geomMeanPop;

      protected override void Context()
      {
         base.Context();
         _mergedYDimension = A.Fake<IDimension>();
         _geomMeanPop = new DataColumn("ArithmeticMeanPop", _dataColumn.Dimension, _dataColumn.BaseGrid)
         {
            DataInfo = { AuxiliaryType = AuxiliaryType.ArithmeticMeanPop }
         };
         _dataColumn.AddRelatedColumn(_geomMeanPop);
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_geomMeanPop)).Returns(_mergedYDimension);
      }

      protected override void Because()
      {
         sut.SetyData(_dataColumn, _dimensionFactory);
      }

      [Observation]
      public void should_set_the_xdata()
      {
         sut.yData.ShouldBeEqualTo(_dataColumn);
      }

      [Observation]
      public void should_also_update_the_ydimension_using_the_corresponding_merge_dimension_in_the_dimension_factory()
      {
         sut.yDimension.ShouldBeEqualTo(_mergedYDimension);
      }
   }

}