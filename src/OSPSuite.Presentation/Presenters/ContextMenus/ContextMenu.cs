using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ContextMenus;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface IContextMenu
   {
      void ActivateFirstMenu();
      void Show(IView parentView, Point popupLocation);
   }

   public class EmptyContextMenu : IContextMenu
   {
      public void ActivateFirstMenu()
      {
         //nothing to do
      }

      public void Show(IView parentView, Point popupLocation)
      {
         //nothing to do
      }
   }

   public abstract class ContextMenu : IContextMenu
   {
      protected readonly IContextMenuView _view;
      private readonly IContainer _container;

      protected ContextMenu(IContainer container)
         : this(container.Resolve<IContextMenuView>(), container)
      {
      }

      protected ContextMenu(IContextMenuView view, IContainer container)
      {
         _view = view;
         _container = container;
      }

      public void ActivateFirstMenu()
      {
         _view.ActivateMenu(_view.AllMenuItems.FirstOrDefault());
      }

      public virtual void Show(IView parentView, Point popupLocation)
      {
         _view.Display(parentView, popupLocation);
      }
   }

   public abstract class ContextMenu<T> : ContextMenu
   {
      protected readonly IContainer _container;

      protected ContextMenu(T objectRequestingContextMenu, IContainer container) : base(container)
      {
         _container = container;
         AllMenuItemsFor(objectRequestingContextMenu).Each(_view.AddMenuItem);
      }

      protected abstract IEnumerable<IMenuBarItem> AllMenuItemsFor(T objectRequestingContextMenu);
   }

   public abstract class ContextMenu<TObject, TContext> : ContextMenu
   {
      protected readonly IContainer _container;
      protected ContextMenu(TObject objectRequestingContextMenu, TContext context, IContainer container) : base(container)
      {
         _container = container;
         AllMenuItemsFor(objectRequestingContextMenu, context).Each(_view.AddMenuItem);
      }

      protected abstract IEnumerable<IMenuBarItem> AllMenuItemsFor(TObject objectRequestingContextMenu, TContext context);
   }
}