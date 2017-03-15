using NUnit.Framework;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Helpers;

namespace OSPSuite.Core
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