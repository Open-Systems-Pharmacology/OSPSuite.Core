using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Starter.Views;
using OSPSuite.TeXReporting.Events;
using OSPSuite.Utility.Events;

namespace OSPSuite.Starter.Presenters
{
   public interface IShellPresenter : IPresenter<IShellView>, IMainViewPresenter
   {
      void Start();
   }

   public class ShellPresenter : AbstractMainViewPresenter<IShellView, IShellPresenter>, IShellPresenter
   {
      private readonly IMenuAndToolBarPresenter _menuAndToolBarPresenter;
      private readonly IWatermakStatusChecker _watermakStatusChecker;
      private readonly IApplicationSettings _applicationSettings;

      public ShellPresenter(IShellView view, IEventPublisher eventPublisher, ITabbedMdiChildViewContextMenuFactory contextMenuFactory, 
         IMenuAndToolBarPresenter menuAndToolBarPresenter, 
         IWatermakStatusChecker watermakStatusChecker, IApplicationSettings applicationSettings) : base(view,eventPublisher, contextMenuFactory)
      {
         _menuAndToolBarPresenter = menuAndToolBarPresenter;
         _watermakStatusChecker = watermakStatusChecker;
         _applicationSettings = applicationSettings;
      }

      public void Start()
      {
         _menuAndToolBarPresenter.Initialize();
         _applicationSettings.UseWatermark = null;
         _watermakStatusChecker.CheckWatermarkStatus();
         View.Show();
      }

      public override void Run()
      {
      }

      public override void RemoveAlert()
      {
         
      }

      public override void OpenFile(string fileName)
      {
         
      }

      public override void Handle(ReportCreationStartedEvent eventToHandle)
      {
         
      }

      public override void Handle(ReportCreationFinishedEvent eventToHandle)
      {
         
      }
   }
}