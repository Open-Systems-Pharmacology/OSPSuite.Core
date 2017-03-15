using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationParameterSelectionView: IView<IParameterIdentificationParameterSelectionPresenter>
   {
      void AddSimulationParametersView(IView view);
      void AddIdentificationParametersView(IView view);
      void AddLinkedParametersView(IView view);
      string LinkedParametersCaption { get; set; }
   }
}