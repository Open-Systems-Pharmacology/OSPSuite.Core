using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class EntityXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         Container x1 = CreateObject<Container>().WithName("Ennea");
         Assert.IsNull(x1.ParentContainer);

         x1.AddTag("Greek");
         x1.AddTag("Girl");

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualEntity(x2, x1);
      }
   }
}