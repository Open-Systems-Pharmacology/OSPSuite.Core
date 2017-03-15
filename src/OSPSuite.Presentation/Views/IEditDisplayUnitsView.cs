using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IEditDisplayUnitsView : IModalView<IEditDisplayUnitsPresenter>
   {
      void AddUnitsView(IView view);
   }
}