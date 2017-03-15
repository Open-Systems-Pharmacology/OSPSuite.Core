using OSPSuite.Core.Reporting;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IReportingView : IModalView<IReportingPresenter>
   {
      void BindTo(ReportConfiguration reportConfiguration);
      bool IsDeveloperMode { set; }
   }
}