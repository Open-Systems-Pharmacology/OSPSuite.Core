using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Import;

namespace OSPSuite.Core.Commands
{
   public class AddImporterConfigurationToProjectCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private readonly string _configurationId;
      private ImporterConfiguration _configuration;

      public AddImporterConfigurationToProjectCommand(ImporterConfiguration configuration)
      {
         _configuration = configuration;
         _configurationId = _configuration.Id;
         CommandType = Assets.Command.CommandTypeAdd;
         ObjectType = ObjectTypes.ImporterConfiguration;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var project = context.Project;
         project.AddImporterConfiguration(_configuration);
         Description = Assets.Command.AddObservedDataToProjectDescription(_configuration.FileName, project.Name);
         context.Register(_configuration);
      }

      protected override void ClearReferences()
      {
         _configuration = null;
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new RemoveImporterConfigurationFromProjectCommand(_configuration).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _configuration = context.Project.ImporterConfigurationBy(_configurationId);
      }
   }
}
