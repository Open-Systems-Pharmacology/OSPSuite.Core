using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class MoleculeBuilderXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         MoleculeBuilder x1 = CreateObject<MoleculeBuilder>().WithName("Monica.Builder");
         x1.IsFloating = true;
         x1.QuantityType = QuantityType.Metabolite;
         x1.DefaultStartFormula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);
         TransportBuilder t1 = CreateObject<TransportBuilder>().WithName("Passive Transport");
         IFormula consFormula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(5);
         IParameter transporterParameter = CreateObject<Parameter>().WithName("Passive Transport Param").WithFormula(consFormula).WithMode(ParameterBuildMode.Property)
            .WithDimension(DimensionLength);
         t1.AddParameter(transporterParameter);
         IFormula f1 = CreateObject<ExplicitFormula>().WithDimension(DimensionLength).WithFormulaString("3*Patty");
         IFormulaUsablePath fup = new FormulaUsablePath(new[] {"Patricia"}).WithAlias("Patty").WithDimension(DimensionLength);
         f1.AddObjectPath(fup);
         IParameter p1 = CreateObject<Parameter>().WithName("Patricia").WithFormula(f1).WithValue(3.1).WithMode(ParameterBuildMode.Property);
         IParameter p2 = CreateObject<Parameter>().WithName("Pascal").WithFormula(f1).WithValue(3.2).WithMode(ParameterBuildMode.Local);
         IParameter p3 = CreateObject<Parameter>().WithName("Paul").WithFormula(f1).WithValue(3.3);

         x1.AddParameter(p1);
         x1.AddParameter(p2);

         var atbc1 = CreateObject<TransporterMoleculeContainer>().WithName("Tranquilo");
         ITransportBuilder atb1 = CreateObject<TransportBuilder>();
         atb1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(2.5);
         atb1.AddParameter(p3);
         atb1.Name = "Tranquilo";
         atbc1.AddActiveTransportRealization(atb1);
         var atbc2 = CreateObject<TransporterMoleculeContainer>().WithName("Tranquilo2");
         ITransportBuilder atb2 = CreateObject<TransportBuilder>();
         atb2.Formula = f1;
         atb2.Name = "Tranquilo2";
         atbc2.AddActiveTransportRealization(atb2);
         x1.AddTransporterMoleculeContainer(atbc1);
         x1.AddTransporterMoleculeContainer(atbc2);
         x1.IsXenobiotic = !x1.IsXenobiotic;


         var interactionContainer = CreateObject<InteractionContainer>().WithName("Interactions");
         x1.AddInteractionContainer(interactionContainer);
         
         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualMoleculeBuilder(x1, x2);
      }
   }
}