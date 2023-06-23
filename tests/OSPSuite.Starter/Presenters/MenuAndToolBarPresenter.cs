using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Repositories;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Presenters
{
   public interface IMenuAndToolBarPresenter : IMainViewItemPresenter
   {
   }

   public class MenuAndToolBarPresenter : AbstractMenuAndToolBarPresenter, IMenuAndToolBarPresenter
   {
      private int _menuId;
      private readonly IContainer _container;

      public MenuAndToolBarPresenter(IMenuAndToolBarView view, IMenuBarItemRepository menuBarItemRepository, IMRUProvider mruProvider, IContainer container) : base(view, menuBarItemRepository, mruProvider)
      {
         _container = container;
      }

      protected override void DisableMenuBarItemsForProgramStart()
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

         _view.AddPageHeaderItemLinks(CommonMenuBarButtons.Help(new MenuBarItemId("Help", _menuId++), _container));
      }

      private IButtonGroup createSensitivityAnalysisAnalyses()
      {
         return CreateButtonGroup.WithCaption("Analyses")
            .WithButton(CreateRibbonButton.From(createSensitivityAnalysisAnalysesButton()))
            .WithId("SAA");
      }

      private IMenuBarItem createSensitivityAnalysisAnalysesButton()
      {
         return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisPKParameterAnalysis(new MenuBarItemId("Sensitivity Analysis", _menuId++), _container);
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
         return SensitivityAnalysisMenuBarButtons.SensitivityAnalysisFeedbackView(new MenuBarItemId("Show Visual Feedback SA", _menuId++), _container);
      }

      private IMenuBarItem createSensitivityAnalysisRunButton()
      {
         return SensitivityAnalysisMenuBarButtons.RunSensitivityAnalysis(new MenuBarItemId("Run SA", _menuId++), _container);
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
         return ParameterIdentificationMenuBarButtons.TimeProfilePredictionInterval(new MenuBarItemId("Prediction Interval", _menuId++), _container);
      }

      private IMenuBarItem createVisualPredictiveCheckButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileVPCInterval(new MenuBarItemId("Visual Predictive Check", _menuId++), _container);
      }

      private IMenuBarItem createConfidenceIntervalButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileConfidenceInterval(new MenuBarItemId("Confidence Interval", _menuId++), _container);
      }

      private IMenuBarItem createCorrelationMatrixButton()
      {
         return ParameterIdentificationMenuBarButtons.CorrelationMatrixParameterIdentification(new MenuBarItemId("Correlation Matrix", _menuId++), _container);
      }

      private IMenuBarItem createCovarianceMatrixButton()
      {
         return ParameterIdentificationMenuBarButtons.CovarianceMatrixParameterIdentification(new MenuBarItemId("Covariance Matrix", _menuId++), _container);
      }

      private IMenuBarItem createHistogramOfResidualsButton()
      {
         return ParameterIdentificationMenuBarButtons.ResidualHistogramParameterIdentification(new MenuBarItemId("Histogram of Residuals", _menuId++), _container);
      }

      private IMenuBarItem createResidualsVsTimeButton()
      {
         return ParameterIdentificationMenuBarButtons.ResidualsVsTimeParameterIdentification(new MenuBarItemId("Residual Vs. Time", _menuId++), _container);
      }

      private IMenuBarItem createPredictedVsObservedButton()
      {
         return ParameterIdentificationMenuBarButtons.PredictedVsObservedParameterIdentification(new MenuBarItemId("Predicted Vs. Observed", _menuId++), _container);
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
         return ParameterIdentificationMenuBarButtons.ParameterIdentificationFeedbackView(new MenuBarItemId("Show Visual Feedback", _menuId++), _container);
      }

      private IMenuBarItem createTimeProfileButton()
      {
         return ParameterIdentificationMenuBarButtons.TimeProfileParameterIdentification(new MenuBarItemId("Time Profile", _menuId++), _container);
      }

      private IMenuBarItem createParameterIdentificationRunButton()
      {
         return ParameterIdentificationMenuBarButtons.RunParameterIdentification(new MenuBarItemId("Run", _menuId++), _container);
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