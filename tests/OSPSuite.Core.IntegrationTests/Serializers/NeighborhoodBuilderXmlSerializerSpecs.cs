using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serializers
{
   public class NeighborhoodBuilderXmlSerializerSpecs : MiniEnvironmentSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = CreateObject<NeighborhoodBuilder>().WithName("Nena.Builder")
            .WithMode(ContainerMode.Physical)
            .WithFirstNeighbor(C1)
            .WithSecondNeighbor(C2);

         x1.Add(CreateObject<Container>().WithMode(ContainerMode.Logical).WithName(Constants.MOLECULE_PROPERTIES));

         var K = CreateObject<Parameter>().WithName("K").WithFormula(CreateObject<ConstantFormula>().WithValue(23.4));
         x1.MoleculeProperties.Add(K);
         x1.AddParameter(CreateObject<Parameter>().WithName("Patricia").WithValue(3.1));

         var cont1 = new Container {C1, x1}.WithId("toto");
         var cont2 = SerializeAndDeserialize(cont1);
         var x2 = cont2.FindByName(x1.Name).DowncastTo<NeighborhoodBuilder>();
         AssertForSpecs.AreEqualNeighborhoodBuilder(x1, x2);
      }
   }
}