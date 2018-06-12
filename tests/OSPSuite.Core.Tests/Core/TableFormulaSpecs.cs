using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core
{
   public abstract class concern_for_TableFormula : ContextSpecification<TableFormula>
   {
      protected IDimension _dimensionpH;
      protected IDimension _dimensionLength;

      protected override void Context()
      {
         sut = new TableFormula();
         _dimensionLength = new Dimension(new BaseDimensionRepresentation {LengthExponent = 1}, "Length", "m");
         _dimensionLength.AddUnit("cm", 0.01, 0);
         _dimensionpH = new Dimension(new BaseDimensionRepresentation(), "pH", "");
      }
   }

   public class When_creating_a_default_table_formula : concern_for_TableFormula
   {
      [Observation]
      public void x_dimension_and_y_dimension_should_not_be_null()
      {
         sut.XDimension.ShouldNotBeNull();
         sut.Dimension.ShouldNotBeNull();
      }

      [Observation]
      public void x_display_unit_and_y_display_unit_should_not_be_null()
      {
         sut.XDisplayUnit.ShouldNotBeNull();
         sut.YDisplayUnit.ShouldNotBeNull();
      }

   }

   public class When_adding_points_to_the_table_formula : concern_for_TableFormula
   {
      protected override void Context()
      {
         base.Context();
         sut.AddPoint(1, 10);
         sut.AddPoint(2, 20);
         sut.AddPoint(3, 30);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_given_value_for_an_exact_time()
      {
         sut.ValueAt(1).ShouldBeEqualTo(10);
         sut.ValueAt(3).ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_return_the_first_value_for_a_time_below_the_first_time_sample()
      {
         sut.ValueAt(0).ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_return_the_largest_value_for_a_time_above_the_first_time_sample()
      {
         sut.ValueAt(4).ShouldBeEqualTo(30);
      }

      [Observation]
      public void should_retun_the_interpolated_value_if_the_time_is_not_one_of_the_defined_time()
      {
         sut.ValueAt(1.5).ShouldBeEqualTo(15);
      }
   }

   public class When_adding_points_to_the_table_formula_that_are_not_sorted_by_the_time : concern_for_TableFormula
   {
      private ValuePoint _p1;
      private ValuePoint _p2;
      private ValuePoint _p3;

      protected override void Context()
      {
         base.Context();
         _p1 = new ValuePoint(1, 10);
         _p2 = new ValuePoint(2, 20);
         _p3 = new ValuePoint(3, 30);

         sut.AddPoint(_p1);
         sut.AddPoint(_p3);
         sut.AddPoint(_p2);
      }

      [Observation]
      public void should_return_them_sorted_by_time()
      {
         sut.AllPoints().ShouldOnlyContainInOrder(_p1, _p2, _p3);
      }
   }

   public class When_adding_a_point_to_the_table_that_already_exists : concern_for_TableFormula
   {
      private ValuePoint _p1;
      private ValuePoint _p2;

      protected override void Context()
      {
         base.Context();
         _p1 = new ValuePoint(1, 10);
         _p2 = new ValuePoint(2, 20);

         sut.AddPoint(_p1);
         sut.AddPoint(_p2);
         sut.AddPoint(2, 20);
      }

      [Observation]
      public void should_not_add_the_same_point_twice()
      {
         sut.AllPoints().ShouldOnlyContainInOrder(_p1, _p2);
      }
   }

   public class When_adding_a_point_to_the_table_that_has_the_same_time_but_not_the_same_value : concern_for_TableFormula
   {
      private ValuePoint _p1;
      private ValuePoint _p2;

      protected override void Context()
      {
         base.Context();
         _p1 = new ValuePoint(1, 10);
         _p2 = new ValuePoint(2, 20);

         sut.AddPoint(_p1);
         sut.AddPoint(_p2);
      }

      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.AddPoint(2, 30)).ShouldThrowAn<ValuePointAlreadyExistsForPointException>();
      }
   }

   public class When_clearing_all_points_from_a_table_formula : concern_for_TableFormula
   {
      protected override void Context()
      {
         base.Context();

         sut.AddPoint(new ValuePoint(1, 10));
         sut.AddPoint(new ValuePoint(2, 20));
      }

      protected override void Because()
      {
         sut.ClearPoints();
      }

      [Observation]
      public void the_table_formula_should_not_contain_any_points()
      {
         sut.AllPoints().Count().ShouldBeEqualTo(0);
      }
   }

   public class When_retrieving_the_DisplayUnit_of_a_table_formula : concern_for_TableFormula
   {
      protected override void Context()
      {
         base.Context();
         sut.XDimension = _dimensionLength;
         sut.Dimension = _dimensionpH;
         sut.XDisplayUnit = _dimensionLength.Unit("cm");
      }

      [Observation]
      public void should_return_the_default_unit_of_the_dimension_if_not_set()
      {
         sut.YDisplayUnit.ShouldBeEqualTo(_dimensionpH.DefaultUnit);
      }

      [Observation]
      public void should_return_the_display_unit_if_set()
      {
         sut.XDisplayUnit.ShouldBeEqualTo(_dimensionLength.Unit("cm"));
      }
   }

   public class When_removing_an_existing_point : concern_for_TableFormula
   {
      private ValuePoint _p1;
      private ValuePoint _p2;

      protected override void Context()
      {
         base.Context();
         _p1 = new ValuePoint(1, 10);
         _p2 = new ValuePoint(2, 20);

         sut.AddPoint(_p1);
         sut.AddPoint(_p2);
      }

      protected override void Because()
      {
         sut.RemovePoint(_p1);
      }

      [Observation]
      public void should_have_removed_that_point()
      {
         sut.AllPoints().ShouldOnlyContain(_p2);
      }
   }

   public class When_removing_an_existing_using_the_x_and_y_values : concern_for_TableFormula
   {
      private ValuePoint _p1;
      private ValuePoint _p2;

      protected override void Context()
      {
         base.Context();
         _p1 = new ValuePoint(1, 10);
         _p2 = new ValuePoint(2, 20);

         sut.AddPoint(_p1);
         sut.AddPoint(_p2);
      }

      protected override void Because()
      {
         sut.RemovePoint(_p1.X, _p1.Y);
      }

      [Observation]
      public void should_have_removed_that_point()
      {
         sut.AllPoints().ShouldOnlyContain(_p2);
      }
   }

   public class When_removing_a_point_that_does_not_exist : concern_for_TableFormula
   {
      private ValuePoint _p1;
      private ValuePoint _p2;

      protected override void Context()
      {
         base.Context();
         _p1 = new ValuePoint(1, 10);
         _p2 = new ValuePoint(2, 20);

         sut.AddPoint(_p1);
         sut.AddPoint(_p2);
      }

      protected override void Because()
      {
         sut.RemovePoint(2, 30);
      }

      [Observation]
      public void should_not_remove_the_existing_points()
      {
         sut.AllPoints().ShouldOnlyContain(_p1, _p2);
      }
   }

   public class When_retrieving_the_value_in_display_unit : concern_for_TableFormula
   {
      protected override void Context()
      {
         base.Context();
         sut.Dimension = A.Fake<IDimension>();
         sut.XDisplayUnit = A.Fake<Unit>();
         sut.XDimension = A.Fake<IDimension>();
         sut.YDisplayUnit = A.Fake<Unit>();
         A.CallTo(() => sut.XDimension.BaseUnitValueToUnitValue(sut.XDisplayUnit, 1)).Returns(10);
         A.CallTo(() => sut.Dimension.BaseUnitValueToUnitValue(sut.YDisplayUnit, 1)).Returns(100);
      }

      [Observation]
      public void should_use_the_expcted_conversion_method_to_convert_the_values_from_base_to_display_unit()
      {
         sut.XDisplayValueFor(1).ShouldBeEqualTo(10);
         sut.YDisplayValueFor(1).ShouldBeEqualTo(100);
      }
   }

   public class When_retrieving_the_value_in_base_unit : concern_for_TableFormula
   {
      protected override void Context()
      {
         base.Context();
         sut.Dimension = A.Fake<IDimension>();
         sut.XDisplayUnit = A.Fake<Unit>();
         sut.XDimension = A.Fake<IDimension>();
         sut.YDisplayUnit = A.Fake<Unit>();
         A.CallTo(() => sut.XDimension.UnitValueToBaseUnitValue(sut.XDisplayUnit, 1)).Returns(10);
         A.CallTo(() => sut.Dimension.UnitValueToBaseUnitValue(sut.YDisplayUnit, 1)).Returns(100);
      }

      [Observation]
      public void should_use_the_expcted_conversion_method_to_convert_the_values_from_display_to_base_unit()
      {
         sut.XBaseValueFor(1).ShouldBeEqualTo(10);
         sut.YBaseValueFor(1).ShouldBeEqualTo(100);
      }
   }
}