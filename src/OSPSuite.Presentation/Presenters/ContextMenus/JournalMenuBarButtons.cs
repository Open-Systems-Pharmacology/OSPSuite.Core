using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class JournalMenuBarButtons
   {
      public static IMenuBarItem JournalView(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.JournalView)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.JournalViewDescription)
            .WithCommand<JournalVisibilityCommand>()
            .WithIcon(ApplicationIcons.Journal)
            .WithShortcut(Keys.Control | Keys.Shift | Keys.J);
      }

      public static IMenuBarItem CreateJournalPage(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.CreateJournalPage)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.ToolTip.CreateJournalPage)
            .WithIcon(ApplicationIcons.PageAdd)
            .WithCommand<CreateJournalPageUICommand>()
            .WithShortcut(Keys.Control | Keys.Alt | Keys.J);

      }

      public static IMenuBarItem SelectJournal(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.SelectJournal)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.SelectJournalDescription)
            .WithIcon(ApplicationIcons.JournalSelect)
            .WithCommand<SelectJournalUICommand>();
      }

      public static IMenuBarItem JournalEditorView(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.JournalEditorView)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.JournalEditorViewDescription)
            .WithCommand<JournalEditorVisibiliyUICommand>()
            .WithIcon(ApplicationIcons.PageEdit);
      }

      public static IMenuBarItem SearchJournal(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.SearchJournal)
            .WithId(menuBarItemId)
            .WithDescription(Captions.Journal.SearchJournalDescription)
            .WithCommand<SearchJournalUICommand>()
            .WithIcon(ApplicationIcons.Search);
      }

      public static IMenuBarButton ExportJournal(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.ExportJournal)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.ExportJournalToFile)
            .WithCommand<ExportJournalToFileUICommand>()
            .WithIcon(ApplicationIcons.JournalExportToWord);
      }

      public static IMenuBarButton RefreshJournal(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.RefreshJournal)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.RefreshJournal)
            .WithCommand<RefreshJournalUICommand>()
            .WithIcon(ApplicationIcons.Refresh);
      }
   }
}