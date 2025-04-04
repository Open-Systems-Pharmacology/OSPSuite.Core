using System.Threading;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IHeavyWorkCancellablePresenter : IHeavyWorkPresenter
   {
      void SetCancellationSource(CancellationTokenSource cts);
      void Cancel();
   }

   public class HeavyWorkCancellablePresenter : AbstractDisposablePresenter<IHeavyWorkCancellableView, IHeavyWorkCancellablePresenter>,
      IHeavyWorkCancellablePresenter
   {
      private CancellationTokenSource _cts;

      public HeavyWorkCancellablePresenter(IHeavyWorkCancellableView view)
         : base(view)
      {
         _view.AttachPresenter(this);
      }

      public void SetCancellationSource(CancellationTokenSource cts)
      {
         _cts = cts;
      }

      public void Start(string actionCaption)
      {
         _view.Caption = actionCaption;
         _view.Display();
      }

      public void Close()
      {
         _view.Close();
      }

      public void Cancel()
      {
         _cts?.Cancel();
      }
   }
}