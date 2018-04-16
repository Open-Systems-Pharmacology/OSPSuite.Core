using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public class FormulaXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
      //Use ExplicitFormula instead of ConstantFormula, because behaviour of ObjectPaths is overwritten in ConstantFormula
   {
      [Test]
      public void TestSerializationFormulaWithoutObjectPaths()
      {
         ExplicitFormula x1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         IFormula x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualFormula(x2, x1);
      }

      [Test]
      public void TestSerializationFormulaWithObjectPathsWithoutObjectReferences()
      {
         ExplicitFormula x1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         x1.AddObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P2).WithAlias("Pitter"));

         IFormula x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualFormula(x2, x1);
      }

      [Test]
      public void TestSerializationTableFormulaWithArgument()
      {
         var x1 = CreateObject<TableFormulaWithXArgument>().WithName("Fortunato").WithDimension(DimensionLength);
         x1.AddTableObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         x1.AddXArgumentObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualTableFormulaWithArgument(x2, x1);
      }

      [Test]
      public void TestSerializationFormulaWithObjectPathsWithObjectReferences()
      {
         ExplicitFormula x1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         x1.AddObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P2).WithAlias("Pitter"));
         x1.ResolveObjectPathsFor(P);

         IFormula x2 = SerializeAndDeserialize(x1);

         x2.ResolveObjectPathsFor(P);

         AssertForSpecs.AreEqualFormula(x2, x1);
      }
   }

   public class ConstantFormulaXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         ConstantFormula x1 = CreateObject<ConstantFormula>().WithName("Fortunato").WithDimension(DimensionLength).WithValue(5.3);
         x1.AddObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P2).WithAlias("Pitter"));
         x1.ResolveObjectPathsFor(P);

         ConstantFormula x2 = SerializeAndDeserialize(x1);

         x2.ResolveObjectPathsFor(P);

         AssertForSpecs.AreEqualConstantFormula(x2, x1);
      }
   }

   public class JacobiMatrixXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new JacobianMatrix(new[] { "P1", "P2"});
         var partialDerivatives = new PartialDerivatives("O1", x1.ParameterNames);
         partialDerivatives.AddPartialDerivative(new[] {1d, 2d});
         partialDerivatives.AddPartialDerivative(new[] {3d, 4d});
         x1.AddPartialDerivatives(partialDerivatives);

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualJacobianMatrix(x1, x2);
      }
   }

   public class TableFormulaXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         TableFormula x1 = CreateObject<TableFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         x1.XName = "Great X name";
         x1.YName = "Great Y name";
         x1.XDimension = DimensionMolarConcentration;
         x1.XDisplayUnit = DimensionMolarConcentration.DefaultUnit;
         x1.AddPoint(1, 10);
         x1.AddPoint(2, 20);
         x1.AddPoint(new ValuePoint(3, 30) {RestartSolver = true});
         x1.UseDerivedValues = false;

         TableFormula x2 = SerializeAndDeserialize(x1);
         x2.ResolveObjectPathsFor(P);

         AssertForSpecs.AreEqualTableFormula(x2, x1);
      }
   }

   public class ExplicitFormulaXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         ExplicitFormula x1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         x1.AddObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P2).WithAlias("Pitter"));
         x1.ResolveObjectPathsFor(P);
         x1.FormulaString = "1.1 + 2.2";

         ExplicitFormula x2 = SerializeAndDeserialize(x1);

         x2.ResolveObjectPathsFor(P);

         AssertForSpecs.AreEqualExplicitFormula(x2, x1);
      }

      [Test]
      public void TestSerializationWithDependencies()
      {
         ExplicitFormula x1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         x1.AddObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));
         x1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P2).WithAlias("Pitter"));
         x1.ResolveObjectPathsFor(P);
         x1.FormulaString = "Paul + Peter + Pitter";

         P1.Value = 1.0;
         P0.Value = 10.0;
         P2.Value = 2.0;

         ExplicitFormula x2 = SerializeAndDeserialize(x1);

         x2.ResolveObjectPathsFor(P);

         AssertForSpecs.AreEqualExplicitFormula(x2, x1);
      }
   }
}