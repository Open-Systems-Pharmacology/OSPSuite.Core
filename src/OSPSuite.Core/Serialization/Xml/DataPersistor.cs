using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using IContainer = OSPSuite.Utility.Container.IContainer;

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
      private readonly IContainer _container;

      public DataPersistor(IOSPSuiteXmlSerializerRepository serializerRepository, IContainer container)
      {
         _serializerRepository = serializerRepository;
         _container = container;
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
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var xel = Serialize(obj, serializationContext);
            xel.PermissiveSave(fileName);
         }
      }

      public virtual T Load<T>(string fileName, IDimensionFactory dimensionFactory = null, IEnumerable<DataRepository> dataRepositories = null)
      {
         using (var serializationContext = SerializationTransaction.Create(_container, dimensionFactory, dataRepositories: dataRepositories))
         {
            var xel = XElementSerializer.PermissiveLoad(fileName);
            return Deserialize<T>(xel, serializationContext);
         }
      }

      public virtual string ToString<T>(T obj)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            return Serialize(obj, serializationContext).ToString();
         }
      }

      public virtual T FromString<T>(string serializationString)
      {
         if (string.IsNullOrEmpty(serializationString))
            return default(T);

         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var xDoc = XDocument.Load(new StringReader(serializationString));
            return Deserialize<T>(xDoc.Root, serializationContext);
         }
      }
   }
}