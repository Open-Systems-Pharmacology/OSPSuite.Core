using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class ParameterBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationParameterBuilderWithoutParameter()
      {
         Parameter x1 = CreateObject<Parameter>().WithName("Pascal.Builder");
         x1.BuildMode = ParameterBuildMode.Global;
         x1.CanBeVaried = true;
         x1.Visible = true;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameterBuilder(x2, x1);
      }

      [Test]
      public void TestSerializationParameterBuilderWithParameterWithConstantFormula()
      {
         IFormula f1 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(2.3);
         Parameter x1 = CreateObject<Parameter>().WithName("Patricia").WithFormula(f1).WithDimension(DimensionLength);
         x1.BuildMode = ParameterBuildMode.Property;
         x1.Visible  = false;
         x1.CanBeVaried  = false;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameterBuilder(x1, x2);
      }

      [Test]
      public void TestSerializationParameterBuilderWithParameterWithExplicitFormula()
      {
         IFormula f1 = CreateObject<ExplicitFormula>().WithDimension(DimensionLength).WithFormulaString("3*Patty");
         Parameter x1 = CreateObject<Parameter>().WithName("Peter").WithFormula(f1);
         x1.BuildMode = ParameterBuildMode.Local;

         IFormulaUsablePath fup = new FormulaUsablePath(new[] {"Patricia"}).WithAlias("Patty").WithDimension(DimensionLength);
         f1.AddObjectPath(fup);

         x1.Value = 3.4;


         IParameter x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameterBuilder(x2, x1);
      }
   }
}