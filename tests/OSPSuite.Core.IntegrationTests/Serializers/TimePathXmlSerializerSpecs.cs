using NUnit.Framework;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{
   internal class TimePathXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var objectPathFactory = IoC.Resolve<IObjectPathFactory>();
         TimePath x1 = objectPathFactory.CreateTimePath(DimensionTime);
         x1.Alias = "t";
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqual(x1, x2);
         x2.Alias.ShouldBeEqualTo("t");
      }
   }
}
