using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationWeightedObservedDataPresenter : IPresenter<IParameterIdentificationWeightedObservedDataView>
   {
      void Edit(WeightedObservedData weightedObservedData);

      string Caption { get; set; }
   }

   public class ParameterIdentificationWeightedObservedDataPresenter : AbstractPresenter<IParameterIdentificationWeightedObservedDataView, IParameterIdentificationWeightedObservedDataPresenter>, IParameterIdentificationWeightedObservedDataPresenter
   {
      private readonly IWeightedDataRepositoryDataPresenter _dataPresenter;
      private readonly ISimpleChartPresenter _chartPresenter;
      private bool _alreadyEditing;

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
   }
}