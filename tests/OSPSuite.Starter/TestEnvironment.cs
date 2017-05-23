using OSPSuite.Utility.Container;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Starter
{
   public class TestEnvironment
   {
      protected IModel _model;
 
      public TestEnvironment()
      {
         var buildConfiguration = IoC.Resolve<ModelHelperForSpecs>().CreateBuildConfiguration();
         var modelConstructor = IoC.Resolve<IModelConstructor>();
         var result = modelConstructor.CreateModelFrom(buildConfiguration, "MyModel");
         _model = result.Model;
      }

      public IModel Model => _model;
   }
}