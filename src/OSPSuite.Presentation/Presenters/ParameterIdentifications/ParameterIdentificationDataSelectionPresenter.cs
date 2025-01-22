using System;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationDataSelectionPresenter : IParameterIdentificationItemPresenter,
      IListener<ObservedDataAddedToAnalysableEvent>,
      IListener<ObservedDataRemovedFromAnalysableEvent>,
      IListener<RenamedEvent>,
      IListener<SimulationRemovedEvent>,
      IListener<SimulationReplacedInParameterAnalyzableEvent>
   {
      event EventHandler<SimulationEventArgs> SimulationAdded;
      event EventHandler<SimulationEventArgs> SimulationRemoved;
   }

   public class ParameterIdentificationDataSelectionPresenter : AbstractSubPresenter<IParameterIdentificationDataSelectionView, IParameterIdentificationDataSelectionPresenter>, IParameterIdentificationDataSelectionPresenter
   {
      private readonly IParameterIdentificationSimulationSelectionPresenter _simulationSelectionPresenter;
      private readonly IParameterIdentificationOutputMappingPresenter _outputMappingPresenter;
      private readonly IParameterIdentificationWeightedObservedDataPresenter _weightedObservedDataPresenter;
      private ParameterIdentification _parameterIdentification;
      public event EventHandler<SimulationEventArgs> SimulationAdded = delegate { };
      public event EventHandler<SimulationEventArgs> SimulationRemoved = delegate { };

      public ParameterIdentificationDataSelectionPresenter(IParameterIdentificationDataSelectionView view,
         IParameterIdentificationSimulationSelectionPresenter simulationSelectionPresenter,
         IParameterIdentificationOutputMappingPresenter outputMappingPresenter,
         IParameterIdentificationWeightedObservedDataPresenter weightedObservedDataPresenter) : base(view)
      {
         _simulationSelectionPresenter = simulationSelectionPresenter;
         _outputMappingPresenter = outputMappingPresenter;
         _weightedObservedDataPresenter = weightedObservedDataPresenter;
         view.AddSimulationSelectionView(_simulationSelectionPresenter.BaseView);
         view.AddOutputMappingView(_outputMappingPresenter.BaseView);
         view.AddWeightedObservedDataCollectorView(_weightedObservedDataPresenter.BaseView);
         AddSubPresenters(_simulationSelectionPresenter, _outputMappingPresenter, _weightedObservedDataPresenter);

         _simulationSelectionPresenter.SimulationAdded += (o, e) => simulationAdded(e);
         _simulationSelectionPresenter.SimulationRemoved += (o, e) => simulationRemoved(e);

         _outputMappingPresenter.ObservedDataMapped += (o, e) => observedDataMapped(e.OutputMapping);
         _outputMappingPresenter.ObservedDataUnmapped += (o, e) => observedDataUnmapped(e.OutputMapping.WeightedObservedData);
         _outputMappingPresenter.ObservedDataSelected += (o, e) => observedDataSelected(e.OutputMapping);
      }

      private void observedDataUnmapped(WeightedObservedData weightedObservedData) => _weightedObservedDataPresenter.Clear(weightedObservedData);

      private void observedDataSelected(OutputMapping outputMapping) => _weightedObservedDataPresenter.Edit(outputMapping);

      private void observedDataMapped(OutputMapping outputMapping) => _weightedObservedDataPresenter.Edit(outputMapping);

      private void simulationAdded(SimulationEventArgs e)
      {
         updateOutputAndWeightsPresenters();
         SimulationAdded(this, e);
      }

      private void updateOutputAndWeightsPresenters()
      {
         _outputMappingPresenter.Refresh();
         ViewChanged();
      }

      private void simulationRemoved(SimulationEventArgs e)
      {
         updateOutputAndWeightsPresenters();
         SimulationRemoved(this, e);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         _simulationSelectionPresenter.EditParameterIdentification(_parameterIdentification);
         _outputMappingPresenter.EditParameterIdentification(_parameterIdentification);
      }

      public void Handle(ObservedDataAddedToAnalysableEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         _simulationSelectionPresenter.Refresh();
         _outputMappingPresenter.UpdateCache();
      }

      private bool canHandle(AnalysableEvent analysableEvent)
      {
         return identificationUsesSimulation(analysableEvent.Analysable as ISimulation);
      }

      private bool identificationUsesSimulation(ISimulation simulation) => _parameterIdentification.UsesSimulation(simulation);

      public void Handle(ObservedDataRemovedFromAnalysableEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            _simulationSelectionPresenter.Refresh();
      }

      public void Handle(RenamedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         _simulationSelectionPresenter.Refresh();
         _outputMappingPresenter.Refresh();
      }

      private bool canHandle(RenamedEvent eventToHandle) => identificationUsesSimulation(eventToHandle.RenamedObject as ISimulation);

      public void Handle(SimulationRemovedEvent eventToHandle) => refreshSubPresenters();

      private void refreshSubPresenters()
      {
         _simulationSelectionPresenter.Refresh();
         _outputMappingPresenter.Refresh();
      }

      public void Handle(SimulationReplacedInParameterAnalyzableEvent eventToHandle)
      {
         if (canHandle(eventToHandle))
            refreshSubPresenters();
      }

      private bool canHandle(SimulationReplacedInParameterAnalyzableEvent eventToHandle) => Equals(eventToHandle.ParameterAnalysable, _parameterIdentification);
   }
}