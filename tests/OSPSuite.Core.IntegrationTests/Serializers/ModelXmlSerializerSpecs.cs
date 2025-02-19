using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   internal class ModelXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestSimpleModelContainer()
      {
         Model x1 = CreateObject<Model>().WithName("Monica");
         x1.Root = CreateObject<Container>().WithName("Root");
         IContainer c1 = CreateObject<Container>().WithName("Conrad");
         IContainer c2 = CreateObject<Container>().WithName("Carla");
         x1.Root.Add(c1);
         x1.Root.Add(c2);

         x1.Neighborhoods = CreateObject<Container>().WithName(Constants.NEIGHBORHOODS);
         var n12 = CreateObject<Neighborhood>().WithName("Nina").WithFirstNeighbor(c1).WithSecondNeighbor(c2);
         x1.Neighborhoods.Add(n12);

         Model x2 = SerializeAndDeserialize(x1);


         AssertForSpecs.AreEqualModel(x1, x2);
      }

      [Test]
      public void TestComplexModelContainer()
      {
         var x1 = _model;
         Assert.IsNotNull(x1);

         var x2 = SerializeAndDeserialize(x1);
         var refResolver = new ReferencesResolver();
         refResolver.ResolveReferencesIn(x2);
         AssertForSpecs.AreEqualModel(x1, x2);
      }
   }
}