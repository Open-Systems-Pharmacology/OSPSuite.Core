using System.Collections.Generic;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface IParameterIdentificationConfidenceIntervalView : IView<IParameterIdentificationConfidenceIntervalPresenter>
   {
      void DeleteBinding();
      void BindTo(IEnumerable<ParameterConfidenceIntervalDTO> parameterConfidenceIntervalDTOs);
   }
}