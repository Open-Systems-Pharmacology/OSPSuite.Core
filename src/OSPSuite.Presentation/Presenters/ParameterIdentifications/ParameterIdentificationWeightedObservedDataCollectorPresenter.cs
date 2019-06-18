using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationWeightedObservedDataCollectorPresenter : IPresenter<IParameterIdentificationWeightedObservedDataCollectorView>, IParameterIdentificationPresenter, ILatchable
   {
      void AddObservedData(WeightedObservedData weightedObservedData);
      void SelectObservedData(WeightedObservedData weightedObservedData);
      void RemoveObservedData(WeightedObservedData weightedObservedData);
      void Refresh();
      void ObservedDataViewSelected(IView view);
   }

   public class ParameterIdentificationWeightedObservedDataCollectorPresenter : AbstractPresenter<IParameterIdentificationWeightedObservedDataCollectorView, IParameterIdentificationWeightedObservedDataCollectorPresenter>, IParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      private readonly IApplicationController _applicationController;
      private readonly IEventPublisher _eventPublisher;
      private ParameterIdentification _parameterIdentification;
      public bool IsLatched { get; set; }

      private readonly Cache<WeightedObservedData, IParameterIdentificationWeightedObservedDataPresenter> _allObservedDataPresenters;

      public ParameterIdentificationWeightedObservedDataCollectorPresenter(IParameterIdentificationWeightedObservedDataCollectorView view, IApplicationController applicationController, IEventPublisher eventPublisher) : base(view)
      {
         _applicationController = applicationController;
         _eventPublisher = eventPublisher;
         _allObservedDataPresenters = new Cache<WeightedObservedData, IParameterIdentificationWeightedObservedDataPresenter>(onMissingKey: x => null);
      }

      public void EditParameterIdentification(ParameterIdentification parameterIdentification)
      {
         _parameterIdentification = parameterIdentification;
         clear();
         var allWeightedObservedData = _parameterIdentification.AllOutputMappings.Select(x => x.WeightedObservedData).ToList();
         addWeightedObservedDataToView(allWeightedObservedData);
         SelectObservedData(allWeightedObservedData.FirstOrDefault());
      }

      private void addWeightedObservedDataToView(List<WeightedObservedData> allWeightedObservedData)
      {
         this.DoWithinLatch(() =>
         {
            using (new BatchUpdate(View))
            {
               allWeightedObservedData.Each(AddObservedData);
            }
         });
      }

      public void AddObservedData(WeightedObservedData weightedObservedData)
      {
         // You can get a null weighted observed data when a mapping has not been assigned observed data yet or if mapping is corrupted
         if (weightedObservedData?.ObservedData == null)
            return;

         if (_allObservedDataPresenters.Contains(weightedObservedData))
            return;

         var presenter = _applicationController.Start<IParameterIdentificationWeightedObservedDataPresenter>();
         _allObservedDataPresenters.Add(weightedObservedData, presenter);
         edit(weightedObservedData);
         _view.AddObservedDataView(presenter.View);
         SelectObservedData(weightedObservedData);
      }

      public void SelectObservedData(WeightedObservedData weightedObservedData)
      {
         if (IsLatched) return;

         var observedDataPresenter = edit(weightedObservedData);
         if (observedDataPresenter == null) return;

         _view.SelectObservedDataView(observedDataPresenter.View);
      }

      public void RemoveObservedData(WeightedObservedData weightedObservedData)
      {
         var presenter = _allObservedDataPresenters[weightedObservedData];
         if (presenter == null)
            return;

         _allObservedDataPresenters.Remove(weightedObservedData);
         removeObservedDataPresenter(presenter);
      }

      private void removeObservedDataPresenter(IParameterIdentificationWeightedObservedDataPresenter presenter)
      {
         presenter.ReleaseFrom(_eventPublisher);
         _view.RemoveObservedDataView(presenter.View);
      }

      public void Refresh()
      {
         EditParameterIdentification(_parameterIdentification);
      }

      public void ObservedDataViewSelected(IView view)
      {
         this.DoWithinLatch(() =>
         {
            var weightedObservedData = findObservedDataEditedBy(view);
            edit(weightedObservedData);
         });
      }

      private IParameterIdentificationWeightedObservedDataPresenter edit(WeightedObservedData weightedObservedData)
      {
         var observedDataPresenter = _allObservedDataPresenters[weightedObservedData];
         observedDataPresenter?.Edit(weightedObservedData);

         return observedDataPresenter;
      }

      private WeightedObservedData findObservedDataEditedBy(IView view)
      {
         return _allObservedDataPresenters.KeyValues.Where(x => Equals(x.Value.View, view)).Select(x => x.Key).FirstOrDefault();
      }

      private void clear()
      {
         using (new BatchUpdate(View))
         {
            _view.Clear();
            _allObservedDataPresenters.Each(p => p.ReleaseFrom(_eventPublisher));
            _allObservedDataPresenters.Clear();
         }
      }

      public override void ReleaseFrom(IEventPublisher eventPublisher)
      {
         try
         {
            clear();
         }
         finally
         {
            base.ReleaseFrom(eventPublisher);
         }
      }
   }
}