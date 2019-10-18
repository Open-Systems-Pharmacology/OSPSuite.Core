using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public abstract class concern_for_EditMultipleMetaDataUICommand : ContextSpecification<EditMultipleMetaDataUICommand>
   {
      protected IEditObservedDataTask _task = A.Fake<IEditObservedDataTask>();
      protected IEnumerable<DataRepository> _repositories;

      protected override void Context()
      {
         base.Context();
         _repositories= A.Fake<IEnumerable<DataRepository>>();
         sut = new EditMultipleMetaDataUICommand(_task);
         sut.For(_repositories);
      }
   }

   public class When_editing_multiple_meta_data_for_a_set_of_repositories : concern_for_EditMultipleMetaDataUICommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_leverage_the_observed_data_task_to_edit_the_meta_data()
      {
         A.CallTo(() => _task.EditMultipleMetaDataFor(_repositories)).MustHaveHappened();
      }
   }
}