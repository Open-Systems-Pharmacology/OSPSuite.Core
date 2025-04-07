using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ObserverBuilderXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationObserverBuilderWithoutFormula()
      {
         var x1 = CreateObject<ContainerObserverBuilder>().WithName("Oberon.Builder");
         x1.MoleculeList.ForAll = true;
         x1.AddMoleculeName("H2O");
         x1.AddMoleculeName("CO2");
         x1.AddMoleculeNameToExclude("NO2");

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualObserverBuilder(x2, x1);
      }

      [Test]
      public void TestSerializationObserverBuilderWithConstantFormula()
      {
         var x1 = CreateObject<ContainerObserverBuilder>()
            .WithName("Obasi.Builder").WithDimension(DimensionLength);
         x1.MoleculeList.ForAll = false;
         x1.AddMoleculeName("H2O");
         x1.AddMoleculeName("CO2");

         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualObserverBuilder(x2, x1);
      }

      [Test]
      public void TestSerializationObserverBuilderWithExplicitFormula()
      {
         var x1 = CreateObject<ContainerObserverBuilder>().WithName("Oberon.Builder");
         x1.ForAll = false;
         x1.AddMoleculeName("H2O");
         x1.AddMoleculeName("CO2");

         ExplicitFormula f1 = CreateObject<ExplicitFormula>().WithName("Fortunato").WithDimension(DimensionLength);
         var fup = new FormulaUsablePath(new string[] {"aa", "bb"}).WithAlias("b").WithDimension(DimensionLength);
         f1.AddObjectPath(fup);

         IFormulaCache formulaCache = new FormulaCache();
         formulaCache.Add(f1);

         x1.Formula = formulaCache[f1.Id];

         //SerializeAndDeserializeGeneral(formulaCache);
         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualObserverBuilder(x2, x1);
      }
   }

   public class AmountObserverBuilderXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationWithoutFormula()
      {
         AmountObserverBuilder x1 = CreateObject<AmountObserverBuilder>();
         x1.ForAll = true;
         x1.ContainerCriteria = Create.Criteria(x => x.With("Organ").And.Not("Compartment").And.InContainer("Liver").And.NotInContainer("Cell"));
         x1.Dimension = DimensionLength;
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualAmountObserverBuilder(x2, x1);
      }
   }

   public class MatchTagConditionXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         MatchTagCondition x1 = new MatchTagCondition("Franz");

         MatchTagCondition x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualMatchTagCondition(x2, x1);
      }
   }

   [TestFixture]
   public class NotMatchTagConditionXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         NotMatchTagCondition x1 = new NotMatchTagCondition("Franz");

         NotMatchTagCondition x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualNotMatchTagCondition(x2, x1);
      }
   }

   [TestFixture]
   public class InContainerConditionXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new InContainerCondition("Franz");

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualInContainerCondition(x2, x1);
      }
   }

   [TestFixture]
   public class NotInContainerConditionXmlSerializerSpecs : ModelingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new NotInContainerCondition("Franz");

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualNotInContainerCondition(x2, x1);
      }
   }
}