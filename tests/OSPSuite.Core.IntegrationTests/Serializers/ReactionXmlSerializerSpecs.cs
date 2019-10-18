using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serializers
{
   public class ReactionXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyReaction()
      {
         Reaction x1 = CreateObject<Reaction>().WithName("Regina").WithDimension(DimensionLength);
         IContainer c1 = CreateObject<Container>().WithName("Conrad");
         IMoleculeAmount educt1 = CreateObject<MoleculeAmount>().WithName("Eduard").WithParentContainer(c1);
         IMoleculeAmount educt2 = CreateObject<MoleculeAmount>().WithName("Edward").WithParentContainer(c1);
         x1.AddEduct(new ReactionPartner(1.1, educt1));
         x1.AddEduct(new ReactionPartner(1.2, educt2));

         IMoleculeAmount product1 = CreateObject<MoleculeAmount>().WithName("Proton").WithParentContainer(c1);
         IMoleculeAmount product2 = CreateObject<MoleculeAmount>().WithName("Prosit").WithParentContainer(c1);
         x1.AddProduct(new ReactionPartner(1.1, product1));
         x1.AddProduct(new ReactionPartner(1.2, product2));
         x1.AddModifier("mod");

         var cont1 = new Container {c1, x1}.WithId("toto");
         var cont2 = SerializeAndDeserialize(cont1);
         var x2 = cont2.FindByName(x1.Name).DowncastTo<Reaction>();

         AssertForSpecs.AreEqualReaction(x2, x1);
      }
   }
}