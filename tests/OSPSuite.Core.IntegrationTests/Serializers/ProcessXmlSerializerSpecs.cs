using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ProcessXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationEmptyProcess()
      {
         Transport x1 = CreateObject<Transport>().WithName("Proton");
         IProcess x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualProcess(x2, x1);
      }

      [Test]
      public void TestSerializationNonEmptyProcess()
      {
         Transport x1 = CreateObject<Transport>()
            .WithName("Proton").WithDimension(DimensionLength);
         x1.Formula = CreateObject<ConstantFormula>().WithDimension(DimensionLength).WithValue(23.4);

         IProcess x2 = SerializeAndDeserialize(x1);
         AssertForSpecs.AreEqualProcess(x2, x1);
      }
   }
}