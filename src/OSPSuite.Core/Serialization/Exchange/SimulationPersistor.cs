using System.Xml.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.Xml;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serialization.Exchange
{
   public interface ISimulationPersistor
   {
      void Save(SimulationTransfer simulationTransfer, string fileName);
      SimulationTransfer Load(string pkmlFileFullPath, IWithIdRepository withIdRepository = null);
      string Serialize(SimulationTransfer simulationTransfer);
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

      public void Save(SimulationTransfer simulationTransfer, string fileName) => xElementFor(simulationTransfer).PermissiveSave(fileName);

      public string Serialize(SimulationTransfer simulationTransfer) => xElementFor(simulationTransfer).ToString();

      private XElement xElementFor(SimulationTransfer simulationTransfer)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            var serializer = _xmlSerializerRepository.SerializerFor(simulationTransfer);
            var simulationTransferElement = serializer.Serialize(simulationTransfer, serializationContext);
            AddToSerializedSimulationTransfer(simulationTransferElement, simulationTransfer, serializationContext);
            return simulationTransferElement;
         }
      }

      /// <summary>
      ///    Called once the <paramref name="simulationTransfer" /> has been serialized into
      ///    <paramref name="simulationTransferElement" />, before the element is saved or returned.
      ///    Override it to add application specific content to the pkml. Applications that do not know about this content
      ///    ignore it when loading the file.
      /// </summary>
      protected virtual void AddToSerializedSimulationTransfer(XElement simulationTransferElement, SimulationTransfer simulationTransfer, SerializationContext serializationContext)
      {
      }

      public SimulationTransfer Load(string pkmlFileFullPath, IWithIdRepository withIdRepository = null) => 
         _pkmlPersistor.Load<SimulationTransfer>(pkmlFileFullPath, withIdRepository: withIdRepository);
   }
}