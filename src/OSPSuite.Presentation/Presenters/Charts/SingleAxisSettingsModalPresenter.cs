using OSPSuite.Assets;
using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface ISingleAxisSettingsModalPresenter : IDisposablePresenter
   {
      void ShowView();
      void Edit(Axis axis);
   }

   public class SingleAxisSettingsModalPresenter : AbstractDisposableContainerPresenter<ISingleAxisSettingsModalView, ISingleAxisSettingsModalPresenter>, ISingleAxisSettingsModalPresenter
   {
      private readonly ISingleAxisSettingsPresenter _singleAxisSettingsPresenter;

      public SingleAxisSettingsModalPresenter(ISingleAxisSettingsModalView view, ISingleAxisSettingsPresenter singleAxisSettingsPresenter) : base(view)
      {
         _view.SetSummaryView(singleAxisSettingsPresenter.BaseView);
         AddSubPresenters(singleAxisSettingsPresenter);
         _view.Caption = Captions.AxisSettings;
         _singleAxisSettingsPresenter = singleAxisSettingsPresenter;
      }

      public void Edit(Axis axis)
      {
         _singleAxisSettingsPresenter.Edit(axis);
      }

      public void ShowView()
      {
         _view.Display();
      }
   }
}