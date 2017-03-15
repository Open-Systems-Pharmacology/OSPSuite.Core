using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationChartFeedbackView : IView<IParameterIdentificationChartFeedbackPresenter>
   {
      void AddChartView(IView view);
      void BindToSelecteOutput();
      void ClearBinding();
      bool OutputSelectionEnabled { get; set; }
   }
}