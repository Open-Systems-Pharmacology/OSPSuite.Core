using System.Collections.Generic;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Helpers;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class QuantityInfoXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var path = new List<string>(new string[] {"aa", "bb"});
         QuantityInfo x1 = new QuantityInfo(path, QuantityType.Parameter);

         var mapper = new StringEnumerableAttributeMapper();
         bool ismatch = mapper.IsMatch(typeof (List<string>));
         ismatch = mapper.IsMatch(typeof (IList<string>));
         QuantityInfo x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantityInfo(x1, x2);
      }

      [Test]
      public void TestSerializationWithEmptyPath()
      {
         var path = new List<string>();
         QuantityInfo x1 = new QuantityInfo(path, QuantityType.Parameter);
         QuantityInfo x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantityInfo(x1, x2);
      }
   }
}