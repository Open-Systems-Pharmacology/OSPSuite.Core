﻿using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Serializers
{
   public class NeighborhoodXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyNeighborhood()
      {
         //Neighborhood x1 = new Neighborhood().WithName("otto"); Does not help, because ObjectBaseFactory is used in Deserialization
         var x1 = CreateObject<Neighborhood>().WithName("Nele");
         Assert.IsNull(x1.FirstNeighbor);
         Assert.IsNull(x1.SecondNeighbor);

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualNeighborhood(x2, x1);
      }

      [Test]
      public void TestSerializationNeighborhoodWithContainers()
      {
         Neighborhood x1 = CreateObject<Neighborhood>().WithName("Nele");

         Container c1 = CreateObject<Container>().WithName("Carla").WithMode(ContainerMode.Physical);
         Container c2 = CreateObject<Container>().WithName("Conrad").WithMode(ContainerMode.Physical);
         x1.FirstNeighbor = c1;
         x1.SecondNeighbor = c2;


         var cont1 = new Container {c1, x1, c2}.WithId("toto");
         var cont2 = SerializeAndDeserialize(cont1);
         var x2 = cont2.FindByName(x1.Name).DowncastTo<Neighborhood>();

         AssertForSpecs.AreEqualNeighborhood(x2, x1);
      }
   }
}