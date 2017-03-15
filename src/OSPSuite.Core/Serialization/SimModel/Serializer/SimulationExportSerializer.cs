using OSPSuite.Core.Serialization.SimModel.DTO;

namespace OSPSuite.Core.Serialization.SimModel.Serializer
{
   internal class SimulationExportSerializer : SimModelSerializerBase<SimulationExport>
   {
      public override void PerformMapping()
      {
         Map(x => x.ObjectPathDelimiter);
         Map(x => x.Version);
         MapEnumerable(x => x.ObserverList);
         MapEnumerable(x => x.EventList);
         MapEnumerable(x => x.FormulaList);
         MapEnumerable(x => x.VariableList);
         MapEnumerable(x => x.ParameterList);
         Map(x => x.Solver);
         Map(x => x.OutputSchema);
         ElementName = SimModelSchemaConstants.Simulation;
      }
   }
}