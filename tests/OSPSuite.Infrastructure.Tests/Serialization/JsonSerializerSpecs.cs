using System.Collections.Generic;
using System.Globalization;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Infrastructure.Serialization.Json;

namespace OSPSuite.Infrastructure.Serialization
{
   public abstract class concern_for_JsonSerializer : ContextSpecification<IJsonSerializer>
   {
      protected override void Context()
      {
         sut = new JsonSerializer();
      }
   }

   public class When_round_tripping_a_snapshot_with_special_floating_point_data_column_values : concern_for_JsonSerializer
   {
      private DataColumn _dataColumn;
      private string _serialized;
      private DataColumn _result;

      protected override void Context()
      {
         base.Context();
         _dataColumn = new DataColumn
         {
            Name = "SD",
            Values = new List<float> {float.NaN, float.PositiveInfinity, float.NegativeInfinity, 1.5f}
         };
         _serialized = sut.Serialize(_dataColumn);
      }

      protected override void Because()
      {
         _result = sut.DeserializeFromString<DataColumn>(_serialized).Result;
      }

      [Observation]
      public void should_write_the_special_values_as_quoted_string_literals()
      {
         _serialized.Contains("\"NaN\"").ShouldBeTrue();
         _serialized.Contains("\"Infinity\"").ShouldBeTrue();
         _serialized.Contains("\"-Infinity\"").ShouldBeTrue();
      }

      [Observation]
      public void should_load_the_snapshot_without_a_schema_mismatch_and_preserve_the_values()
      {
         float.IsNaN(_result.Values[0]).ShouldBeTrue();
         float.IsPositiveInfinity(_result.Values[1]).ShouldBeTrue();
         float.IsNegativeInfinity(_result.Values[2]).ShouldBeTrue();
         _result.Values[3].ShouldBeEqualTo(1.5f);
      }
   }

   public class When_round_tripping_a_snapshot_with_a_special_floating_point_literal_in_a_string_field : concern_for_JsonSerializer
   {
      private DataColumn _dataColumn;
      private string _serialized;
      private DataColumn _result;

      protected override void Context()
      {
         base.Context();
         _dataColumn = new DataColumn
         {
            Name = "NaN",
            Values = new List<float> {1.5f}
         };
         _serialized = sut.Serialize(_dataColumn);
      }

      protected override void Because()
      {
         _result = sut.DeserializeFromString<DataColumn>(_serialized).Result;
      }

      [Observation]
      public void should_leave_the_string_field_untouched()
      {
         _result.Name.ShouldBeEqualTo("NaN");
         _result.Values[0].ShouldBeEqualTo(1.5f);
      }
   }

   public class When_round_tripping_a_nullable_double_using_a_culture_with_a_comma_as_decimal_separator : concern_for_JsonSerializer
   {
      private CultureInfo _originalCulture;

      protected override void Context()
      {
         base.Context();
         _originalCulture = CultureInfo.CurrentCulture;
         CultureInfo.CurrentCulture = new CultureInfo("de-DE");
      }

      public override void Cleanup()
      {
         base.Cleanup();
         CultureInfo.CurrentCulture = _originalCulture;
      }

      private double? roundTrip(double? value) =>
         sut.DeserializeFromString<Parameter>(sut.Serialize(new Parameter {Value = value})).Result.Value;

      [Observation]
      public void should_not_interpret_the_decimal_separator_as_a_group_separator()
      {
         roundTrip(1.0).ShouldBeEqualTo(1.0);
         roundTrip(30.0).ShouldBeEqualTo(30.0);
         roundTrip(1.123456789).ShouldBeEqualTo(1.123456789);
         roundTrip(1e-12).ShouldBeEqualTo(1e-12);
         roundTrip(0.0000000142).ShouldBeEqualTo(0.0000000142);
         roundTrip(449.9999999996).ShouldBeEqualTo(449.9999999996);
         roundTrip(-1.5).ShouldBeEqualTo(-1.5);
      }

      [Observation]
      public void should_still_round_to_the_expected_precision()
      {
         roundTrip(449.9999999999991).ShouldBeEqualTo(450);
         roundTrip(199.999999998E-2).ShouldBeEqualTo(2d);
      }

      [Observation]
      public void should_preserve_a_null_value()
      {
         roundTrip(null).ShouldBeNull();
      }
   }
}
