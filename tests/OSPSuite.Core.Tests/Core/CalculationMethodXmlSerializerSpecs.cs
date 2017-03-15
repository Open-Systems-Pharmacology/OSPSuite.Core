using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class CalculationMethodXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationCalculationMethod()
      {
         var x1 = CreateObject<CoreCalculationMethod>().WithName("CM");
         x1.Category = "Category";

         var explicitFormula = CreateObject<ExplicitFormula>().WithName("EP").WithFormulaString("1+2");
         var constantFormula = CreateObject<ConstantFormula>().WithName("C").WithValue(5);

         var paramDescriptor1 = new ParameterDescriptor("K", Create.Criteria(x => x.With("Liver")));
         var paramDescriptor2 = new ParameterDescriptor("P", Create.Criteria(x => x.With("Kidney")));

         x1.AddOutputFormula(explicitFormula, paramDescriptor1);
         x1.AddOutputFormula(constantFormula, paramDescriptor2);

         var p1 = CreateObject<Parameter>().WithName("P1").WithFormula(CreateObject<ExplicitFormula>().WithName("P1 Formula").WithFormulaString("3+4"));
         var p2 = CreateObject<Parameter>().WithName("P2").WithFormula(CreateObject<ExplicitFormula>().WithName("P2 Formula").WithFormulaString("5+6"));

         x1.AddHelpParameter(p1, Create.Criteria(x => x.With("Lung")));
         x1.AddHelpParameter(p2, Create.Criteria(x => x.With("Heart")));


         var x2 = SerializeAndDeserialize(x1);


         AssertForSpecs.AreEqualCalculationMethod(x1, x2);
      }
   }
}