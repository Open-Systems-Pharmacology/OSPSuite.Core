using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationFactory
   {
      ParameterIdentification Create();
   }

   public class ParameterIdentificationFactory : IParameterIdentificationFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IContainerTask _containerTask;

      public ParameterIdentificationFactory(IObjectBaseFactory objectBaseFactory, IOSPSuiteExecutionContext executionContext, IContainerTask containerTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _executionContext = executionContext;
         _containerTask = containerTask;
      }

      public ParameterIdentification Create()
      {
         var parameterIdentification = _objectBaseFactory.Create<ParameterIdentification>();
         var project = _executionContext.Project;
         parameterIdentification.Name = _containerTask.CreateUniqueName(project.AllParameterIdentifications, Captions.ParameterIdentification.ParameterIdentificationDefaultName);
         parameterIdentification.Icon = ApplicationIcons.ParameterIdentification.IconName;
         parameterIdentification.IsLoaded = true;
         return parameterIdentification;
      }
   }
}