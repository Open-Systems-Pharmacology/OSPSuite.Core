using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ProcessBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         TransportBuilder x1 = CreateObject<TransportBuilder>().WithName("Priscilla.Builder").WithDimension(DimensionLength);
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         x1.CreateProcessRateParameter = true;
         IFormula f1 = CreateObject<ExplicitFormula>().WithDimension(DimensionLength).WithFormulaString("3*Patty");
         IFormulaUsablePath fup = new FormulaUsablePath(new[] {"Patricia"}).WithAlias("Patty").WithDimension(DimensionLength);
         f1.AddObjectPath(fup);
         //WithValue to avoid formula evaluation in McAssertForSpecs-comparison.
         IParameter p1 = CreateObject<Parameter>().WithName("Patricia").WithFormula(f1).WithValue(3.1);
         IParameter p2 = CreateObject<Parameter>().WithName("Pascal").WithFormula(f1).WithValue(3.2);

         x1.AddParameter(p1);
         x1.AddParameter(p2);

         ITransportBuilder x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualProcessBuilder(x1, x2);
      }
   }

   public class ActiveTransportBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         TransportBuilder x1 = CreateObject<TransportBuilder>().WithName("Acerola.Builder").WithDimension(DimensionLength);
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);

         IFormula f1 = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(2.3);
         IParameter p1 = CreateObject<Parameter>().WithName("Patricia").WithFormula(f1).WithDimension(DimensionLength);

         x1.AddParameter(p1);
         x1.Name = "Tranquilo";
         x1.CreateProcessRateParameter = true;
         x1.ProcessRateParameterPersistable = true;

         ITransportBuilder x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualTransportBuilder(x1, x2);
      }
   }

   public class PassiveTransportBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         TransportBuilder x1 = CreateObject<TransportBuilder>().WithName("Passionata.Builder");
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);


         x1.AddParameter(CreateObject<Parameter>().WithName("Patricia").WithValue(5.1));
         x1.TransportType = TransportType.Diffusion;
         x1.SourceCriteria = new DescriptorCriteria {new MatchTagCondition("Venous")};
         x1.TargetCriteria = new DescriptorCriteria {new NotMatchTagCondition("Venous")};

         x1.MoleculeList.ForAll = !x1.MoleculeList.ForAll;
         x1.AddMoleculeName("A");
         x1.AddMoleculeNameToExclude("B");

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualTransportBuilder(x1, x2);
      }
   }

   public class ReactionPartnerBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new ReactionPartnerBuilder("H2O", 3.1);
         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualReactionPartnerBuilder(x1, x2);
      }
   }

   public class ReactionBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = CreateObject<ReactionBuilder>().WithName("Passionata.Builder");
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         x1.ContainerCriteria = Create.Criteria(x => x.With("TOTO"));


         x1.AddEduct(new ReactionPartnerBuilder("H2", 2.0));
         x1.AddEduct(new ReactionPartnerBuilder("O2", 1.0));
         x1.AddProduct(new ReactionPartnerBuilder("H2O", 2.0));
         x1.AddModifier("nix");

         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualReactionBuilder(x1, x2);
      }
   }
}