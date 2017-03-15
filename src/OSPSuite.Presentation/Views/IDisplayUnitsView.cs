using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IDisplayUnitsView : IView<IDisplayUnitsPresenter>
   {
      void BindTo(IEnumerable<DefaultUnitMapDTO> defaultUnits);
   }
}