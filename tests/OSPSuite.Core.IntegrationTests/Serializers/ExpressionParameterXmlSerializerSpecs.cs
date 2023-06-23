using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ExpressionParameterXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Observation]
      public void TestSerialization()
      {
         var formula = CreateObject<ExplicitFormula>().WithName("F.Erika").WithDimension(DimensionLength).WithFormulaString("A * 2");
         var fup = new FormulaUsablePath("aa", "bb").WithAlias("b").WithDimension(DimensionLength);
         formula.AddObjectPath(fup);


         var x1 = new ExpressionParameter
         {
            Value = 2.0,
            Formula = formula
         };

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualExpressionParameters(x2, x1);
      }

   }
}
