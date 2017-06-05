using System;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.ObservedData;

namespace OSPSuite.Presentation.Presenters.ObservedData
{
   public interface IDataRepositoryChartPresenter : IDataRepositoryItemPresenter,
   IListener<ObservedDataValueChangedEvent>,
   IListener<ObservedDataTableChangedEvent>
   {
      Action<int> HotTracked { set; }
   }

   public class DataRepositoryChartPresenter : AbstractSubPresenter<IDataRepositoryChartView, IDataRepositoryChartPresenter>, IDataRepositoryChartPresenter
   {
      private readonly ISimpleChartPresenter _simpleChartPresenter;
      private DataRepository _observedData;

      public DataRepositoryChartPresenter(IDataRepositoryChartView view, ISimpleChartPresenter simpleChartPresenter)
         : base(view)
      {
         _simpleChartPresenter = simpleChartPresenter;
         AddSubPresenters(_simpleChartPresenter);

         _view.AddChart(_simpleChartPresenter.View);
      }

      public void EditObservedData(DataRepository observedData)
      {
         _observedData = observedData;
         plotObservedData(observedData);
      }

      public override void Initialize()
      {
         base.Initialize();
         _simpleChartPresenter.LogLinSelectionEnabled = true;
      }

      public Action<int> HotTracked
      {
         set { _simpleChartPresenter.HotTracked = value; }
      }

      private void plotObservedData(DataRepository dataRepository)
      {
         _simpleChartPresenter.PlotObservedData(dataRepository);
         _simpleChartPresenter.Chart.AxisBy(AxisTypes.X).GridLines = true;
         _simpleChartPresenter.Chart.AxisBy(AxisTypes.Y).GridLines = true;
      }

      private bool shouldHandleEvent(ObservedDataEvent eventToHandle)
      {
         return Equals(eventToHandle.ObservedData, _observedData);
      }

      public void Handle(ObservedDataValueChangedEvent eventToHandle)
      {
         handle(eventToHandle);
      }

      private void handle(ObservedDataEvent eventToHandle)
      {
         if (!shouldHandleEvent(eventToHandle))
            return;

         plotObservedData(eventToHandle.ObservedData);
      }

      public void Handle(ObservedDataTableChangedEvent eventToHandle)
      {
         handle(eventToHandle);
      }
   }
}