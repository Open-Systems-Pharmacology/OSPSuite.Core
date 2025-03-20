using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization.Exchange
{
   public interface ISimulationPersistor
   {
      void Save(SimulationTransfer simulationTransfer, string fileName);
      SimulationTransfer Load(string pkmlFileFullPath, IWithIdRepository withIdRepository = null);
   }

   public class SimulationPersistor : ISimulationPersistor
   {
      private readonly IOSPSuiteXmlSerializerRepository _xmlSerializerRepository;
      private readonly IContainer _container;
      private readonly IPKMLPersistor _pkmlPersistor;

      public SimulationPersistor(
         IOSPSuiteXmlSerializerRepository xmlSerializerRepository,
         IContainer container,
         IPKMLPersistor pkmlPersistor)
      {
         _xmlSerializerRepository = xmlSerializerRepository;
         _container = container;
         _pkmlPersistor = pkmlPersistor;
      }

      public void Save(SimulationTransfer simulationTransfer, string fileName)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _xmlSerializerRepository.SerializerFor(simulationTransfer);
            var element = serializer.Serialize(simulationTransfer, serializationContext);

            element.PermissiveSave(fileName);
         }
      }

      public SimulationTransfer Load(string pkmlFileFullPath, IWithIdRepository withIdRepository = null)
      {
         return _pkmlPersistor.Load<SimulationTransfer>(pkmlFileFullPath, withIdRepository: withIdRepository);
      }
   }
}