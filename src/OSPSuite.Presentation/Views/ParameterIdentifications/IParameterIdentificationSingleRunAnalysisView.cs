using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationSingleRunAnalysisView : IView<IParameterIdentificationSingleRunAnalysisPresenter>, IParameterIdentificationAnalysisView
   {
      bool CanSelectRunResults { set; }
      void BindToSelectedRunResult();
   }
}