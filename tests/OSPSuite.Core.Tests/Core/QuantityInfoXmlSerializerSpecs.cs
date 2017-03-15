using System.Collections.Generic;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class QuantityInfoXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var path = new List<string>(new string[] {"aa", "bb"});
         QuantityInfo x1 = new QuantityInfo("Quain", path, QuantityType.Parameter);

         var mapper = new StringEnumerableAttributeMapper();
         bool ismatch = mapper.IsMatch(typeof (List<string>));
         ismatch = mapper.IsMatch(typeof (IList<string>));
         QuantityInfo x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualMcQuantityInfo(x1, x2);
      }

      [Test]
      public void TestSerializationWithEmptyPath()
      {
         var path = new List<string>();
         QuantityInfo x1 = new QuantityInfo("Quain", path, QuantityType.Parameter);
         QuantityInfo x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualMcQuantityInfo(x1, x2);
      }
   }
}