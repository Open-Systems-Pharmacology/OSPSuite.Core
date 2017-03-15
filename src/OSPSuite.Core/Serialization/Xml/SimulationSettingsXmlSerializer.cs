using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SimulationSettingsXmlSerializer : BuildingBlockXmlSerializer<SimulationSettings>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.RandomSeed);
         Map(x => x.Solver);
         Map(x => x.OutputSchema);
         Map(x => x.OutputSelections);
         MapEnumerable(x => x.ChartTemplates, x => x.AddChartTemplate);
      }
   }
}