using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class CommonMenuBarButtons
   {
      public static IMenuBarButton SaveFavoritesToFile(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.SaveFavoriteToFile)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.SaveFavoritesToFile)
            .WithCommand<SaveFavoritesToFileUICommand>(container)
            .WithIcon(ApplicationIcons.FavoritesSave);
      }

      public static IMenuBarButton LoadFavoritesFromFile(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.LoadFavoritesFromFile)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.LoadFavoritesFromFile)
            .WithCommand<LoadFavoritesFromFileUICommand>(container)
            .WithIcon(ApplicationIcons.FavoritesLoad);
      }

      public static IMenuBarButton ManageProjectDisplayUnits(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.ManageProjectDisplayUnits)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.ManageProjectDisplayUnits)
            .WithIcon(ApplicationIcons.ProjectDisplayUnitsConfigure)
            .WithCommand<ManageProjectDisplayUnitsUICommand>(container);
      }

      public static IMenuBarButton ManageUserDisplayUnits(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.ManageUserDisplayUnits)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.ManageUserDisplayUnits)
            .WithIcon(ApplicationIcons.UserDisplayUnitsConfigure)
            .WithCommand<ManageUserDisplayUnitsUICommand>(container);
      }

      public static IMenuBarButton UpdateAllToDisplayUnits(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.UpdateAllToDisplayUnits)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.UpdateAllToDisplayUnits)
            .WithIcon(ApplicationIcons.Refresh)
            .WithCommand<UpdateAllDisplayUnitsUICommand>(container);
      }

      public static IMenuBarItem JournalDiagramView(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.JournalDiagramView)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.JournalDiagramDescription)
            .WithCommand<JournalDiagramVisibilityUICommand>(container)
            .WithIcon(ApplicationIcons.JournalDiagram);
      }

      public static IMenuBarItem ClearHistory(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.ClearHistory)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.ClearHistory)
            .WithCommand<ClearHistoryUICommand>(container)
            .WithIcon(ApplicationIcons.ClearHistory);
      }

      public static IMenuBarItem Help(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Help)
            .WithId(menuBarItemId)
            .WithIcon(ApplicationIcons.Help)
            .WithCommand<ShowHelpCommand>(container)
            .WithShortcut(Keys.F1);
      }
   }
}