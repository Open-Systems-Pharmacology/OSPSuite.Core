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
      private int _menuId = 0;
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

         _view.AddPageGroupToPage(createSensitivityAnalysis(), "Modeling");
         _view.AddPageGroupToPage(createSensitivityAnalysisAnalyses(), "Modeling");

         _view.AddPageHeaderItemLinks(CommonMenuBarButtons.Help(new MenuBarItemId("Help", _menuId++)));
      }

      private IButtonGroup createSensitivityAnalysisAnalyses()
      {
         return CreateButtonGroup.WithCaption("Analyses")
            .WithButton(CreateRibbonButton.From(createSensitivityAnalysisAnalysesButton()))
            .WithId("SAA");
      }

      private IMenuBarItem createSensitivityAnalysisAnalysesButton()
      {
         return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisPKParameterAnalysis(new MenuBarItemId("Sensitivity Analysis", _menuId++));
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
         return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisFeedbackView(new MenuBarItemId("Show Visual Feedback SA", _menuId++));
      }

      private IMenuBarItem createSensitivityAnalysisRunButton()
      {
         return SensitivityAnalysisMenuBarButtons.RunSensitivityAnalysis(new MenuBarItemId("Run SA", _menuId++));
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
         return ParameterIdentificationMenuBarButtons.TimeProfilePredictionInterval(new MenuBarItemId("Prediction Interval", _menuId++));
      }

      private IMenuBarItem createVisualPredictiveCheckButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileVPCInterval(new MenuBarItemId("Visual Predictive Check", _menuId++));
      }

      private IMenuBarItem createConfidenceIntervalButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileConfidenceInterval(new MenuBarItemId("Confidence Interval", _menuId++));
      }

      private IMenuBarItem createCorrelationMatrixButton()
      {
         return ParameterIdentificationMenuBarButtons.CorrelationMatrixParameterIdentification(new MenuBarItemId("Correlation Matrix", _menuId++));
      }

      private IMenuBarItem createCovarianceMatrixButton()
      {
         return ParameterIdentificationMenuBarButtons.CovarianceMatrixParameterIdentification(new MenuBarItemId("Covariance Matrix", _menuId++));
      }

      private IMenuBarItem createHistogramOfResidualsButton()
      {
         return ParameterIdentificationMenuBarButtons.ResidualHistogramParameterIdentification(new MenuBarItemId("Histogram of Residuals", _menuId++));
      }

      private IMenuBarItem createResidualsVsTimeButton()
      {
         return ParameterIdentificationMenuBarButtons.ResidualsVsTimeParameterIdentification(new MenuBarItemId("Residual Vs. Time", _menuId++));
      }

      private IMenuBarItem createPredictedVsObservedButton()
      {
         return ParameterIdentificationMenuBarButtons.PredictedVsObservedParameterIdentification(new MenuBarItemId("Predicted Vs. Observed", _menuId++));
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
         return ParameterIdentificationMenuBarButtons.ParameterIdentificationFeedbackView(new MenuBarItemId("Show Visual Feedback", _menuId++));
      }

      private IMenuBarItem createTimeProfileButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileParameterIdentification(new MenuBarItemId("Time Profile", _menuId++));
      }

      private IMenuBarItem createParameterIdentificationRunButton()
      {
         return ParameterIdentificationMenuBarButtons.RunParameterIdentification(new MenuBarItemId("Run", _menuId++));
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