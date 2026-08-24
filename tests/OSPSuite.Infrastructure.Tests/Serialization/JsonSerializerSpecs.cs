using System.Collections.Generic;
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
}
