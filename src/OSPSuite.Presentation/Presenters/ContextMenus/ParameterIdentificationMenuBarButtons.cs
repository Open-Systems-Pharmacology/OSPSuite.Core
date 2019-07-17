using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public static class ParameterIdentificationMenuBarButtons
   {
      public static IMenuBarItem CreateParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return ParameterIdentificationContextMenuItems.CreateParameterIdentification()
            .WithCaption(MenuNames.CreateParameterIdentification)
            .WithId(menuBarItemId)
            .WithShortcut(Keys.Control | Keys.Alt | Keys.O);
      }

      public static IMenuBarItem RunParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.RunParameterIdentification)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.RunParameterIdentification)
            .WithIcon(ApplicationIcons.Run)
            .WithCommand<RunParameterIdentificationUICommand>()
            .WithShortcut(Keys.F6);

      }

      public static IMenuBarItem StopParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.StopParameterIdentification)
            .WithId(menuBarItemId)
            .WithDescription(MenuDescriptions.StopParameterIdentification)
            .WithIcon(ApplicationIcons.Stop)
            .WithCommand<StopParameterIdentificationUICommand>()
            .WithShortcut(Keys.Shift | Keys.F6);

      }

      public static IMenuBarItem TimeProfileParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.TimeProfileAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartTimeProfileParameterIdentificationAnalysisUICommand>()
            .WithDescription(MenuDescriptions.TimeProfileAnalysisDescription)
            .WithIcon(ApplicationIcons.TimeProfileAnalysis);
      }

      public static IMenuBarItem PredictedVsObservedParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.PredictedVsObservedAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartPredictedVsObservedParameterIdentificationAnalysisUICommand>()
            .WithDescription(MenuDescriptions.PredictedVsObservedAnalysisDescription)
            .WithIcon(ApplicationIcons.PredictedVsObservedAnalysis);
      }

      public static IMenuBarItem ResidualsVsTimeParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.ResidualsVsTimeAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartResidualsVsTimeParameterIdentificationAnalysisUICommand>()
            .WithDescription(MenuDescriptions.ResidualsVsTimeAnalysisDescription)
            .WithIcon(ApplicationIcons.ResidualVsTimeAnalysis);
      }

      public static IMenuBarItem ResidualHistogramParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.ResidualHistogramAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartResidualHistogramParameterIdentificationAnalysisUICommand>()
            .WithDescription(MenuDescriptions.ResidualHistogramAnalysisDescription)
            .WithIcon(ApplicationIcons.ResidualHistogramAnalysis);
      }

      public static IMenuBarItem ParameterIdentificationFeedbackView(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(MenuNames.FeedbackView)
            .WithId(menuBarItemId)
            .WithCommand<ParameterIdentificationFeedbackViewVisibilityUICommand>()
            .WithDescription(MenuDescriptions.ParameterIdentificationFeedbackViewDescription)
            .WithIcon(ApplicationIcons.ParameterIdentificationVisualFeedback);
      }

      public static IMenuBarItem CovarianceMatrixParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.CovarianceMatrix)
            .WithId(menuBarItemId)
            .WithCommand<StartCovarianceMatrixAnalysisUICommand>()
            .WithDescription(MenuDescriptions.CovarianceMatrix)
            .WithIcon(ApplicationIcons.CovarianceAnalysis);
      }

      public static IMenuBarItem CorrelationMatrixParameterIdentification(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.CorrelationMatrix)
            .WithId(menuBarItemId)
            .WithCommand<StartCorrelationMatrixAnalysisUICommand>()
            .WithDescription(MenuDescriptions.CorrelationMatrix)
            .WithIcon(ApplicationIcons.CorrelationAnalysis);
      }

      public static IMenuBarButton TimeProfilePredictionInterval(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.TimeProfilePredictionIntervalAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartTimeProfilePredictionIntervalAnalysisUICommand>()
            .WithDescription(MenuDescriptions.TimeProfilePredictionInterval)
            .WithIcon(ApplicationIcons.TimeProfilePredictionInterval);
      }

      public static IMenuBarButton TimeProfileConfidenceInterval(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.TimeProfileConfidenceIntervalAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartTimeProfileConfidenceIntervalAnalysisUICommand>()
            .WithDescription(MenuDescriptions.TimeProfileConfidenceInterval)
            .WithIcon(ApplicationIcons.TimeProfileConfidenceInterval);
      }

      public static IMenuBarButton TimeProfileVPCInterval(MenuBarItemId menuBarItemId)
      {
         return CreateMenuButton.WithCaption(Captions.ParameterIdentification.TimeProfileVPCIntervalAnalysis)
            .WithId(menuBarItemId)
            .WithCommand<StartTimeProfileVPCIntervalAnalysisUICommand>()
            .WithDescription(MenuDescriptions.TimeProfileVPCInterval)
            .WithIcon(ApplicationIcons.TimeProfileVPCInterval);
      }
   }
}