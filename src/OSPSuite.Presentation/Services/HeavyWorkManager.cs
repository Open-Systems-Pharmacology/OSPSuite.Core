using System;
using System.ComponentModel;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Services
{
   public class HeavyWorkEventArgs : EventArgs
   {
      public bool Success { get; private set; }

      public HeavyWorkEventArgs(bool success)
      {
         Success = success;
      }
   }
   public interface IHeavyWorkManager
   {
      /// <summary>
      /// run the heavy work action. Returns true if the action ran successfuly, and false, if an exception was thrown or the action was canceled
      /// </summary>
      /// <param name="heavyWorkAction">Action that need to be performed</param>
      bool Start(Action heavyWorkAction);

      /// <summary>
      /// run the heavy work action. Returns true if the action ran successfuly, and false, if an exception was thrown or the action was canceled
      /// </summary>
      /// <param name="heavyWorkAction">Action that need to be performed</param>
      /// <param name="caption">Caption that will be displayed</param>
      bool Start(Action heavyWorkAction, string caption);

      /// <summary>
      /// run the heavy work action and returns to the called immediatly
      /// Event Heavy Action finised will be raised when the action is terminated (either canceled , exception or finished)
      /// </summary>
      /// <param name="heavyWorkAction">Action that need to be performed</param>
      void StartAsync(Action heavyWorkAction);

      /// <summary>
      /// event raised whenever the heavy work aktion is finished
      /// </summary>
      event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished;
   }

   public class HeavyWorkManager : IHeavyWorkManager
   {
      private readonly IHeavyWorkPresenterFactory _heavyWorkPresenterFactory;
      private readonly IExceptionManager _exceptionManager;
      private readonly BackgroundWorker _backgrounWorker;
      private Action _action;
      private bool _success;
      private IHeavyWorkPresenter _presenter;
      public event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished = delegate { };

      public HeavyWorkManager(IHeavyWorkPresenterFactory heavyWorkPresenterFactory, IExceptionManager exceptionManager)
      {
         _heavyWorkPresenterFactory = heavyWorkPresenterFactory;
         _exceptionManager = exceptionManager;
         _backgrounWorker = new BackgroundWorker();
         _backgrounWorker.RunWorkerCompleted += (o, e) => actionCompleted(e);
         _backgrounWorker.DoWork += (o, e) => doWork();
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
         if (_backgrounWorker.IsBusy)
            throw new OSPSuiteException(Captions.ActionAlreadyInProgress);

         _action = heavyWorkAction;
         _backgrounWorker.RunWorkerAsync();
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