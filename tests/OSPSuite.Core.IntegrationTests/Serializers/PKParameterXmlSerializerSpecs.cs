using NUnit.Framework;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class PKParameterXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new PKParameter {Description = "one pk parameter", Name = "Cmax", DisplayName = "The CMax", Dimension = DimensionLength};
         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualPKParameter(x1, x2);
      }
   }
}