using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Commands
{
   public abstract class concern_for_RemoveObservedDataFromProjectCommand : ContextSpecification<RemoveObservedDataFromProjectCommand>
   {
      protected DataRepository _observedData;

      protected override void Context()
      {
         _observedData = A.Fake<DataRepository>().WithId("Repository");
         sut = new RemoveObservedDataFromProjectCommand(_observedData);
      }
   }

   class When_executing_the_remove_observed_data_from_project_command : concern_for_RemoveObservedDataFromProjectCommand
   {
      private IOSPSuiteExecutionContext _context;
      private IProject _project;

      protected override void Context()
      {
         base.Context();
         _context = A.Fake<IOSPSuiteExecutionContext>();
         _project = A.Fake<IProject>();
         A.CallTo(() => _context.Project).Returns(_project);
      }

      protected override void Because()
      {
         sut.Execute(_context);
      }

      [Observation]
      public void should_remove_the_observed_data_from_the_project()
      {
         A.CallTo(() => _project.RemoveObservedData(_observedData)).MustHaveHappened();
      }
   }
}	