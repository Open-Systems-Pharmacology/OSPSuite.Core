using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface ILogView : IView<ILogPresenter>
   {
      void BindTo(MessageStatusFilterDTO statusFilter);
      void AddLog(string log);
      void ClearLog();

   }

}