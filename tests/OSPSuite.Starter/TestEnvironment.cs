using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter
{
   public class TestEnvironment
   {
      protected IModel _model;

      public TestEnvironment()
      {
         var simulationConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateSimulationConfiguration();
         var modelConstructor = IoC.Resolve<IModelConstructor>();
         var result = modelConstructor.CreateModelFrom(simulationConfiguration, "MyModel");
         _model = result.Model;
      }

      public IModel Model => _model;
   }
}