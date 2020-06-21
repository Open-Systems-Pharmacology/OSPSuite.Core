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
         Container x1 = CreateObject<Container>().WithName("Conrad").WithMode(ContainerMode.Logical);
         IContainer x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualContainer(x2, x1);
      }

      [Test]
      public void TestSerializationNonEmptyContainer()
      {
         Container x1 = CreateObject<Container>().WithName("Conrad").WithMode(ContainerMode.Physical);
         Observer o1 = CreateObject<Observer>().WithName("Oberon").WithParentContainer(x1);
         Container c1 = CreateObject<Container>().WithName("Carolin").WithMode(ContainerMode.Logical);
         x1.Add(c1);

         IContainer x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualContainer(x2, x1);
      }
   }
}