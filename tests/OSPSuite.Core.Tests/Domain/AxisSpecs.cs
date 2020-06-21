using System.Drawing;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Helpers;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_Axis : ContextSpecification<Axis>
   {

   }

   public class When_validating_axis_minimum_and_maximum : concern_for_Axis
   {
      [Observation]
      public void invalid_for_max_less_than_min()
      {
         new Axis(AxisTypes.X)
         {
            Min = 0,
            Max = -1
         }.IsValid().ShouldBeFalse();
      }

      [Observation]
      public void invalid_for_log_max_equal_to_0()
      {
         new Axis(AxisTypes.X)
         {
            Scaling = Scalings.Log,
            Min = 0,
            Max = 0
         }.IsValid().ShouldBeFalse();
      }

   }

   public class When_updating_an_axis_properties_from_another : concern_for_Axis
   {
      private Axis _sourceAxis;

      protected override void Context()
      {
         base.Context();
         sut = new Axis(AxisTypes.X);
         _sourceAxis = new Axis(AxisTypes.X)
         {
            Caption = "A Caption",
            Scaling = Scalings.Log,
            NumberMode = NumberModes.Scientific,
            Dimension = DomainHelperForSpecs.TimeDimensionForSpecs(),
            GridLines = false,
            Min = -1,
            Max = 300,
            DefaultLineStyle = LineStyles.DashDot,
            DefaultColor = Color.Azure,
            Visible = false
         };


      }

      protected override void Because()
      {
         sut.UpdateFrom(_sourceAxis);
      }

      [Observation]
      public void the_properties_from_the_source_should_match_the_target_of_update()
      {
         _sourceAxis.Caption.ShouldBeEqualTo(sut.Caption);
         _sourceAxis.Scaling.ShouldBeEqualTo(sut.Scaling);
         _sourceAxis.NumberMode.ShouldBeEqualTo(sut.NumberMode);
         _sourceAxis.Dimension.ShouldBeEqualTo(sut.Dimension);
         _sourceAxis.UnitName.ShouldBeEqualTo(sut.UnitName);
         _sourceAxis.GridLines.ShouldBeEqualTo(sut.GridLines);
         _sourceAxis.Min.ShouldBeEqualTo(sut.Min);
         _sourceAxis.Max.ShouldBeEqualTo(sut.Max);
         _sourceAxis.DefaultLineStyle.ShouldBeEqualTo(sut.DefaultLineStyle);
         _sourceAxis.DefaultColor.ShouldBeEqualTo(sut.DefaultColor);
         _sourceAxis.Visible.ShouldBeEqualTo(sut.Visible);
      }
   }

   public class When_instantiating_a_new_axis : concern_for_Axis
   {
      protected override void Context()
      {
         base.Context();
         sut = new Axis(AxisTypes.X);
      }

      [Observation]
      public void the_axis_should_be_visible_by_default()
      {
         sut.Visible.ShouldBeTrue();
      }
   }

   public class When_testing_axis_types_for_X : concern_for_Axis
   {
      protected override void Context()
      {
         sut = new Axis(AxisTypes.X);
      }

      [Observation]
      public void test_for_type_should_be_true()
      {
         sut.IsYAxis.ShouldBeFalse();
      }
   }

   public abstract class When_testing_axis_types_for_Y : concern_for_Axis
   {
      protected AxisTypes _axisType;
      protected override void Context()
      {
         sut = new Axis(_axisType);
      }

      [Observation]
      public void test_for_type_should_be_true()
      {
         sut.IsYAxis.ShouldBeTrue();
      }
   }

   public class When_testing_axis_type_for_Y_axis_type : When_testing_axis_types_for_Y
   {
      protected override void Context()
      {
         _axisType = AxisTypes.Y;
         base.Context();
      }
   }

   public class When_testing_axis_type_for_Y2_axis_type : When_testing_axis_types_for_Y
   {
      protected override void Context()
      {
         _axisType = AxisTypes.Y2;
         base.Context();
      }
   }

   public class When_testing_axis_type_for_Y3_axis_type : When_testing_axis_types_for_Y
   {
      protected override void Context()
      {
         _axisType = AxisTypes.Y3;
         base.Context();
      }
   }
}
