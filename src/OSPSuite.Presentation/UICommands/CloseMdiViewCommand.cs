using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.UICommands
{
   public class CloseMdiViewCommand : ObjectUICommand<IMdiChildView>
   {
      protected override void PerformExecute()
      {
         Subject.Presenter.Close();
      }
   }
}