using OSPSuite.Presentation.Views;
using System.Threading;

namespace OSPSuite.Presentation.Presenters
{
   public interface IHeavyWorkPresenter : IDisposablePresenter
   {
      void Start(string actionCaption);
      void Close();
      void SetCancellationSource(CancellationTokenSource cts);
      void Cancel();
   }

   public class HeavyWorkPresenter : AbstractDisposablePresenter<IHeavyWorkView, IHeavyWorkPresenter>, IHeavyWorkPresenter
   {
      private CancellationTokenSource _cts;

      public HeavyWorkPresenter(IHeavyWorkView view)
         : base(view)
      {
         _view.AttachPresenter(this);
      }

      public void SetCancellationSource(CancellationTokenSource cts)
      {
         _cts = cts;
         _view.CancelVisible = cts != null;
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