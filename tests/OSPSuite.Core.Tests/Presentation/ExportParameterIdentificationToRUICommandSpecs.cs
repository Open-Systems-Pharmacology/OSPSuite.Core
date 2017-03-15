using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Services.ParameterIdentifications;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ExportParameterIdentificationToRUICommand : ContextSpecification<ExportParameterIdentificationToRUICommand>
   {
      protected IParameterIdentificationExportTask _parameterIdentificationExportTask;
      protected ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         _parameterIdentificationExportTask = A.Fake<IParameterIdentificationExportTask>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         sut = new ExportParameterIdentificationToRUICommand(_parameterIdentificationExportTask) { Subject = _parameterIdentification };
      }
   }

   public class When_export_a_parameter_identification_to_R : concern_for_ExportParameterIdentificationToRUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void the_export_task_should_be_used_to_export_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentificationExportTask.ExportToR(_parameterIdentification)).MustHaveHappened();
      }
   }
}
