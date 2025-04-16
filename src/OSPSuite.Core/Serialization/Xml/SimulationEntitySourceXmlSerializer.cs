using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SimulationEntitySourceXmlSerializer : OSPSuiteXmlSerializer<SimulationEntitySource>
   {
      public override void PerformMapping()
      {
         Map(x => x.SimulationEntityPath);
         Map(x => x.BuildingBlockName);
         Map(x => x.BuildingBlockType);
         Map(x => x.ModuleName);
         Map(x => x.SourcePath);
      }
   }
}