using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationConfigurationView : IView<IParameterIdentificationConfigurationPresenter>
   {
      void BindTo(ParameterIdentificationConfigurationDTO parameterIdentificationConfiguration);
      void AddAlgorithmOptionsView(IExtendedPropertiesView view);
      void UpdateOptimizationsView(IView view);
   }
}