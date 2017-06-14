using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IMenuAndToolBarPresenter : IMainViewItemPresenter
   {
   }

   public class MenuAndToolBarPresenter : AbstractMenuAndToolBarPresenter, IMenuAndToolBarPresenter
   {
      public MenuAndToolBarPresenter(IMenuAndToolBarView view, IMenuBarItemRepository menuBarItemRepository, IMRUProvider mruProvider) : base(view, menuBarItemRepository, mruProvider)
      {
      }

      protected override void DisableMenuBarItemsForPogramStart()
      {
      }

      protected override void AddRibbonPages()
      {
         _view.AddPageGroupToPage(createGroupModelling(), "Modeling");
         _view.AddPageGroupToPage(createParameterIdentification(), "Modeling");
         _view.AddPageGroupToPage(createParameterIdentificationAnalyses(), "Modeling");
         _view.AddPageGroupToPage(createParameterIdentificationConfidenceIntervals(), "Modeling");

//         _view.AddPageGroupToPage(createSensitivityAnalysis(), "Modeling");
//         _view.AddPageGroupToPage(createSensitivityAnalysisAnalyses(), "Modeling");
      }

      private IButtonGroup createSensitivityAnalysisAnalyses()
      {
         return CreateButtonGroup.WithCaption("Analyses")
            .WithButton(CreateRibbonButton.From(createSensitivityAnalysisAnalysesButton()))
            .WithId("SAA");
      }

      private IMenuBarItem createSensitivityAnalysisAnalysesButton()
      {
         return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisPKParameterAnalysis(new MenuBarItemId("Sensitivity Analysis", 13));
      }

      private IButtonGroup createSensitivityAnalysis()
      {
         return CreateButtonGroup.WithCaption("Sensitivity Analysis")
            .WithButton(CreateRibbonButton.From(createSensitivityAnalysisRunButton()))
            .WithButton(CreateRibbonButton.From(createSensitivityAnalysisShowVisualFeedbackButton()))
            .WithId("SA");
      }

      private IMenuBarItem createSensitivityAnalysisShowVisualFeedbackButton()
      {
         return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisFeedbackView(new MenuBarItemId("Show Visual Feedback SA", 12));
      }

      private IMenuBarItem createSensitivityAnalysisRunButton()
      {
         return SensitivityAnalysisMenuBarButtons.RunSensitivityAnalysis(new MenuBarItemId("Run SA", 11));
      }

      private IButtonGroup createParameterIdentificationConfidenceIntervals()
      {
         return CreateButtonGroup.WithCaption("Confidence Intervals")
            .WithButton(CreateRibbonButton.From(createConfidenceIntervalButton()))
            .WithButton(CreateRibbonButton.From(createVisualPredictiveCheckButton()))
            .WithButton(CreateRibbonButton.From(createPredictionIntervalButton()))
            .WithId("Confidence");
      }

      private IButtonGroup createParameterIdentificationAnalyses()
      {
         return CreateButtonGroup.WithCaption("Analyses")
            .WithButton(CreateRibbonButton.From(createTimeProfileButton()))
            .WithButton(CreateRibbonButton.From(createPredictedVsObservedButton()))
            .WithButton(CreateRibbonButton.From(createResidualsVsTimeButton()))
            .WithButton(CreateRibbonButton.From(createHistogramOfResidualsButton()))
            .WithButton(CreateRibbonButton.From(createCovarianceMatrixButton()))
            .WithButton(CreateRibbonButton.From(createCorrelationMatrixButton()))
            .WithId("Analyses");
      }

      private IMenuBarItem createPredictionIntervalButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfilePredictionInterval(new MenuBarItemId("Prediction Interval", 9));
      }

      private IMenuBarItem createVisualPredictiveCheckButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileVPCInterval(new MenuBarItemId("Visual Predictive Check", 8));
      }

      private IMenuBarItem createConfidenceIntervalButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileConfidenceInterval(new MenuBarItemId("Confidence Interval", 7));
      }

      private IMenuBarItem createCorrelationMatrixButton()
      {
         return ParameterIdentificationMenuBarButtons.CorrelationMatrixParameterIdentification(new MenuBarItemId("Correlation Matrix", 6));
      }

      private IMenuBarItem createCovarianceMatrixButton()
      {
         return ParameterIdentificationMenuBarButtons.CovarianceMatrixParameterIdentification(new MenuBarItemId("Covariance Matrix", 5));
      }

      private IMenuBarItem createHistogramOfResidualsButton()
      {
         return ParameterIdentificationMenuBarButtons.ResidualHistogramParameterIdentification(new MenuBarItemId("Histogram of Residuals", 4));
      }

      private IMenuBarItem createResidualsVsTimeButton()
      {
         return ParameterIdentificationMenuBarButtons.ResidualsVsTimeParameterIdentifcation(new MenuBarItemId("Residual Vs. Time", 3));
      }

      private IMenuBarItem createPredictedVsObservedButton()
      {
         return ParameterIdentificationMenuBarButtons.PredictedVsObservedParameterIdentification(new MenuBarItemId("Predicted Vs. Observed", 2));
      }

      private IButtonGroup createParameterIdentification()
      {
         return CreateButtonGroup.WithCaption("Parameter Identification")
            .WithButton(CreateRibbonButton.From(createParameterIdentificationRunButton()))
            .WithButton(CreateRibbonButton.From(createParameterIdentificationShowVisualFeedbackButton()))
            .WithId("TEST");
      }

      private IMenuBarItem createParameterIdentificationShowVisualFeedbackButton()
      {
         return ParameterIdentificationMenuBarButtons.ParameterIdentificationFeedbackView(new MenuBarItemId("Show Visual Feedback", 10));
      }

      private static IMenuBarItem createTimeProfileButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileParameterIdentification(new MenuBarItemId("Time Profile", 1));
      }

      private static IMenuBarItem createParameterIdentificationRunButton()
      {
         return ParameterIdentificationMenuBarButtons.RunParameterIdentification(new MenuBarItemId("Run", 0));
      }

      private IButtonGroup createGroupModelling()
      {
         return CreateButtonGroup.WithCaption("TEST")
            .WithButton(createButton(ApplicationIcons.PopulationSimulationComparison, "Current"))
            .WithId("TEST");
      }

      private IRibbonBarItem createButton(ApplicationIcon icon, string caption)
      {
         return CreateRibbonButton.From(createMenuButton(icon, caption));
      }

      private IMenuBarButton createMenuButton(ApplicationIcon icon, string caption)
      {
         return CreateMenuButton.WithCaption(caption)
            .WithIcon(icon);
      }
   }
}