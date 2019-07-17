using NUnit.Framework;
using OSPSuite.Core.Comparison;
using OSPSuite.Core.Helpers;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Serializers
{
   public class ComparerSettingsXmlSerializerSpecs : ModellingXmlSerializerBaseSpecs
   {
      [Test]
      public void TestSerializationComparerSettings()
      {
         var x1 = new ComparerSettings
         {
            FormulaComparison = FormulaComparison.Value,
            OnlyComputingRelevant = true,
            RelativeTolerance = 1e-2,
            ShowValueOrigin = false
         };


         var x2 = SerializeAndDeserialize(x1);

         AssertForSpecs.AreEqualComparerSettings(x1, x2);
      }
   }
}