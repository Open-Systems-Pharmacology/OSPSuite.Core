using OSPSuite.Presentation.Presenters.Parameters;

namespace OSPSuite.Presentation.Views.Parameters
{
   public interface IEditTableParameterView : IModalView<IEditTableParameterPresenter>
   {
      void AddView(IView baseView);
      void AddChart(IView baseView);
   }
}