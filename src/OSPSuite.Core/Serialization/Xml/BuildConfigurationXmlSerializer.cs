using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class BuildConfigurationXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : IBuildConfiguration
   {
      protected BuildConfigurationXmlSerializer()
      {
      }

      protected BuildConfigurationXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Molecules);
         Map(x => x.Observers);
         Map(x => x.EventGroups);
         Map(x => x.PassiveTransports);
         Map(x => x.Reactions);
         Map(x => x.SpatialStructure);
         Map(x => x.ParameterStartValues);
         Map(x => x.MoleculeStartValues);
         Map(x => x.SimulationSettings);
         MapEnumerable(x => x.AllCalculationMethods(), x => x.AddCalculationMethod);
      }
   }

   public class BuildConfigurationXmlSerializer : BuildConfigurationXmlSerializer<BuildConfiguration>
   {
   }
}