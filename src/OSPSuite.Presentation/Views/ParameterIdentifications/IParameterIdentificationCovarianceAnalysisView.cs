namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationCovarianceAnalysisView : IView
   {
      void SetMatrixView(IView view);
      void SetConfidenceIntevalView(IView view);
   }
}