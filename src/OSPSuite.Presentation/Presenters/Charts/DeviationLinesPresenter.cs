using OSPSuite.Presentation.Views.Charts;

namespace OSPSuite.Presentation.Presenters.Charts
{
   public interface IDeviationLinesPresenter : IDisposablePresenter, IPresenter<IDeviationLinesView>
   {
      float GetFoldValue();
   }

   public class DeviationLinesPresenter : AbstractDisposablePresenter<IDeviationLinesView, IDeviationLinesPresenter>,
      IDeviationLinesPresenter
   {
      public DeviationLinesPresenter(IDeviationLinesView view) : base(view)
      {
      }

      public float GetFoldValue()
      {
         _view.Display();
         return _view.Canceled ? 0 : _view.GetFoldValue();
      }
   }
}