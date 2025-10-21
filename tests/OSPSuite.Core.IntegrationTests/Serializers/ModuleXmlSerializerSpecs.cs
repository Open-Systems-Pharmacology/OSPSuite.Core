using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ModuleXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var module1 = CreateObject<Module>().WithName("Module1");

         module1.PKSimVersion = "version1";
         module1.Add(CreateObject<EventGroupBuildingBlock>().WithName("EventGroup"));
         module1.Add(CreateObject<PassiveTransportBuildingBlock>().WithName("PassiveTransport"));
         module1.Add(CreateObject<MoleculeBuildingBlock>().WithName("Molecule"));
         module1.Add(CreateObject<ReactionBuildingBlock>().WithName("Reaction"));
         module1.Add(CreateObject<SpatialStructure>().WithName("SpatialStructure"));
         module1.Add(CreateObject<ObserverBuildingBlock>().WithName("Observer"));

         module1.Add(CreateObject<InitialConditionsBuildingBlock>().WithName("ICBB"));
         module1.Add(CreateObject<ParameterValuesBuildingBlock>().WithName("PVBB"));

         var module2 = SerializeAndDeserialize(module1);

         AssertForSpecs.AreEqual(module1, module2);
      }
   }
}