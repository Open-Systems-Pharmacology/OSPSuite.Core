using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationAnalysisView : IView
   {
      void SetAnalysisView(IView view);
   }

   public interface IParameterIdentificationMultipleRunsAnalysisView : IView<IParameterIdentificationAnalysisPresenter>,  IParameterIdentificationAnalysisView
   {
   }
}