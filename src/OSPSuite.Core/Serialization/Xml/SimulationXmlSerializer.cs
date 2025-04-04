using OSPSuite.Core.Domain;
using OSPSuite.Serializer;
using static OSPSuite.Core.Domain.Constants.Serialization;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class SimulationXmlSerializer<TSimulation> : ObjectBaseXmlSerializer<TSimulation> where TSimulation : IModelCoreSimulation
   {
      protected SimulationXmlSerializer()
      {
      }

      protected SimulationXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.Configuration).WithMappingName(SIMULATION_CONFIGURATION);
         Map(x => x.Model);
         Map(x => x.Creation);
         MapEnumerable(x => x.EntitySources, x => x.EntitySources.Add);
      }
   }

   public class ModelCoreSimulationXmlSerializer : SimulationXmlSerializer<ModelCoreSimulation>
   {
   }
}