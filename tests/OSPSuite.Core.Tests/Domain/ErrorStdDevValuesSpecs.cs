using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Import;

namespace OSPSuite.Core.Domain
{
   public class When_resolving_the_error_type_written_in_a_cell : StaticContextSpecification
   {
      [TestCase("ASD")]
      [TestCase("asd")]
      [TestCase(" ArithmeticStdDev ")]
      [TestCase("arithmetic standard deviation")]
      public void should_return_the_arithmetic_standard_deviation_for_an_arithmetic_value(string value)
      {
         ErrorStdDevValues.ErrorStdDevFor(value).ShouldBeEqualTo(Constants.STD_DEV_ARITHMETIC);
      }

      [TestCase("GSD")]
      [TestCase("gsd ")]
      [TestCase("GeometricStdDev")]
      [TestCase("Geometric Standard Deviation")]
      public void should_return_the_geometric_standard_deviation_for_a_geometric_value(string value)
      {
         ErrorStdDevValues.ErrorStdDevFor(value).ShouldBeEqualTo(Constants.STD_DEV_GEOMETRIC);
      }

      [TestCase(new object[] { null })]
      [TestCase("")]
      [TestCase("  ")]
      [TestCase("SD")]
      [TestCase("Arithmetic")]
      public void should_return_null_for_an_empty_or_unknown_value(string value)
      {
         ErrorStdDevValues.ErrorStdDevFor(value).ShouldBeNull();
      }
   }

   public abstract class concern_for_Column : ContextSpecification<Column>
   {
      protected override void Context()
      {
         sut = new Column
         {
            Name = "Error",
            Unit = new UnitDescription(UnitDescription.InvalidUnit),
            ErrorStdDev = Constants.STD_DEV_GEOMETRIC
         };
      }
   }

   public class When_checking_the_unit_mapping_of_a_geometric_error_column : concern_for_Column
   {
      [Observation]
      public void should_not_require_a_unit_when_every_data_set_is_geometric()
      {
         sut.ErrorStdDevIsAlwaysGeometric.ShouldBeTrue();
         sut.MissingUnitMapping().ShouldBeFalse();
      }

      [Observation]
      public void should_require_a_unit_when_the_error_type_comes_from_a_column()
      {
         sut.ErrorStdDevColumn = "Error type";
         sut.ErrorStdDevIsAlwaysGeometric.ShouldBeFalse();
         sut.MissingUnitMapping().ShouldBeTrue();
      }
   }

   public class When_copying_a_column_with_an_error_type_column : concern_for_Column
   {
      private Column _copy;

      protected override void Context()
      {
         base.Context();
         sut.ErrorStdDevColumn = "Error type";
      }

      protected override void Because()
      {
         _copy = new Column(sut);
      }

      [Observation]
      public void should_copy_the_error_type_column()
      {
         _copy.ErrorStdDevColumn.ShouldBeEqualTo("Error type");
      }
   }
}
