using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class FormulaCacheXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         IFormula f1 = CreateObject<ConstantFormula>().WithName("F.Constantin").WithDimension(DimensionLength).WithValue(2.1);
         IFormula f2 = CreateObject<ExplicitFormula>().WithName("F.Erika").WithDimension(DimensionLength).WithFormulaString("A * 2");
         IFormulaUsablePath fup = new FormulaUsablePath(new[] {"aa", "bb"}).WithAlias("b").WithDimension(DimensionLength);
         f2.AddObjectPath(fup);

         FormulaCache x1 = new FormulaCache();
         x1.Add(f1);
         x1.Add(f2);

         IFormulaCache x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualFormulaCache(x2, x1);
      }
   }
}