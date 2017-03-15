using OSPSuite.Core.Domain;

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
         Map(x => x.BuildConfiguration);
         Map(x => x.Model);
         Map(x => x.Creation);
      }
   }

   public class ModelCoreSimulationXmlSerializer : SimulationXmlSerializer<ModelCoreSimulation>
   {
   }
}