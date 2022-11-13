using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
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
   }

   public class ParameterIdentificationWeightedObservedDataPresenter : AbstractPresenter<IParameterIdentificationWeightedObservedDataView, IParameterIdentificationWeightedObservedDataPresenter>, IParameterIdentificationWeightedObservedDataPresenter
   {
      private readonly IWeightedDataRepositoryDataPresenter _dataPresenter;
      private readonly ISimpleChartPresenter _chartPresenter;
      private bool _alreadyEditing;
      private WeightedObservedData _observedData;

      public ParameterIdentificationWeightedObservedDataPresenter(IParameterIdentificationWeightedObservedDataView view, IWeightedDataRepositoryDataPresenter dataPresenter, ISimpleChartPresenter chartPresenter) : base(view)
      {
         _dataPresenter = dataPresenter;
         _chartPresenter = chartPresenter;

         AddSubPresenters(_dataPresenter, _chartPresenter);

         view.AddDataView(_dataPresenter.BaseView);
         view.AddChartView(_chartPresenter.BaseView);
      }

      public void Edit(WeightedObservedData weightedObservedData)
      {
         if (_alreadyEditing) return;

         _observedData = weightedObservedData;
         _dataPresenter.EditObservedData(weightedObservedData);
         _chartPresenter.PlotObservedData(weightedObservedData);
         _chartPresenter.LogLinSelectionEnabled = true;
         _chartPresenter.HotTracked = hotTracked;
         Caption = weightedObservedData.DisplayName;
         _alreadyEditing = true;
      }

      public string Caption
      {
         get => View.Caption;
         set => View.Caption = value;
      }

      private void hotTracked(int rowIndex) => _dataPresenter.SelectRow(rowIndex);

      private bool shouldHandleEvent(ObservedDataEvent eventToHandle)
      {
         return Equals(eventToHandle.ObservedData, _observedData.ObservedData);
      }

      public void Handle(ObservedDataValueChangedEvent eventToHandle)
      {
         if (!shouldHandleEvent(eventToHandle))
            return;

         _alreadyEditing = false;
         Edit(_observedData);
      }
   }
}