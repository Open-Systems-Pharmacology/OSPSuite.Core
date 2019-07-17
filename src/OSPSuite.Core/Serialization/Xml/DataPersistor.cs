using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Serialization.Xml
{
   public interface IDataPersistor
   {
      T Load<T>(string fileName, IDimensionFactory dimensionFactory = null, IEnumerable<DataRepository> dataRepositories = null);
      string ToString<T>(T obj);
      T FromString<T>(string serializationString);
      void Save<T>(T obj, string fileName);
   }

   public class DataPersistor : IDataPersistor
   {
      private readonly IOSPSuiteXmlSerializerRepository _serializerRepository;

      public DataPersistor(IOSPSuiteXmlSerializerRepository serializerRepository)
      {
         _serializerRepository = serializerRepository;
      }

      public virtual XElement Serialize<T>(T obj, SerializationContext serializationContext)
      {
         var serializer = _serializerRepository.SerializerFor(obj);
         return serializer.Serialize(obj, serializationContext);
      }

      public virtual T Deserialize<T>(XElement xel, SerializationContext serializationContext)
      {
         var serializer = _serializerRepository.SerializerFor(xel);
         return serializer.Deserialize<T>(xel, serializationContext);
      }

      public virtual void Save<T>(T obj, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create())
         {
            var xel = Serialize(obj, serializationContext);
            xel.Save(fileName);
         }
      }

      public virtual T Load<T>(string fileName, IDimensionFactory dimensionFactory = null, IEnumerable<DataRepository> dataRepositories = null)
      {
         using (var serializationContext = SerializationTransaction.Create(dimensionFactory, dataRepositories: dataRepositories))
         {
            var xel = XElement.Load(fileName);
            return Deserialize<T>(xel, serializationContext);
         }
      }

      public virtual string ToString<T>(T obj)
      {
         using (var serializationContext = SerializationTransaction.Create())
         {
            return Serialize(obj, serializationContext).ToString();
         }
      }

      public virtual T FromString<T>(string serializationString)
      {
         if (string.IsNullOrEmpty(serializationString))
            return default(T);

         using (var serializationContext = SerializationTransaction.Create())
         {
            var xDoc = XDocument.Load(new StringReader(serializationString));
            return Deserialize<T>(xDoc.Root, serializationContext);
         }
      }
   }
}