using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IJournalRichEditFormView : IModalView<IJournalRichEditFormPresenter>
   {
      void SetPageContent(byte[] data);
   }
}