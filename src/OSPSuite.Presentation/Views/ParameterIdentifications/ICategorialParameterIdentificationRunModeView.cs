using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface ICategorialParameterIdentificationRunModeView : IView<ICategorialParameterIdentificationRunModePresenter>
   {
      void BindTo(IEnumerable<CategoryDTO> categories);
      void BindTo(CategorialRunModeDTO categorialRunModeDTO);
      void UpdateParameterIdentificationCount(int parameterIdentificationCount);
   }
}