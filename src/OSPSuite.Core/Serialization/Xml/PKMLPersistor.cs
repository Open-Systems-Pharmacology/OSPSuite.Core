using System.Xml.Linq;
using OSPSuite.Serializer.Xml.Extensions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using IContainer = OSPSuite.Utility.Container.IContainer;


namespace OSPSuite.Core.Serialization.Xml
{
   public interface IPKMLPersistor
   {
      void SaveToPKML<T>(T entityToSerialize, string fileName);
   }

   public class PKMLPersistor : IPKMLPersistor
   {
      private readonly IOSPSuiteXmlSerializerRepository _serializerRepository;
      private readonly IContainer _container;

      public PKMLPersistor(IOSPSuiteXmlSerializerRepository serializerRepository, IContainer container)
      {
         _serializerRepository = serializerRepository;
         _container = container;
      }

      public void SaveToPKML<T>(T entityToSerialize, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var xElement = serializeModelPart(entityToSerialize, serializationContext);
            xElement.AddAttribute(Constants.Serialization.Attribute.VERSION, Constants.PKML_VERSION.ToString());
            xElement.Save(fileName);
         }
      }

      private XElement serializeModelPart<T>(T entityToSerialize, SerializationContext serializationContext)
      {
         var partSerializer = _serializerRepository.SerializerFor(entityToSerialize);
         var xElement = partSerializer.Serialize(entityToSerialize, serializationContext);
         if (needsFormulaCacheSerialization(entityToSerialize))
         {
            var xElementFormulas = serializeFormulas(serializationContext.Formulas, serializationContext);
            xElement.Add(xElementFormulas);
         }
         return xElement;
      }

      private static bool needsFormulaCacheSerialization<T>(T entityToSerialize)
      {
         return !(entityToSerialize.IsAnImplementationOf<IBuildingBlock>() ||
                  entityToSerialize.IsAnImplementationOf<IModelCoreSimulation>() ||
                  entityToSerialize.IsAnImplementationOf<IBuildConfiguration>() ||
                  entityToSerialize.IsAnImplementationOf<IModel>());
      }

      private XElement serializeFormulas(IFormulaCache formulas, SerializationContext serializationContext)
      {
         var serializer = _serializerRepository.SerializerFor(formulas);
         return serializer.Serialize(formulas, serializationContext);
      }
   }
}