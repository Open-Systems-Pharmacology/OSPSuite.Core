using OSPSuite.Core.Comparison;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ComparerSettingsXmlSerializer : OSPSuiteXmlSerializer<ComparerSettings>
   {
      public override void PerformMapping()
      {
         Map(x => x.FormulaComparison);
         Map(x => x.OnlyComputingRelevant);
         Map(x => x.RelativeTolerance);
         Map(x => x.ShowValueOrigin);
      }
   }
}