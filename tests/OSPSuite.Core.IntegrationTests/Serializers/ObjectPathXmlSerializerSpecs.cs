using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Serializers
{
   public class ObjectPathXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new[] {"aa", "bb"}.ToObjectPath();
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualObjectPath(x2, x1);
      }
   }

   public class FormulaUsablePathXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var objectPathFactory = IoC.Resolve<IObjectPathFactory>();
         var x1 = objectPathFactory.CreateFormulaUsablePathFrom("..", "aa", "bb").WithAlias("FUP").WithDimension(DimensionLength) as FormulaUsablePath;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualFormulaUsablePath(x2, x1);
      }
   }
}