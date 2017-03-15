using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class ObjectBaseXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = CreateObject<Container>().WithName("Obasi");
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualObjectBase(x2, x1);
      }
   }
}