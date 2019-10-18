using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class CommonMenuBarButtons
   {
      public static IMenuBarButton SaveFavoritesToFile(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.SaveFavoriteToFile)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.SaveFavoritesToFile)
            .WithCommand<SaveFavoritesToFileUICommand>()
            .WithIcon(ApplicationIcons.FavoritesSave);
      }

      public static IMenuBarButton LoadFavoritesFromFile(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.LoadFavoritesFromFile)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.LoadFavoritesFromFile)
            .WithCommand<LoadFavoritesFromFileUICommand>()
            .WithIcon(ApplicationIcons.FavoritesLoad);
      }

      public static IMenuBarButton ManageProjectDisplayUnits(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.ManageProjectDisplayUnits)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.ManageProjectDisplayUnits)
            .WithIcon(ApplicationIcons.ProjectDisplayUnitsConfigure)
            .WithCommand<ManageProjectDisplayUnitsUICommand>();
      }

      public static IMenuBarButton ManageUserDisplayUnits(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.ManageUserDisplayUnits)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.ManageUserDisplayUnits)
            .WithIcon(ApplicationIcons.UserDisplayUnitsConfigure)
            .WithCommand<ManageUserDisplayUnitsUICommand>();
      }

      public static IMenuBarButton UpdateAllToDisplayUnits(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.UpdateAllToDisplaytUnits)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.UpdateAllToDisplayUnits)
            .WithIcon(ApplicationIcons.Refresh)
            .WithCommand<UpdateAllDisplayUnitsUICommand>();
      }

      public static IMenuBarItem JournalDiagramView(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.JournalDiagramView)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.JournalDiagramDescription)
            .WithCommand<JournalDiagramVisibilityUICommand>()
            .WithIcon(ApplicationIcons.JournalDiagram);
      }

      public static IMenuBarItem ClearHistory(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.ClearHistory)
            .WithId(menuBarItemId)
            .WithDescription(ToolTips.ClearHistory)
            .WithCommand<ClearHistoryUICommand>()
            .WithIcon(ApplicationIcons.ClearHistory);
      }

      public static IMenuBarItem Help(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.Help)
            .WithId(menuBarItemId)
            .WithIcon(ApplicationIcons.Help)
            .WithCommand<ShowHelpCommand>()
            .WithShortcut(Keys.F1);
      }
   }
}