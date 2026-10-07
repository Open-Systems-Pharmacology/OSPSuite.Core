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

   public class When_deserializing_a_snapshot_whose_root_has_a_dollar_type_naming_another_class : concern_for_JsonSerializer
   {
      private DataColumn _result;

      protected override void Context()
      {
         base.Context();
         TypeNamedInJson.Instantiated = false;
      }

      protected override void Because()
      {
         _result = sut.DeserializeFromString<DataColumn>($"{{\"$type\":\"{typeof(TypeNamedInJson).AssemblyQualifiedName}\",\"Name\":\"SD\"}}").Result;
      }

      [Observation]
      public void should_not_instantiate_the_named_type_and_deserialize_the_requested_snapshot()
      {
         TypeNamedInJson.Instantiated.ShouldBeFalse();
         _result.Name.ShouldBeEqualTo("SD");
      }
   }

   public class When_deserializing_a_snapshot_with_dollar_types_while_the_global_json_settings_enable_type_names : concern_for_JsonSerializer
   {
      private ExtendedProperty _result;

      protected override void Context()
      {
         base.Context();
         TypeNamedInJson.Instantiated = false;
      }

      protected override void Because()
      {
         var originalDefaultSettings = Newtonsoft.Json.JsonConvert.DefaultSettings;
         Newtonsoft.Json.JsonConvert.DefaultSettings = () => new Newtonsoft.Json.JsonSerializerSettings { TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto };
         try
         {
            var typeName = typeof(TypeNamedInJson).AssemblyQualifiedName;
            _result = sut.DeserializeFromString<ExtendedProperty>($"{{\"$type\":\"{typeName}\",\"Name\":\"Prop\",\"Value\":{{\"$type\":\"{typeName}\"}}}}").Result;
         }
         finally
         {
            Newtonsoft.Json.JsonConvert.DefaultSettings = originalDefaultSettings;
         }
      }

      [Observation]
      public void should_instantiate_neither_the_root_nor_the_nested_named_type()
      {
         TypeNamedInJson.Instantiated.ShouldBeFalse();
         _result.Name.ShouldBeEqualTo("Prop");
      }
   }

   internal class TypeNamedInJson
   {
      public static bool Instantiated;

      public TypeNamedInJson() => Instantiated = true;
   }
}
