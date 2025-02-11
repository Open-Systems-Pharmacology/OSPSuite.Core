using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class SensitivityAnalysisContextMenuItems
   {
      public static IMenuBarItem CreateSensitivityAnalysis(IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.AddSensitivityAnalysis).WithDescription(MenuDescriptions.CreateSensitivityAnalysis)
            .WithIcon(ApplicationIcons.SensitivityAnalysis)
            .WithCommand<CreateSensitivityAnalysisUICommand>(container);
      }

      public static IMenuBarItem CreateSensitivityAnalysisFor(ISimulation simulation, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.StartSensitivityAnalysis)
            .WithIcon(ApplicationIcons.SensitivityAnalysis)
            .WithCommandFor<CreateSensisitivityAnalysisBasedOnSimulationUICommand, ISimulation>(simulation, container);
      }

      public static IEnumerable<IMenuBarItem> ContextMenuItemsFor(SensitivityAnalysis sensitivityAnalysis, IContainer container)
      {
         yield return Edit(sensitivityAnalysis, container);
         yield return Rename(sensitivityAnalysis, container);
         yield return Clone(sensitivityAnalysis, container).AsGroupStarter();
         yield return AddToJournal(sensitivityAnalysis, container);
         yield return Delete(sensitivityAnalysis, container).AsGroupStarter();
      }

      public static IMenuBarItem Clone(SensitivityAnalysis sensitivityAnalysis, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Clone)
            .WithIcon(ApplicationIcons.Clone)
            .WithCommandFor<CloneSensitivityAnalysisCommand, SensitivityAnalysis>(sensitivityAnalysis, container);
      }

      public static IMenuBarItem Delete(SensitivityAnalysis sensitivityAnalysis, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithIcon(ApplicationIcons.Delete)
            .WithCommandFor<DeleteSensitivityAnalysisUICommand, SensitivityAnalysis>(sensitivityAnalysis, container);
      }

      public static IMenuBarItem AddToJournal(SensitivityAnalysis sensitivityAnalysis, IContainer container)
      {
         return CreateMenuButton.WithCaption(Captions.Journal.AddToJournal)
            .WithCommandFor<AddParameterAnalysableToActiveJournalPageUICommand, IParameterAnalysable>(sensitivityAnalysis, container)
            .WithIcon(ApplicationIcons.AddToJournal);
      }

      public static IMenuBarItem Rename(SensitivityAnalysis sensitivityAnalysis, IContainer container)
      {
         return CreateMenuButton
            .WithCaption(MenuNames.Rename)
            .WithIcon(ApplicationIcons.Rename)
            .WithCommandFor<RenameSensitivityAnalysisUICommand, SensitivityAnalysis>(sensitivityAnalysis, container);
      }

      public static IMenuBarItem Edit(SensitivityAnalysis sensitivityAnalysis, IContainer container)
      {
         return CreateMenuButton
            .WithCaption(MenuNames.Edit)
            .WithIcon(ApplicationIcons.Edit)
            .WithCommandFor<EditSensitivityAnalysisUICommand, SensitivityAnalysis>(sensitivityAnalysis, container);
      }
   }
}