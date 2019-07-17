using System.Linq;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class PKParameterRepositoryXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerialization()
      {
         var x1 = new PKParameterRepository();
         x1.Add(new PKParameter {Description = "one pk parameter", Name = "Cmax", DisplayName = "The CMax", Dimension = DimensionLength});
         x1.Add(new PKParameter {Description = "one pk parameter", Name = "Tmax", DisplayName = "The TMax", Dimension = DimensionLength});
         var x2 = SerializeAndDeserialize(x1);

         Assert.AreEqual(x1.All().Count(), x2.All().Count());
         foreach (var pkParameterMetaData in x1.All())
         {
            AssertForSpecs.AreEqualPKParameter(pkParameterMetaData, x2.All().FindByName(pkParameterMetaData.Name));
         }
      }
   }
}