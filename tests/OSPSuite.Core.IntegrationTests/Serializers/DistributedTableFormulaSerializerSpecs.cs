using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Serializers
{
   public class DistributedTableFormulaSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Observation]
      public void TestSerialization()
      {
         var x1 = new DistributedTableFormula().WithName("TOTO");
         x1.AddPoint(0, 1, new DistributionMetaData {Deviation = 0.5, Distribution = DistributionType.Normal, Mean = 1});
         x1.AddPoint(2, 3, new DistributionMetaData {Deviation = 0.8, Distribution = DistributionType.LogNormal, Mean = 2});

         var x2 = SerializeAndDeserialize(x1);

         x2.AllPoints.Count().ShouldBeEqualTo(2);

         x2.AllPoints.ElementAt(0).X.ShouldBeEqualTo(0);
         x2.AllPoints.ElementAt(0).Y.ShouldBeEqualTo(1);

         x2.AllPoints.ElementAt(1).X.ShouldBeEqualTo(2);
         x2.AllPoints.ElementAt(1).Y.ShouldBeEqualTo(3);

         x2.AllDistributionMetaData().Count().ShouldBeEqualTo(2);
         var distributionMetaData = x2.AllDistributionMetaData().ElementAt(0);
         distributionMetaData.Deviation.ShouldBeEqualTo(0.5);
         distributionMetaData.Distribution.ShouldBeEqualTo(DistributionType.Normal);
         distributionMetaData.Mean.ShouldBeEqualTo(1);

         distributionMetaData = x2.AllDistributionMetaData().ElementAt(1);
         distributionMetaData.Deviation.ShouldBeEqualTo(0.8);
         distributionMetaData.Distribution.ShouldBeEqualTo(DistributionType.LogNormal);
         distributionMetaData.Mean.ShouldBeEqualTo(2);
      }
   }
}