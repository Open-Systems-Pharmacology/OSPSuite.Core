using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IEditOutputSelectionsView : IView<IEditOutputSelectionsPresenter>
   {
      void BindTo(IEnumerable<QuantitySelection> allOutputs);
   }
}