using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationParameterSelectionPresenter : IParameterIdentificationItemPresenter,
     IListener<RenamedEvent>, IListener<SimulationRemovedEvent>, IListener<SimulationReplacedInParameterAnalyzableEvent>
   {
      void SimulationAdded(ISimulation simulation);
      void SimulationRemoved(ISimulation simulation);
      void AddIdentificationParameter();
      void AddLinkedParameter();
   }

   public class ParameterIdentificationParameterSelectionPresenter : AbstractSubPresenter<IParameterIdentificationParameterSelectionView, IParameterIdentificationParameterSelectionPresenter>, IParameterIdentificationParameterSelectionPresenter
   {
      private readonly ISimulationParametersPresenter _simulationParametersPresenter;
      private readonly IParameterIdentificationIdentificationParametersPresenter _identificationParametersPresenter;
      private readonly IParameterIdentificationLinkedParametersPresenter _linkedParametersPresenter;
      private ParameterIdentification _parameterIdentification;

      public ParameterIdentificationParameterSelectionPresenter(IParameterIdentificationParameterSelectionView view, ISimulationParametersPresenter simulationParametersPresenter,
         IParameterIdentificationIdentificationParametersPresenter identificationParametersPresenter, IParameterIdentificationLinkedParametersPresenter linkedParametersPresenter) : base(view)
      {
         _simulationParametersPresenter = simulationParametersPresenter;
         _identificationParametersPresenter = identificationParametersPresenter;
         _linkedParametersPresenter = linkedParametersPresenter;
         _view.AddSimulationParametersView(_simulationParametersPresenter.BaseView);
         _view.AddIdentificationParametersView(_identificationParametersPresenter.BaseView);
         _view.AddLinkedParametersView(_linkedParametersPresenter.BaseView);
         AddSubPresenters(simulationParametersPresenter, _identificationParametersPresenter, _linkedParametersPresenter);
         _identificationParametersPresenter.IdentificationParameterSelected += (o, e) => identificationParameterSelected(e.IdentificationParameter);
         _identificationParametersPresenter.NoIdentificationParameterSelected += (o, e) => clearSelection();
         _linkedParametersPresenter.ParameterRemovedFromIdentificationParameter += (o, e) => parameterRemovedFromIdentificationParameter();
         _linkedParametersPresenter.ParameterUnlinkedFromIdentificationParameter += (o, e) => parameterUnlinkedFromIdentificationParameter(e.LinkedParameter);
         _linkedParametersPresenter.ParameterLinkedToIdentificationParameter += (o, e) => parameterLinkedToIdentificationParameter();
      }

      private void clearSelection()
      {
         _linkedParametersPresenter.ClearSelection();
         _view.LinkedParametersCaption = Captions.ParameterIdentification.LinkedParameters;
      }

      private void parameterUnlinkedFromIdentificationParameter(ParameterSelection unlinkedParameter)
      {
         _identificationParametersPresenter.AddParameter(unlinkedParameter);
      }

      private void parameterRemovedFromIdentificationParameter()
      {
         _identificationParametersPresenter.Refresh();
      }

      private void parameterLinkedToIdentificationParameter()
      {
         _identificationParametersPresenter.Refresh();
      }

      private void identificationParameterSelected(IdentificationParameter identificationParameter)
      {
         _linkedParametersPresenter.Edit(identificationParameter);
         _view.LinkedParametersCaption = Captions.ParameterIdentification.LinkedParametersIn(identificationParameter.Name);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         _simulationParametersPresenter.EditParameterAnalysable(parameterIdentification);
         _identificationParametersPresenter.EditParameterIdentification(parameterIdentification);
         _linkedParametersPresenter.EditParameterIdentification(parameterIdentification);
      }

      public void SimulationAdded(ISimulation simulation)
      {
         _simulationParametersPresenter.AddParametersOf(simulation);
      }

      public void SimulationRemoved(ISimulation simulation)
      {
         _simulationParametersPresenter.RemoveParametersFrom(simulation);
         _identificationParametersPresenter.Refresh();
         _linkedParametersPresenter.Refresh();
      }

      public void AddIdentificationParameter()
      {
         _identificationParametersPresenter.AddParameters(_simulationParametersPresenter.SelectedParameters);
      }

      public void AddLinkedParameter()
      {
         _linkedParametersPresenter.AddLinkedParameters(_simulationParametersPresenter.SelectedParameters);
      }

      public void Handle(RenamedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         _linkedParametersPresenter.Refresh();
         _simulationParametersPresenter.Refresh();
      }

      private bool canHandle(RenamedEvent eventToHandle)
      {
         return _parameterIdentification.UsesSimulation(eventToHandle.RenamedObject as ISimulation);
      }

      public void Handle(SimulationRemovedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;
         refreshSubPresenters();
      }

      private void refreshSubPresenters()
      {
         _linkedParametersPresenter.Refresh();
         _simulationParametersPresenter.Refresh();
         _identificationParametersPresenter.Refresh();
      }

      private bool canHandle(SimulationRemovedEvent eventToHandle)
      {
         return eventToHandle.Simulation.IsAnImplementationOf<ISimulation>();
      }

      public void Handle(SimulationReplacedInParameterAnalyzableEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            refreshSubPresenters();
      }

      private bool canHandle(SimulationReplacedInParameterAnalyzableEvent eventToHandle)
      {
         return Equals(eventToHandle.ParameterAnalysable, _parameterIdentification);
      }
   }
}