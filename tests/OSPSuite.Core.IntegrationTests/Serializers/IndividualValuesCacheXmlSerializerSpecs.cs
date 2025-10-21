using NUnit.Framework;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public  class IndividualValuesCacheXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationFormulaWithoutObjectPaths()
      {
         var x1 = new IndividualValuesCache();
         x1.SetValues("Path1", new []{10,20,30d});
         x1.SetValues("Path2", new []{10,20,30d});
         x1.AddCovariate("Gender", new []{"Male", "Female", "Female"});
         x1.IndividualIds.AddRange(new []{1,4,5});
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualIndividualValueCache(x2, x1);
      }
   }
}