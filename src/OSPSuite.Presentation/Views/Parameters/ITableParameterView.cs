using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.Parameters;

namespace OSPSuite.Presentation.Views.Parameters
{
   public interface ITableParameterView : IView<ITableParameterPresenter>
   {
      void Clear();
      void BindTo(IEnumerable<ValuePointDTO> allPoints);
      void EditPoint(ValuePointDTO pointToEdit);
      bool ImportVisible { set; }
      string YCaption { set; }
      string XCaption { set; }
      bool Editable { get; set; }
      string Description { get; set; }
      string ImportToolTip { get; set; }
   }
}