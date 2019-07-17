using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CloseAllMdiViewCommand : ContextSpecification<CloseAllMdiViewCommand>
   {
      protected IApplicationController _applicationController;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         sut = new CloseAllMdiViewCommand(_applicationController);
      }
   }

   public class When_executing_the_close_all_view_command : concern_for_CloseAllMdiViewCommand
   {
      protected override void Because()
      {
         sut.Execute();
      }

      [Observation]
      public void should_close_all_visible_views()
      {
         A.CallTo(() => _applicationController.CloseAll()).MustHaveHappened();
      }
   }
}