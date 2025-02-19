using NUnit.Framework;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   internal class SimulationXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {
      [Test]
      public void TestComplexSimulation()
      {
         //CONTEXT
         var x1 = _simulation;
         Assert.IsNotNull(x1);

         //ACT
         var x2 = SerializeAndDeserialize(x1);
         var refResolver = new ReferencesResolver();
         refResolver.ResolveReferencesIn(x2.Model);
         //ASSERT
         AssertForSpecs.AreEqualSimulation(x2, x1);
      }
   }
}