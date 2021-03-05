using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services.ParameterIdentifications;

namespace OSPSuite.Presentation.UICommands
{
   public class ExportParameterIdentificationToMatlabUICommand : ObjectUICommand<ParameterIdentification>
   {
      private readonly IParameterIdentificationExportTask _parameterIdentificationExportTask;

      public ExportParameterIdentificationToMatlabUICommand(IParameterIdentificationExportTask parameterIdentificationExportTask)
      {
         _parameterIdentificationExportTask = parameterIdentificationExportTask;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationExportTask.ExportToMatlab(Subject);
      }
   }
}