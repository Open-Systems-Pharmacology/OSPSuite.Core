using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IEditParameterIdentificationPresenter : ISingleStartPresenter<ParameterIdentification>,
      IListener<SimulationAnalysisCreatedEvent>,
      IListener<ParameterIdentificationResultsUpdatedEvent>

   {
   }

   public class EditParameterIdentificationPresenter : EditAnalyzablePresenter<IEditParameterIdentificationView, IEditParameterIdentificationPresenter, ParameterIdentification, IParameterIdentificationItemPresenter>, IEditParameterIdentificationPresenter
   {
      private readonly IOSPSuiteExecutionContext _executionContext;

      public EditParameterIdentificationPresenter(IEditParameterIdentificationView view,
         ISubPresenterItemManager<IParameterIdentificationItemPresenter> subPresenterItemManager,
         IOSPSuiteExecutionContext executionContext,
         ISimulationAnalysisPresenterFactory simulationAnalysisPresenterFactory,
         ISimulationAnalysisPresenterContextMenuFactory contextMenuFactory,
         IPresentationSettingsTask presentationSettingsTask,
         IParameterIdentificationAnalysisCreator simulationAnalysisCreator) :
            base(view, subPresenterItemManager, ParameterIdentificationItems.All, simulationAnalysisPresenterFactory, contextMenuFactory, presentationSettingsTask, simulationAnalysisCreator)
      {
         _executionContext = executionContext;
      }

      protected override void InitializeSubPresentersWith(ParameterIdentification parameterIdentification)
      {
         loadAllSimulations();
         _subPresenterItemManager.AllSubPresenters.Each(x => x.EditParameterIdentification(parameterIdentification));
      }

      public override void InitializeWith(ICommandCollector commandRegister)
      {
         base.InitializeWith(commandRegister);
         dataSelectionPresenter.SimulationAdded += (o, e) => simulationAdded(e);
         dataSelectionPresenter.SimulationRemoved += (o, e) => simulationRemoved(e);
      }

      private void simulationRemoved(SimulationEventArgs e)
      {
         parameterSelectionPresenter.SimulationRemoved(e.Simulation);
         configurationPresenter.SimulationRemoved(e.Simulation);
      }

      private void simulationAdded(SimulationEventArgs e)
      {
         parameterSelectionPresenter.SimulationAdded(e.Simulation);
         configurationPresenter.SimulationAdded(e.Simulation);
      }

      protected override void UpdateCaption()
      {
         _view.Caption = Captions.ParameterIdentification.EditParameterIdentification(Analyzable.Name);
      }

      private void loadAllSimulations()
      {
         Analyzable.AllSimulations.Each(_executionContext.Load);
      }

      private IParameterIdentificationDataSelectionPresenter dataSelectionPresenter => _subPresenterItemManager.PresenterAt(ParameterIdentificationItems.Data);
      private IParameterIdentificationParameterSelectionPresenter parameterSelectionPresenter => _subPresenterItemManager.PresenterAt(ParameterIdentificationItems.Parameters);
      private IParameterIdentificationConfigurationPresenter configurationPresenter => _subPresenterItemManager.PresenterAt(ParameterIdentificationItems.Configuration);

      protected override bool CanHandle(AnalysableEvent analysableEvent)
      {
         return Equals(analysableEvent.Analysable, Analyzable);
      }

      public override string PresentationKey => PresenterConstants.PresenterKeys.EditParameterIdentificationPresenter;

      public void Handle(ParameterIdentificationResultsUpdatedEvent eventToHandle)
      {
         if (!CanHandle(eventToHandle))
            return;

         UpdateResultsInCharts();
      }

      protected override void UpdateTrafficLightFor(ISimulationAnalysisPresenter simulationAnalysisPresenter)
      {
         //disable feature in PI as there is no logic yet to determine if PI results are updated or not
      }

      public override void ViewChanged()
      {
         base.ViewChanged();
         _executionContext.ProjectChanged();
      }
   }
}