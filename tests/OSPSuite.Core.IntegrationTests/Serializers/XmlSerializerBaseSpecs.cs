using System.Collections.Generic;
using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{
   public abstract class XmlSerializerBaseSpecs<TSerializerRepository> : ContextForIntegration<TSerializerRepository>
      where TSerializerRepository : IXmlSerializerRepository<SerializationContext>
   {
      protected TSerializerRepository SerializerRepository { get; private set; }

      public override void GlobalContext()
      {
         base.GlobalContext();
         SerializerRepository = IoC.Resolve<TSerializerRepository>();
      }

      protected T SerializeAndDeserialize<T>(T x1)
      {
         XElement xel;
         XElement formulaCacheElement;
         var formulaCacheSerializer = SerializerRepository.SerializerFor<IFormulaCache>();
         IXmlSerializer<SerializationContext> serializer;

         using (var serializationContext = SerializationTransaction.Create())
         {
            serializer = SerializerRepository.SerializerFor(x1);
            xel = serializer.Serialize(x1, serializationContext);
            formulaCacheElement = formulaCacheSerializer.Serialize(serializationContext.Formulas, serializationContext);
         }

         using (var serializationContext = NewDeserializationContext())
         {
            formulaCacheSerializer.Deserialize(serializationContext.Formulas, formulaCacheElement, serializationContext);
            return serializer.Deserialize<T>(xel, serializationContext);
         }
      }

      protected SerializationContext NewDeserializationContext()
      {
         var objectBaseFactory = IoC.Resolve<IObjectBaseFactory>();
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         return SerializationTransaction.Create(dimensionFactory, objectBaseFactory, new WithIdRepository(), new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), new ModelFinalizer(new ObjectPathFactory(new AliasCreator()), new ReferencesResolver())), new List<DataRepository>());
      }
   }
}