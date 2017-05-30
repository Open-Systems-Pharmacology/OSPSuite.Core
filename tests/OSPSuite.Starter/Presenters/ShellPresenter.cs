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

      public ShellPresenter(IShellView view, IEventPublisher eventPublisher, ITabbedMdiChildViewContextMenuFactory contextMenuFactory, IMenuAndToolBarPresenter menuAndToolBarPresenter) : base(view,eventPublisher, contextMenuFactory)
      {
         _menuAndToolBarPresenter = menuAndToolBarPresenter;
      }

      public void Start()
      {
         _menuAndToolBarPresenter.Initialize();
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