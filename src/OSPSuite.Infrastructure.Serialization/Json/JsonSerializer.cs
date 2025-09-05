using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSPSuite.Assets.Extensions;

namespace OSPSuite.Infrastructure.Serialization.Json
{
   public class JsonSerializer : IJsonSerializer
   {
      protected readonly JsonSerializerSettings _settings = new OSPSuiteJsonSerializerSettings();

      //Defines a static field as the free license only allows for a limited number of schema generation per hour
      private static readonly ConcurrentDictionary<Type, JSchema> _schemas = new ConcurrentDictionary<Type, JSchema>();

      public async Task Serialize(object objectToSerialize, string fileName)
      {
         var data = Serialize(objectToSerialize);

         using (var sw = new StreamWriter(fileName))
         {
            await sw.WriteAsync(data);
         }
      }

      public string Serialize(object objectToSerialize) => JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented, _settings);

      public string SerializeToBase64String(object objectToSerialize) => Serialize(objectToSerialize).ToBase64String();

      public async Task<object[]> DeserializeAsArray(string fileName, Type objectType)
      {
         string json;
         using (var reader = new StreamReader(fileName))
         {
            json = await reader.ReadToEndAsync();
         }

         return deserializeAsArrayFromString(json, objectType);
      }

      public Task<object[]> DeserializeAsArrayFromString(string jsonString, Type objectType) =>
         Task.FromResult(deserializeAsArrayFromString(jsonString, objectType));

      public async Task<T[]> DeserializeAsArrayFromString<T>(string jsonString) => (await DeserializeAsArrayFromString(jsonString, typeof(T))).OfType<T>().ToArray();

      private object[] deserializeAsArrayFromString(string json, Type objectType)
      {
         var schema = validateSnapshot(objectType);
         var deserializedSnapshot = JsonConvert.DeserializeObject(json, _settings);

         switch (deserializedSnapshot)
         {
            case JObject jsonObject:
               return new[] { ValidatedObject(jsonObject, schema, objectType) };

            case JArray array:
               return array.Select(x => ValidatedObject(x, schema, objectType)).ToArray();
            default:
               return null;
         }
      }

      public async Task<object> Deserialize(string fileName, Type objectType)
      {
         var deserializedObjects = await DeserializeAsArray(fileName, objectType);
         return deserializedObjects.FirstOrDefault();
      }

      public async Task<T> Deserialize<T>(string fileName) where T : class
      {
         var deserializedObject = await Deserialize(fileName, typeof(T));
         return deserializedObject as T;
      }

      public async Task<object> DeserializeFromString(string jsonString, Type objectType)
      {
         var deserializedObjects = await DeserializeAsArrayFromString(jsonString, objectType);
         return deserializedObjects.FirstOrDefault();
      }

      public async Task<T> DeserializeFromString<T>(string jsonString) where T : class
      {
         var deserializedObject = await DeserializeFromString(jsonString, typeof(T));
         return deserializedObject as T;
      }

      public async Task<T> DeserializeFromBase64String<T>(string base64String) where T : class
      {
         var jsonString = base64String.FromBase64String();
         var deserializedObject = await DeserializeFromString(jsonString, typeof(T));
         return deserializedObject as T;
      }

      protected virtual object ValidatedObject(JToken jToken, JSchema schema, Type snapshotType)
      {
         if (jToken.IsValid(schema, out IList<string> errorMessages))
            return jToken.ToObject(snapshotType);

         throw new SnapshotFileMismatchException(snapshotType.Name, errorMessages);
      }

      private JSchema validateSnapshot(Type snapshotType) => _schemas.GetOrAdd(snapshotType, createSchemaForType);

      private JSchema createSchemaForType(Type snapshotType)
      {
         var generator = new JSchemaGenerator { DefaultRequired = Required.Default };
         generator.GenerationProviders.Add(new StringEnumGenerationProvider());
         return generator.Generate(snapshotType);
      }
   }
}