using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class SensitivityAnalysisMenuBarButtons
   {
      public static IMenuBarItem CreateSensitivityAnalysis(MenuBarItemId menuBarItemId)
      {
         return SensitivityAnalysisContextMenuItems.CreateSensitivityAnalysis()
            .WithCaption(MenuNames.CreateSensitivityAnalysis)
            .WithId(menuBarItemId)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.Y);
      }

      public static IMenuBarButton SensitivityAnalysisFeedbackView(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.FeedbackView)
            .WithId(menuBarItemId)
            .WithCommand<SensitivityAnalysisFeedbackViewVisibilityUICommand>()
            .WithDescription(MenuDescriptions.SensitivityAnalysisFeedbackViewDescription)
            .WithIcon(ApplicationIcons.SensitivityAnalysisVisualFeedback);
      }

      public static IMenuBarButton RunSensitivityAnalysis(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.RunSensitivityAnalysis)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.RunSensitivityAnalysis)
            .WithIcon(ApplicationIcons.Run)
            .WithShortcut(Keys.F7)
            .WithCommand<RunSensitivityAnalysisUICommand>();
      }

      public static IMenuBarItem StopSensitivityAnalysis(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.StopSensitivityanalysis)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.StopSensitivityanalysis)
            .WithIcon(ApplicationIcons.Stop)
            .WithCommand<StopSensitivityAnalysisUICommand>()
            .WithShortcut(Keys.Shift | Keys.F7);

      }

      public static IMenuBarItem SensitivityAnalysisPKParameterAnalysis(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.SensitivityAnalysis.SensitivityAnalysisPKParameterAnalysis)
            .WithId(menuBarItemId)
            .WithDescription(Captions.SensitivityAnalysis.SensitivityAnalysisPKParameterAnalysisDescription)
            .WithIcon(ApplicationIcons.PKParameterSensitivityAnalysis)
            .WithCommand<StartSensitivityAnalysisUICommand>();
      }
   }
}