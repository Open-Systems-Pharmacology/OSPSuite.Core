using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class IndividualBuildingBlockXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Observation]
      public void TestSerializationIndividualBuildingBlock()
      {
         var x1 = new IndividualBuildingBlock
         {
            new IndividualParameter
            {
               ContainerPath = new ObjectPath("A", "B", "C"),
               Name = "P1",
               Value = 10,
               DistributionType = DistributionType.Normal
            },
            new IndividualParameter
            {
               ContainerPath = new ObjectPath("A", "B", "C", "D"),
               Name = "P2",
               Value = 20,
            }
         }.WithId("test");

         var x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualIndividualBuildingBlock(x1, x2);
      }
   }
}