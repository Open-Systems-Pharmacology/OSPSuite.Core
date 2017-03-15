using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IJournalTestView : IView<IJournalTestPresenter>
   {
      void AddDiagram(IView view);
   }
}