using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class SimulationXmlSerializerSpecs : ModellingXmlSerializerWithModelBaseSpecs
   {

      [Test]
      public void TestComplexSimulation()
      {
         //CONTEXT
         ModelCoreSimulation x1 = _simulation as ModelCoreSimulation;
         Assert.IsNotNull(x1);

         //ACT
         IModelCoreSimulation x2 = SerializeAndDeserialize(x1);
         var refResolver = new ReferencesResolver();
         refResolver.ResolveReferencesIn(x2.Model);
         //ASSERT
         AssertForSpecs.AreEqualSimulation(x2, x1);
      }
   }
}