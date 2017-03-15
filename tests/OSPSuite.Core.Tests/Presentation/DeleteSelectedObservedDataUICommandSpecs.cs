using System.Collections.Generic;
using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_DeleteSelectedObservedDataUICommand : ContextSpecification<DeleteSelectedObservedDataUICommand>
   {
      protected IObservedDataTask _observedDataTask;
      protected IEnumerable<DataRepository> _repositories;

      protected override void Context()
      {
         _observedDataTask = A.Fake<IObservedDataTask>();
         _repositories = A.Fake<IEnumerable<DataRepository>>();

         sut = new DeleteSelectedObservedDataUICommand(_observedDataTask);
         sut.For(_repositories);
      }
   }

   public class When_executing_the_delete_selected_observed_data_command : concern_for_DeleteSelectedObservedDataUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_leverage_the_observed_data_task_to_remove_the_observed_data()
      {
         A.CallTo(() => _observedDataTask.Delete(_repositories)).MustHaveHappened();
      }
   }
}