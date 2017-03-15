using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IHeavyWorkView : IModalView<IHeavyWorkPresenter>
   {
      void Close();
   }
}