using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IMultipleParameterIdentificationRunModeView : IView<IMultipleParameterIdentificationRunModePresenter>
   {
      void BindTo(MultipleParameterIdentificationRunMode multipleOptimization);
   }
}