using System.Data;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IPivotGridTestView : IView<IPivotGridTestPresenter>
   {
      void BindTo(DataTable dtos);
   }
}