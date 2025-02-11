using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ContainerXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyContainer()
      {
         var x1 = CreateObject<Container>().WithName("Conrad").WithMode(ContainerMode.Logical);
         x1.ParentPath = new ObjectPath("A", "B");
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualContainer(x2, x1);
      }

      [Test]
      public void TestSerializationNonEmptyContainer()
      {
         var x1 = CreateObject<Container>().WithName("Conrad").WithMode(ContainerMode.Physical);
         var o1 = CreateObject<Observer>().WithName("Oberon").WithParentContainer(x1);
         var c1 = CreateObject<Container>().WithName("Carolin").WithMode(ContainerMode.Logical);
         x1.Add(c1);

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualContainer(x2, x1);
      }
   }
}