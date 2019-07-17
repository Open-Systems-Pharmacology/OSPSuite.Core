using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services.ParameterIdentifications;

namespace OSPSuite.Presentation.UICommands
{
   public class ExportParameterIdentificationToRUICommand : ObjectUICommand<ParameterIdentification>
   {
      private readonly IParameterIdentificationExportTask _parameterIdentificationExportTask;

      public ExportParameterIdentificationToRUICommand(IParameterIdentificationExportTask parameterIdentificationExportTask)
      {
         _parameterIdentificationExportTask = parameterIdentificationExportTask;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationExportTask.ExportToR(Subject);
      }
   }
}