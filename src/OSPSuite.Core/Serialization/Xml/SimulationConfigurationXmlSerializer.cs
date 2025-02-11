using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public abstract class SimulationConfigurationXmlSerializer<T> : OSPSuiteXmlSerializer<T> where T : SimulationConfiguration
   {
      protected SimulationConfigurationXmlSerializer()
      {
      }

      protected SimulationConfigurationXmlSerializer(string name) : base(name)
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.Individual);
         Map(x => x.SimulationSettings);
         Map(x => x.CreateAllProcessRateParameters);
         MapEnumerable(x => x.ModuleConfigurations, x => x.AddModuleConfiguration);
         MapEnumerable(x => x.ExpressionProfiles, x => x.AddExpressionProfile);
         MapEnumerable(x => x.AllCalculationMethods, x => x.AddCalculationMethod);
      }
   }

   public class SimulationConfigurationXmlSerializer : SimulationConfigurationXmlSerializer<SimulationConfiguration>
   {
   }
}