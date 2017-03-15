using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleCurveSettingsModalPresenter : IDisposablePresenter
   {
      void ShowView();
      void Edit(ICurve curve);
   }

   public class SingleCurveSettingsModalPresenter : AbstractDisposableContainerPresenter<ISingleCurveSettingsModalView, ISingleCurveSettingsModalPresenter>, ISingleCurveSettingsModalPresenter
   {
      private readonly ISingleCurveSettingsPresenter _singleCurveSettingsPresenter;

      public SingleCurveSettingsModalPresenter(ISingleCurveSettingsModalView view, ISingleCurveSettingsPresenter singleCurveSettingsPresenter)
         : base(view)
      {
         _view.SetSummaryView(singleCurveSettingsPresenter.BaseView);
         AddSubPresenters(singleCurveSettingsPresenter);
         _view.Caption = Captions.CurveSettings;
         _singleCurveSettingsPresenter = singleCurveSettingsPresenter;
      }

      public void Edit(ICurve curve)
      {
         _singleCurveSettingsPresenter.Edit(curve);
      }

      public void ShowView()
      {
         _view.Display();
      }
   }
}