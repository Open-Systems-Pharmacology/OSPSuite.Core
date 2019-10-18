using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services.ParameterIdentifications;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_ExportParameterIdentificationToMatlabUICommand : ContextSpecification<ExportParameterIdentificationToMatlabUICommand>
   {
      protected IParameterIdentificationExportTask _parameterIdentificationExportTask;
      protected ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         _parameterIdentificationExportTask = A.Fake<IParameterIdentificationExportTask>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut = new ExportParameterIdentificationToMatlabUICommand(_parameterIdentificationExportTask) { Subject = _parameterIdentification };
      }
   }

   public class When_executing_the_export_command : concern_for_ExportParameterIdentificationToMatlabUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void the_export_task_should_be_used_to_export_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentificationExportTask.ExportToMatlab(_parameterIdentification)).MustHaveHappened();
      }
   }
}
