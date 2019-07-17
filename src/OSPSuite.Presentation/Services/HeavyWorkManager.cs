using System;
using System.ComponentModel;
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
      public event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished = delegate { };

      public HeavyWorkManager(IHeavyWorkPresenterFactory heavyWorkPresenterFactory, IExceptionManager exceptionManager)
      {
         _heavyWorkPresenterFactory = heavyWorkPresenterFactory;
         _exceptionManager = exceptionManager;
         _backgroundWorker = new BackgroundWorker();
         _backgroundWorker.RunWorkerCompleted += (o, e) => actionCompleted(e);
         _backgroundWorker.DoWork += (o, e) => doWork();
      }

      public bool Start(Action heavyWorkAction)
      {
         return Start(heavyWorkAction, Captions.PleaseWait);
      }

      public bool Start(Action heavyWorkAction, string caption)
      {
         _success = true;

         using (var presenter = _heavyWorkPresenterFactory.Create())
         {
            _presenter = presenter;
            StartAsync(heavyWorkAction);

            //action will block the current thread
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

      private void doWork()
      {
         _action();
      }

      private void actionCompleted(RunWorkerCompletedEventArgs runWorkerCompletedEventArgs)
      {
         //close the presenter when the action is completed
         if (_presenter != null)
            _presenter.Close();

         _action = null;
         _presenter = null;
         if (runWorkerCompletedEventArgs.Error != null)
         {
            _success = false;
            _exceptionManager.LogException(runWorkerCompletedEventArgs.Error);
         }
         else
            _success = !runWorkerCompletedEventArgs.Cancelled;

         HeavyWorkedFinished(this, new HeavyWorkEventArgs(_success));
      }
   }
}