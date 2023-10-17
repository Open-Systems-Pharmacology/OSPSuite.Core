using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class ModelConfiguration
   {
      public SimulationBuilder SimulationBuilder { get; }
      public IModel Model { get; }
      public SimulationConfiguration SimulationConfiguration { get; }
      public ReplacementContext ReplacementContext { get; private set; }

      public ModelConfiguration(IModel model, SimulationConfiguration simulationConfiguration, SimulationBuilder simulationBuilder)
      {
         Model = model;
         SimulationConfiguration = simulationConfiguration;
         SimulationBuilder = simulationBuilder;
         UpdateReplacementContext();
      }

      public bool ShouldValidate => SimulationConfiguration.ShouldValidate;

      //This cannot be only in the constructor as the model structure needs to be created first
      public void UpdateReplacementContext()
      {
         ReplacementContext = new ReplacementContext(Model);
      }

      public void Deconstruct(out IModel model, out SimulationBuilder simulationBuilder, out ReplacementContext replacementContext)
      {
         model = Model;
         simulationBuilder = SimulationBuilder;
         replacementContext = ReplacementContext;
      }

      public void Deconstruct(out IModel model, out SimulationBuilder simulationBuilder)
      {
         model = Model;
         simulationBuilder = SimulationBuilder;
      }
   }
}