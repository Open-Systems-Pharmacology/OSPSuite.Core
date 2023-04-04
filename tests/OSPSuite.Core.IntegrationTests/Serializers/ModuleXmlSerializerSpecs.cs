using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ModuleXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var module1 = CreateObject<Module>().WithName("Module1");

         module1.AddExtendedProperty("PKSimVersion", "version1");
         module1.EventGroups = CreateObject<EventGroupBuildingBlock>().WithName("EventGroup");
         module1.PassiveTransports = CreateObject<PassiveTransportBuildingBlock>().WithName("PassiveTransport");
         module1.Molecules = CreateObject<MoleculeBuildingBlock>().WithName("Molecule");
         module1.Reactions = CreateObject<ReactionBuildingBlock>().WithName("Reaction");
         module1.SpatialStructure = CreateObject<SpatialStructure>().WithName("SpatialStructure");
         module1.Observers = CreateObject<ObserverBuildingBlock>().WithName("Observer");

         module1.AddMoleculeStartValueBlock(CreateObject<MoleculeStartValuesBuildingBlock>().WithName("MSVBB"));
         module1.AddParameterStartValueBlock(CreateObject<ParameterStartValuesBuildingBlock>().WithName("PSVBB"));

         var module2 = SerializeAndDeserialize(module1);

         AssertForSpecs.AreEqual(module1, module2);
      }
   }
}