using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class SensitivityAnalysisMenuBarButtons
   {
      public static IMenuBarItem CreateSensitivityAnalysis(MenuBarItemId menuBarItemId, IContainer container)
      {
         return SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysis(container)
            .WithCaption(MenuNames.CreateSensitivityAnalysis)
            .WithId(menuBarItemId)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Y);
      }

      public static IMenuBarButton SensitivityAnalysisFeedbackView(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.FeedbackView)
            .WithId(menuBarItemId)
            .WithCommand<SensitivityAnalysisFeedbackViewVisibilityUICommand>(container)
            .WithDescription(MenuDescriptions.SensitivityAnalysisFeedbackViewDescription)
            .WithIcon(ApplicationIcons.SensitivityAnalysisVisualFeedback);
      }

      public static IMenuBarButton RunSensitivityAnalysis(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.RunSensitivityAnalysis)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.RunSensitivityAnalysis)
            .WithIcon(ApplicationIcons.Run)
            .WithShortcut(Keys.F7)
            .WithCommand<RunSensitivityAnalysisUICommand>(container);
      }

      public static IMenuBarItem StopSensitivityAnalysis(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(MenuNames.StopSensitivityAnalysis)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.StopSensitivityanalysis)
            .WithIcon(ApplicationIcons.Stop)
            .WithCommand<StopSensitivityAnalysisUICommand>(container)
            .WithShortcut(Keys.Shift | Keys.F7);
      }

      public static IMenuBarItem SensitivityAnalysisPKParameterAnalysis(MenuBarItemId menuBarItemId, IContainer container)
      {
         return CreateMenuButton.WithCaption(Captions.SensitivityAnalysis.SensitivityAnalysisPKParameterAnalysis)
            .WithId(menuBarItemId)
            .WithDescription(Captions.SensitivityAnalysis.SensitivityAnalysisPKParameterAnalysisDescription)
            .WithIcon(ApplicationIcons.PKParameterSensitivityAnalysis)
            .WithCommand<StartSensitivityAnalysisUICommand>(container);
      }
   }
}