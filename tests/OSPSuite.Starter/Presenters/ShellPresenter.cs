using OSPSuite.Core;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Starter.Views;
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
      private readonly IWatermarkStatusChecker _watermarkStatusChecker;
      private readonly IApplicationSettings _applicationSettings;

      public ShellPresenter(IShellView view, IEventPublisher eventPublisher, ITabbedMdiChildViewContextMenuFactory contextMenuFactory,
         IMenuAndToolBarPresenter menuAndToolBarPresenter,
         IWatermarkStatusChecker watermarkStatusChecker, IApplicationSettings applicationSettings) : base(view, eventPublisher, contextMenuFactory)
      {
         _menuAndToolBarPresenter = menuAndToolBarPresenter;
         _watermarkStatusChecker = watermarkStatusChecker;
         _applicationSettings = applicationSettings;
      }

      public void Start()
      {
         _menuAndToolBarPresenter.Initialize();
         _applicationSettings.UseWatermark = null;
         _watermarkStatusChecker.CheckWatermarkStatus();
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
   }
}