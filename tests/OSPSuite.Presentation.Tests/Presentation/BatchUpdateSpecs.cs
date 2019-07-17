using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_BatchUpdate : ContextSpecification<BatchUpdate>
   {
      protected IBatchUpdatable _batchUpdatable;

      protected override void Context()
      {
         _batchUpdatable = A.Fake<IBatchUpdatable>();
      }
   }

   public class When_the_batch_update_is_created : concern_for_BatchUpdate
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchUpdatable.Updating).Returns(false);
         sut = new BatchUpdate(_batchUpdatable);
      }

      [Observation]
      public void the_view_should_start_the_update_mode()
      {
         A.CallTo(() => _batchUpdatable.BeginUpdate()).MustHaveHappened();
      }
   }

   public class When_the_batch_update_is_disposed_and_the_batch_mode_was_started_within_the_disposer : concern_for_BatchUpdate
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchUpdatable.Updating).Returns(false);
         sut = new BatchUpdate(_batchUpdatable);
      }

      protected override void Because()
      {
         sut.Dispose();
      }

      [Observation]
      public void the_view_should_end_the_update()
      {
         A.CallTo(() => _batchUpdatable.EndUpdate()).MustHaveHappened();
      }
   }

   public class When_the_batch_update_is_disposed_and_the_batch_mode_was_not_started_within_the_disposer : concern_for_BatchUpdate
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _batchUpdatable.Updating).Returns(true);
         sut = new BatchUpdate(_batchUpdatable);
      }

      protected override void Because()
      {
         sut.Dispose();
      }

      [Observation]
      public void the_view_should_end_the_update()
      {
         A.CallTo(() => _batchUpdatable.EndUpdate()).MustNotHaveHappened();
      }
   }
}