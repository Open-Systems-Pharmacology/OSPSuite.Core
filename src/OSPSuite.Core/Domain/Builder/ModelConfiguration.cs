namespace OSPSuite.Core.Domain.Builder
{
   internal class ModelConfiguration
   {
      public SimulationBuilder SimulationBuilder { get; }
      public IModel Model { get; }
      public SimulationConfiguration SimulationConfiguration { get; }

      public ModelConfiguration(IModel model, SimulationConfiguration simulationConfiguration, SimulationBuilder simulationBuilder)
      {
         Model = model;
         SimulationConfiguration = simulationConfiguration;
         SimulationBuilder = simulationBuilder;
      }

      public bool ShouldValidate => SimulationConfiguration.ShouldValidate;

      public void Deconstruct(out IModel model, out SimulationConfiguration simulationConfiguration, out SimulationBuilder simulationBuilder)
      {
         model = Model;
         simulationConfiguration = SimulationConfiguration;
         simulationBuilder = SimulationBuilder;
      }


      public void Deconstruct(out IModel model, out SimulationBuilder simulationBuilder)
      {
         model = Model;
         simulationBuilder = SimulationBuilder;
      }
   }
}