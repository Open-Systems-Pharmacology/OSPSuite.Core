using System;
using System.Linq;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters.Main
{
   public abstract class AbstractMenuAndToolBarPresenter : IMainViewItemPresenter,
      IListener<ProjectLoadedEvent>,
      IListener<ProjectSavedEvent>
   {
      protected readonly IMenuAndToolBarView _view;
      private readonly IMenuBarItemRepository _menuBarItemRepository;
      private readonly IMRUProvider _mruProvider;
      public event EventHandler StatusChanged = delegate { };

      protected AbstractMenuAndToolBarPresenter(IMenuAndToolBarView view, IMenuBarItemRepository menuBarItemRepository, IMRUProvider mruProvider)
      {
         _view = view;
         _menuBarItemRepository = menuBarItemRepository;
         _mruProvider = mruProvider;
      }

      public virtual void Initialize()
      {
         AddRibbonPages();
         updateMruMenuList();
         DisableMenuBarItemsForPogramStart();
      }

      protected abstract void DisableMenuBarItemsForPogramStart();

      protected abstract void AddRibbonPages();

      private void updateMruMenuList()
      {
         var allProjectMenuItems = _mruProvider.All().Select(path => _menuBarItemRepository.DynamicFileMenuFor(path));
         _view.AddMRUMenus(allProjectMenuItems);
      }

      public virtual void Handle(ProjectSavedEvent eventToHandle)
      {
         updateMruMenuList();
      }

      public virtual void Handle(ProjectLoadedEvent eventToHandle)
      {
         updateMruMenuList();
      }

      public virtual void ReleaseFrom(IEventPublisher eventPublisher)
      {
         eventPublisher.RemoveListener(this);
      }

      protected void DisableAll()
      {
         _menuBarItemRepository.All().Each(item => item.Enabled = false);
      }

      public void ToggleVisibility()
      {
         //nothing to do so far
      }

      public void ViewChanged()
      {
         //nothing to do here
      }

      public IView BaseView
      {
         get { return null; }
      }

      public bool CanClose
      {
         get { return true; }
      }

      public string ErrorMessage
      {
         get { return string.Empty; }
      }
   }
}