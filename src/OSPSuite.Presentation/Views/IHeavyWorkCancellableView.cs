using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IHeavyWorkCancellableView : IModalView<IHeavyWorkCancellablePresenter>
   {
      void Close();
   }
}