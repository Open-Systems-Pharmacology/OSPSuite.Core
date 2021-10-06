using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Import;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Core.Commands
{
   public class RemoveImporterConfigurationFromProjectCommand : OSPSuiteReversibleCommand<IOSPSuiteExecutionContext>
   {
      private ImporterConfiguration _configuration;
      private byte[] _serializationStream;

      public RemoveImporterConfigurationFromProjectCommand(ImporterConfiguration configuration)
      {
         _configuration = configuration;
         CommandType = Assets.Command.CommandTypeDelete;
         ObjectType = ObjectTypes.ImporterConfiguration;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         var project = context.Project;
         project.RemoveImporterConfiguration(_configuration);
         Description = Assets.Command.RemoveObservedDataFromProjectDescription(_configuration.FileName, project.Name);
         _serializationStream = context.Serialize(_configuration);
         context.Unregister(_configuration);
      }

      protected override void ClearReferences()
      {
         _configuration = null;
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new AddImporterConfigurationToProjectCommand(_configuration).AsInverseFor(this);
      }

      public override void RestoreExecutionData(IOSPSuiteExecutionContext context)
      {
         _configuration = context.Deserialize<ImporterConfiguration>(_serializationStream);
      }
   }
}
