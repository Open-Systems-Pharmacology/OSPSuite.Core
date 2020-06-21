using System;

namespace OSPSuite.Core.Services
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
      ///    run the heavy work action. Returns true if the action ran successfully, and false, if an exception was thrown or the
      ///    action was canceled
      /// </summary>
      /// <param name="heavyWorkAction">Action that need to be performed</param>
      bool Start(Action heavyWorkAction);

      /// <summary>
      ///    run the heavy work action. Returns true if the action ran successfully, and false, if an exception was thrown or the
      ///    action was canceled
      /// </summary>
      /// <param name="heavyWorkAction">Action that need to be performed</param>
      /// <param name="caption">Caption that will be displayed</param>
      bool Start(Action heavyWorkAction, string caption);

      /// <summary>
      ///    run the heavy work action and returns to the called immediately
      ///    Event Heavy Action finished will be raised when the action is terminated (either canceled , exception or finished)
      /// </summary>
      /// <param name="heavyWorkAction">Action that need to be performed</param>
      void StartAsync(Action heavyWorkAction);

      /// <summary>
      ///    event raised whenever the heavy work action is finished
      /// </summary>
      event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished;
   }
}