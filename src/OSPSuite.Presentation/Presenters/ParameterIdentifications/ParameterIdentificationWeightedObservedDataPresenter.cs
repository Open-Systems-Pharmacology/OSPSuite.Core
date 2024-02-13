using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationWeightedObservedDataPresenter : IPresenter<IParameterIdentificationWeightedObservedDataView>, IListener<ObservedDataValueChangedEvent>
   {
      void Edit(WeightedObservedData weightedObservedData);

      string Caption { get; set; }

      /// <summary>
      ///    Clears the display if the <paramref name="weightedObservedData" /> is the same as the one being displayed
      /// </summary>
      void Clear(WeightedObservedData weightedObservedData);
   }

   public class ParameterIdentificationWeightedObservedDataPresenter : AbstractPresenter<IParameterIdentificationWeightedObservedDataView, IParameterIdentificationWeightedObservedDataPresenter>, IParameterIdentificationWeightedObservedDataPresenter
   {
      private readonly IWeightedDataRepositoryDataPresenter _dataPresenter;
      private readonly ISimpleChartPresenter _chartPresenter;
      private WeightedObservedData _observedData;

      public ParameterIdentificationWeightedObservedDataPresenter(IParameterIdentificationWeightedObservedDataView view, IWeightedDataRepositoryDataPresenter dataPresenter, ISimpleChartPresenter chartPresenter) : base(view)
      {
         _dataPresenter = dataPresenter;
         _chartPresenter = chartPresenter;

         AddSubPresenters(_dataPresenter, _chartPresenter);

         view.AddDataView(_dataPresenter.BaseView);
         view.AddChartView(_chartPresenter.BaseView);
         clear();
      }

      public void Edit(WeightedObservedData weightedObservedData)
      {
         if (_observedData == weightedObservedData || weightedObservedData == null)
            return;

         _view.SetTitle(weightedObservedData.DisplayName);

         _observedData = weightedObservedData;
         _dataPresenter.EditObservedData(weightedObservedData);
         _chartPresenter.PlotObservedData(weightedObservedData);
         _chartPresenter.LogLinSelectionEnabled = true;
         _chartPresenter.HotTracked = hotTracked;
         Caption = weightedObservedData.DisplayName;
      }

      public void Clear(WeightedObservedData weightedObservedData)
      {
         if (_observedData != weightedObservedData)
            return;

         clear();
      }

      private void clear()
      {
         _view.SetTitle(Captions.SelectMappingToShowObservedData);
         _observedData = null;
         _chartPresenter.Clear();
         _dataPresenter.Clear();
      }

      public string Caption
      {
         get => View.Caption;
         set => View.Caption = value;
      }

      private void hotTracked(int rowIndex) => _dataPresenter.SelectRow(rowIndex);

      private bool shouldHandleEvent(ObservedDataEvent eventToHandle) => Equals(eventToHandle.ObservedData, _observedData.ObservedData);

      public void Handle(ObservedDataValueChangedEvent eventToHandle)
      {
         if (!shouldHandleEvent(eventToHandle))
            return;

         Edit(_observedData);
      }
   }
}