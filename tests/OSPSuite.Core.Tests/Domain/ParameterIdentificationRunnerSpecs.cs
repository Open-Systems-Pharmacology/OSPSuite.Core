using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationRunner : ContextSpecification<IParameterIdentificationRunner>
   {
      protected IParameterIdentificationEngineFactory _engineFactory;
      protected IDialogCreator _dialogCreator;
      protected IEntityValidationTask _entityValidationTask;
      protected IOSPSuiteExecutionContext _context;
      protected ParameterIdentification _parameterIdentification;

      protected override void Context()
      {
         _engineFactory = A.Fake<IParameterIdentificationEngineFactory>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _entityValidationTask = A.Fake<IEntityValidationTask>();
         _context = A.Fake<IOSPSuiteExecutionContext>();
         sut = new ParameterIdentificationRunner(_engineFactory, _dialogCreator, _entityValidationTask, _context);

         _parameterIdentification = A.Fake<ParameterIdentification>();
      }
   }

   public class When_a_parameter_identification_is_finished_running : concern_for_ParameterIdentificationRunner
   {
      protected override void Context()
      {
         base.Context();
         var identificationEngine = A.Fake<IParameterIdentificationEngine>();
         A.CallTo(() => _engineFactory.Create()).Returns(identificationEngine);
         A.CallTo(() => _entityValidationTask.Validate(_parameterIdentification)).Returns(true);
         A.CallTo(() => identificationEngine.StartAsync(_parameterIdentification)).Returns(Task.Delay(100));
      }

      protected override void Because()
      {
         sut.Run(_parameterIdentification).Wait();
      }

      [Observation]
      public void should_notify_project_changed()
      {
         A.CallTo(() => _context.ProjectChanged()).MustHaveHappened();
      }
   }

   public class when_running_a_parameter_identification : concern_for_ParameterIdentificationRunner
   {
      protected override void Because()
      {
         sut.Run(_parameterIdentification).Wait();
      }

      [Observation]
      public void should_execute_context_load()
      {
         A.CallTo(() => _context.Load(_parameterIdentification)).MustHaveHappened();
      }
   }
}