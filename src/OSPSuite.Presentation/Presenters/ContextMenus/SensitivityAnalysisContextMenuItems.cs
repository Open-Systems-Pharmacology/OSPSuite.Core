using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using MenuBarItemExtensions = OSPSuite.Presentation.MenuAndBars.MenuBarItemExtensions;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class SensitivityAnalysisContextMenuItems
   {
      public static IMenuBarItem CreateSensitivityAnalysis()
      {
         return MenuBarItemExtensions.WithDescription(CreateMenuButton.WithCaption(MenuNames.AddSensitivityAnalysis), MenuDescriptions.CreateSensitivityAnalysis)
            .WithIcon(ApplicationIcons.SensitivityAnalysis)
            .WithCommand<CreateSensitivityAnalysisUICommand>();
      }

      public static IMenuBarItem CreateSensitivityAnalysisFor(ISimulation simulation)
      {
         return CreateMenuButton.WithCaption(MenuNames.StartSensitivityanalysis)
            .WithIcon(ApplicationIcons.SensitivityAnalysis)
            .WithCommandFor<CreateSensisitivityAnalysisBasedOnSimulationUICommand, ISimulation>(simulation);
      }

      public static IEnumerable<IMenuBarItem> ContextMenuItemsFor(SensitivityAnalysis sensitivityAnalysis)
      {
         yield return Edit(sensitivityAnalysis);
         yield return Rename(sensitivityAnalysis);
         yield return Clone(sensitivityAnalysis).AsGroupStarter();
         yield return AddToJournal(sensitivityAnalysis);
         yield return Delete(sensitivityAnalysis).AsGroupStarter();
      }

      public static IMenuBarItem Clone(SensitivityAnalysis sensitivityAnalysis)
      {
         return CreateMenuButton.WithCaption(MenuNames.Clone)
            .WithIcon(ApplicationIcons.Clone)
            .WithCommandFor<CloneSensitivityAnalysisCommand, SensitivityAnalysis>(sensitivityAnalysis);
      }

      public static IMenuBarItem Delete(SensitivityAnalysis sensitivityAnalysis)
      {
         return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<DeleteSensitivityAnalysisUICommand, SensitivityAnalysis>(sensitivityAnalysis);
      }

      public static IMenuBarItem AddToJournal(SensitivityAnalysis sensitivityAnalysis)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.AddToJournal)
            .WithCommandFor<AddParameterAnalysableToActiveJournalPageUICommand, IParameterAnalysable>(sensitivityAnalysis)
            .WithIcon(ApplicationIcons.AddToJournal);
      }

      public static IMenuBarItem Rename(SensitivityAnalysis sensitivityAnalysis)
      {
         return CreateMenuButton
            .WithCaption(MenuNames.Rename)
            .WithIcon(ApplicationIcons.Rename)
            .WithCommandFor<RenameSensitivityAnalysisUICommand, SensitivityAnalysis>(sensitivityAnalysis);
      }

      public static IMenuBarItem Edit(SensitivityAnalysis sensitivityAnalysis)
      {
         return CreateMenuButton
            .WithCaption(MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditSensitivityAnalysisUICommand, SensitivityAnalysis>(sensitivityAnalysis);
      }
   }
}