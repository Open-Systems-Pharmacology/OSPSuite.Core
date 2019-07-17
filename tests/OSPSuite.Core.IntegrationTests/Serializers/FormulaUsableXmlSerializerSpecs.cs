using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class FormulaUsableXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyFormulaUsable()
      {
         Observer x1 = CreateObject<Observer>().WithName("Obasi");
         IFormulaUsable x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualFormulaUsable(x2, x1);
      }

      [Test]
      public void TestSerializationNonEmptyFormulaUsable()
      {
         Observer x1 = CreateObject<Observer>().WithName("Obasi").WithDimension(DimensionLength);
         x1.Value = 2.3D;
         IFormulaUsable x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualFormulaUsable(x2, x1);
      }

   }
}