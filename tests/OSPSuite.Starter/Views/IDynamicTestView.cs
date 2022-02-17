using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IDynamicTestView : IModalView<IDynamicTestPresenter>
   {
      void AddCollectorView(IView view);
   }
}