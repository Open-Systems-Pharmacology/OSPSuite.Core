using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface IEditSensitivityAnalysisPresenter : ISingleStartPresenter<SensitivityAnalysis>
   {
   }

   public class EditSensitivityAnalysisPresenter : EditAnalyzablePresenter<IEditSensitivityAnalysisView, IEditSensitivityAnalysisPresenter, SensitivityAnalysis, ISensitivityAnalysisItemPresenter>, IEditSensitivityAnalysisPresenter
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
  
      public EditSensitivityAnalysisPresenter(IEditSensitivityAnalysisView view, 
         ISubPresenterItemManager<ISensitivityAnalysisItemPresenter> subPresenterItemManager,
         IOSPSuiteExecutionContext executionContext,
         ISimulationAnalysisPresenterFactory simulationAnalysisPresenterFactory,
         ISimulationAnalysisPresenterContextMenuFactory contextMenuFactory, IPresentationSettingsTask presentationSettingsTask,
         ISensitivityAnalysisPKParameterAnalysisCreator simulationAnalysisCreator) :
            base(view, subPresenterItemManager, SensitivityAnalysisItems.All, simulationAnalysisPresenterFactory, contextMenuFactory, presentationSettingsTask, simulationAnalysisCreator)
      {
         _executionContext = executionContext;
      }

      protected override void InitializeSubPresentersWith(SensitivityAnalysis sensitivityAnalysis)
      {
         loadAllSimulations();

         // If a simulation was removed when the analysis was not loaded we have to remove the sensitivity parameters
         if (!sensitivityAnalysis.AllSimulations.Any())
            removeSensitivityParameters(sensitivityAnalysis);

         _subPresenterItemManager.AllSubPresenters.Each(x => x.EditSensitivityAnalysis(sensitivityAnalysis));
      }

      private void removeSensitivityParameters(SensitivityAnalysis sensitivityAnalysis)
      {
         sensitivityAnalysis.RemoveAllSensitivityParameters();
      }

      private void loadAllSimulations()
      {
         Analyzable.AllSimulations.Each(_executionContext.Load);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = Captions.SensitivityAnalysis.EditSensitivityAnalysis(Analyzable.Name);
      }

      protected override bool CanHandle(AnalysableEvent analysableEvent)
      {
         return Equals(analysableEvent.Analysable, Analyzable);
      }

      public override string PresentationKey => PresenterConstants.PresenterKeys.EditSensitivityAnalysisPresenter;

      public override void ViewChanged()
      {
         base.ViewChanged();
         _executionContext.ProjectChanged();
      }
   }
}