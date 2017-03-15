using System;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Views.ParameterIdentifications;

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
      private readonly IParameterIdentificationWeightedObservedDataCollectorPresenter _weightedObservedDataCollectorPresenter;
      private ParameterIdentification _parameterIdentification;
      public event EventHandler<SimulationEventArgs> SimulationAdded = delegate { };
      public event EventHandler<SimulationEventArgs> SimulationRemoved = delegate { };

      public ParameterIdentificationDataSelectionPresenter(IParameterIdentificationDataSelectionView view,
         IParameterIdentificationSimulationSelectionPresenter simulationSelectionPresenter,
         IParameterIdentificationOutputMappingPresenter outputMappingPresenter,
         IParameterIdentificationWeightedObservedDataCollectorPresenter weightedObservedDataCollectorPresenter) : base(view)
      {
         _simulationSelectionPresenter = simulationSelectionPresenter;
         _outputMappingPresenter = outputMappingPresenter;
         _weightedObservedDataCollectorPresenter = weightedObservedDataCollectorPresenter;
         view.AddSimulationSelectionView(_simulationSelectionPresenter.BaseView);
         view.AddOutputMappingView(_outputMappingPresenter.BaseView);
         view.AddWeightedObservedDataCollectorView(_weightedObservedDataCollectorPresenter.BaseView);
         AddSubPresenters(_simulationSelectionPresenter, _outputMappingPresenter, _weightedObservedDataCollectorPresenter);

         _simulationSelectionPresenter.SimulationAdded += (o, e) => simulationAdded(e);
         _simulationSelectionPresenter.SimulationRemoved += (o, e) => simulationRemoved(e);

         _outputMappingPresenter.ObservedDataMapped += (o, e) => observedDataMapped(e.WeightedObservedData);
         _outputMappingPresenter.ObservedDataUnmapped += (o, e) => observedDataUnmapped(e.WeightedObservedData);
         _outputMappingPresenter.ObservedDataSelected += (o, e) => observedDataSelected(e.WeightedObservedData);
      }

      private void observedDataUnmapped(WeightedObservedData weightedObservedData)
      {
         _weightedObservedDataCollectorPresenter.RemoveObservedData(weightedObservedData);
      }

      private void observedDataSelected(WeightedObservedData weightedObservedData)
      {
         _weightedObservedDataCollectorPresenter.SelectObservedData(weightedObservedData);
      }

      private void observedDataMapped(WeightedObservedData weightedObservedData)
      {
         _weightedObservedDataCollectorPresenter.AddObservedData(weightedObservedData);
      }

      private void simulationAdded(SimulationEventArgs e)
      {
         updateOutputAndWeightsPresenters();
         SimulationAdded(this, e);
      }

      private void updateOutputAndWeightsPresenters()
      {
         _outputMappingPresenter.Refresh();
         _weightedObservedDataCollectorPresenter.Refresh();
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
         _weightedObservedDataCollectorPresenter.EditParameterIdentification(_parameterIdentification);
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

      private bool identificationUsesSimulation(ISimulation simulation)
      {
         return _parameterIdentification.UsesSimulation(simulation);
      }

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

      private bool canHandle(RenamedEvent eventToHandle)
      {
         return identificationUsesSimulation(eventToHandle.RenamedObject as ISimulation);
      }

      public void Handle(SimulationRemovedEvent eventToHandle)
      {
         refreshSubPresenters();
      }

      private void refreshSubPresenters()
      {
         _simulationSelectionPresenter.Refresh();
         _outputMappingPresenter.Refresh();
         _weightedObservedDataCollectorPresenter.Refresh();
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