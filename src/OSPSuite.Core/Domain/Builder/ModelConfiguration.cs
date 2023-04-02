namespace OSPSuite.Core.Domain.Builder
{
   public class ModelConfiguration
   {
      public IModel Model { get; }
      public SimulationConfiguration SimulationConfiguration { get; }

      public ModelConfiguration(IModel model, SimulationConfiguration simulationConfiguration)
      {
         Model = model;
         SimulationConfiguration = simulationConfiguration;
      }

      public bool ShouldValidate => SimulationConfiguration.ShouldValidate;

      public void Deconstruct(out IModel model, out SimulationConfiguration simulationConfiguration)
      {
         model = Model;
         simulationConfiguration = SimulationConfiguration;
      }
   }
}