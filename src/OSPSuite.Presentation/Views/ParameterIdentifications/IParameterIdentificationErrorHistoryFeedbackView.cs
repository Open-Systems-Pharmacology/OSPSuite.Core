using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
    public interface IParameterIdentificationErrorHistoryFeedbackView : IView<IParameterIdentificationErrorHistoryFeedbackPresenter>
    {
        void AddChartView(IView view);

    }
}