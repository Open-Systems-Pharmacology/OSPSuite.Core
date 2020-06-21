using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class QuantityXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyQuantity()
      {
         Observer x1 = CreateObject<Observer>();
         x1.NegativeValuesAllowed = true;
         IQuantity x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantity(x2, x1);
      }

      [Test]
      public void TestSerializationQuantityWithFixedValue()
      {
         Observer x1 = CreateObject<Observer>().WithName("Quentin").WithDimension(DimensionLength);
         x1.Persistable = true;
         x1.NegativeValuesAllowed = false;
         x1.IsFixedValue = true;
         x1.Value = 2.3;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantity(x2, x1);
      }

      [Test]
      public void TestSerializationQuantityWithConstantFormula()
      {
         Observer x1 = CreateObject<Observer>().WithName("Quentin").WithDimension(DimensionLength);
         x1.IsFixedValue = false;
         x1.Persistable = false;
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualQuantity(x2, x1);
      }

      [Test]
      public void TestSerializationQuantityWithExplicitFormula()
      {
         Observer x1 = CreateObject<Observer>().WithName("Quentin").WithDimension(DimensionLength);
         x1.ParentContainer = C1;
         x1.IsFixedValue = false;
         x1.Persistable = false;
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);

         ExplicitFormula f1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         f1.AddObjectPath(ObjectPathFactory.CreateAbsoluteFormulaUsablePath(P1));
         f1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P0));
         f1.AddObjectPath(ObjectPathFactory.CreateRelativeFormulaUsablePath(P, P2).WithAlias("Pitter"));
         f1.ResolveObjectPathsFor(P);
         f1.FormulaString = "Paul + Peter + Pitter";

         P1.Value = 1.0;
         P0.Value = 10.0;
         P2.Value = 2.0;

         x1.Formula = f1;

         IQuantity x2 = SerializeAndDeserialize(x1);
         x2.ParentContainer = C1;
         x2.Formula.ResolveObjectPathsFor(x2);

         AssertForSpecs.AreEqualQuantity(x2, x1);
      }
   }

   public class ParameterXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationParameterWithRHSConstantFormula()
      {
         Parameter x1 = CreateObject<Parameter>().WithName("Quentin").WithDimension(DimensionLength);
         x1.IsFixedValue = false;
         x1.Persistable = false;
         x1.BuildingBlockType = PKSimBuildingBlockType.Protocol;
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(1.2);
         x1.RHSFormula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(3.4);
         x1.CanBeVaried = false;
         x1.DisplayUnit = DimensionLength.Unit("cm");
         x1.ValueOrigin.Description = "This is the value description";
         x1.ValueOrigin.Source = ValueOriginSources.Internet;
         x1.ValueOrigin.Id = 5;
         x1.ValueOrigin.Method = ValueOriginDeterminationMethods.ParameterIdentification;
         x1.IsDefault = true;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualParameter(x2, x1);
      }
   }
}