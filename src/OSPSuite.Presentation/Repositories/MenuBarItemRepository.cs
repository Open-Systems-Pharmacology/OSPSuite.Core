using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Repositories
{
   public interface IMenuBarItemRepository : IRepository<IMenuBarItem>
   {
      IMenuBarItem Find(MenuBarItemId menuBarItemId);
      IMenuBarItem this[MenuBarItemId menuBarItemId] { get; }
      IMenuBarItem DynamicFileMenuFor(string path);
   }

   public abstract class MenuBarItemRepository : IMenuBarItemRepository
   {
      private readonly IContainer _container;
      private readonly ICache<string, IMenuBarItem> _menuBarItemList;

      protected MenuBarItemRepository(IContainer container)
      {
         _container = container;
         _menuBarItemList = new Cache<string, IMenuBarItem>(menuItem => menuItem.Name);
         AllMenuBarItems().Each(_menuBarItemList.Add);
      }

      protected abstract IEnumerable<IMenuBarItem> AllMenuBarItems();

      public IEnumerable<IMenuBarItem> All()
      {
         return _menuBarItemList.All();
      }

      public IMenuBarItem Find(MenuBarItemId menuBarItemId)
      {
         return _menuBarItemList[menuBarItemId.Name] ?? CreateMenuButton.WithCaption("Empty Menu Bar Item");
      }

      public IMenuBarItem this[MenuBarItemId menuBarItemId] => Find(menuBarItemId);

      public IMenuBarItem DynamicFileMenuFor(string path)
      {
         var command = _container.Resolve<OpenMRUProjectCommand>();
         command.ProjectPath = path;

         return CreateMenuButton
            .WithCaption(FileHelper.FileNameFromFileFullPath(path))
            .WithDescription(path)
            .WithCommand(command);
      }
   }
}