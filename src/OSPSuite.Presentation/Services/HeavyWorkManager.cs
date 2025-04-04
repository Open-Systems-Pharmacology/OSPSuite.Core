using System;
using System.ComponentModel;
using System.Threading;
using OSPSuite.Assets;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Presentation.Services
{
   public class HeavyWorkManager : IHeavyWorkManager
   {
      private readonly IHeavyWorkPresenterFactory _heavyWorkPresenterFactory;
      private readonly IExceptionManager _exceptionManager;
      private readonly BackgroundWorker _backgroundWorker;
      private Action _action;
      private bool _success;
      private IHeavyWorkPresenter _presenter;
      private CancellationTokenSource _cts;

      public event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished = delegate { };

      public HeavyWorkManager(IHeavyWorkPresenterFactory heavyWorkPresenterFactory, IExceptionManager exceptionManager)
      {
         _heavyWorkPresenterFactory = heavyWorkPresenterFactory;
         _exceptionManager = exceptionManager;

         _backgroundWorker = new BackgroundWorker { WorkerSupportsCancellation = true };
         _backgroundWorker.DoWork += (o, e) => doWork();
         _backgroundWorker.RunWorkerCompleted += (o, e) => actionCompleted(e);
      }

      public bool Start(Action heavyWorkAction, CancellationTokenSource cts = null)
      {
         return Start(heavyWorkAction, Captions.PleaseWait, cts);
      }

      public bool Start(Action heavyWorkAction, string caption, CancellationTokenSource cts = null)
      {
         _success = true;
         _cts = cts ?? new CancellationTokenSource();
          
         var supportsCancellation = cts != null;
         using (var presenter = _heavyWorkPresenterFactory.Create(supportsCancellation))
         {
            _presenter = presenter;

            if (supportsCancellation && presenter is IHeavyWorkCancellablePresenter cancelablePresenter)
               cancelablePresenter.SetCancellationSource(_cts);

            StartAsync(heavyWorkAction);

            // Blocks while the modal dialog is shown
            presenter.Start(caption);

            return _success;
         }
      }

      public void StartAsync(Action heavyWorkAction)
      {
         if (_backgroundWorker.IsBusy)
            throw new OSPSuiteException(Captions.ActionAlreadyInProgress);

         _action = heavyWorkAction;
         _backgroundWorker.RunWorkerAsync();
      }

      public void Cancel()
      {
         if (_backgroundWorker.IsBusy)
         {
            _cts?.Cancel();
            _backgroundWorker.CancelAsync();
         }
      }

      private void doWork()
      {
         _action?.Invoke();
      }

      private void actionCompleted(RunWorkerCompletedEventArgs e)
      {
         _presenter?.Close();
         _action = null;
         _presenter = null;

         if (e.Error != null)
         {
            _success = false;
            _exceptionManager.LogException(e.Error);
         }
         else
         {
            _success = !e.Cancelled;
         }

         HeavyWorkedFinished(this, new HeavyWorkEventArgs(_success));
      }
   }
}