using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serializers
{
   public class TransportXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationTransport()
      {
         Transport x1 = CreateObject<Transport>().WithName("Trevor").WithDimension(DimensionLength);
         IContainer c1 = CreateObject<Container>().WithName("Conrad");
         IMoleculeAmount sourceAmount = CreateObject<MoleculeAmount>().WithName("Source").WithParentContainer(c1);
         IMoleculeAmount targetAmount = CreateObject<MoleculeAmount>().WithName("Target").WithParentContainer(c1);
         x1.SourceAmount = sourceAmount;
         x1.TargetAmount = targetAmount;


         var cont1 = new Container {c1, x1}.WithId("toto");
         var cont2 = SerializeAndDeserialize(cont1);
         var x2 = cont2.FindByName(x1.Name).DowncastTo<Transport>();

         AssertForSpecs.AreEqualTransport(x2, x1);
      }
   }
}