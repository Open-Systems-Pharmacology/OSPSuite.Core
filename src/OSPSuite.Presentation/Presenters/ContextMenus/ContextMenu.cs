using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ContextMenus;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;

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

      protected ContextMenu(IContainer container)
         : this(container.Resolve<IContextMenuView>())
      {
      }

      protected ContextMenu(IContextMenuView view)
      {
         _view = view;
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
         AllMenuItemsFor(objectRequestingContextMenu).Where(x => x != null).Each(_view.AddMenuItem);
      }

      protected abstract IEnumerable<IMenuBarItem> AllMenuItemsFor(T objectRequestingContextMenu);
   }

   public abstract class ContextMenu<TObject, TContext> : ContextMenu
   {
      protected readonly IContainer _container;

      protected ContextMenu(TObject objectRequestingContextMenu, TContext context, IContainer container) : base(container)
      {
         _container = container;
         AllMenuItemsFor(objectRequestingContextMenu, context).Where(x => x != null).Each(_view.AddMenuItem);
      }

      protected abstract IEnumerable<IMenuBarItem> AllMenuItemsFor(TObject objectRequestingContextMenu, TContext context);
   }
}