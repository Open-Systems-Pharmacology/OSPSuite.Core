using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace OSPSuite.Core.Services
{
   public class StartableProcess : IDisposable
   {
      private readonly Process _process;
      private bool _exited;
      public ProcessStartInfo StartInfo { get; }

      public StartableProcess(string applicationPath, params string[] arguments)
      {
         StartInfo = new ProcessStartInfo
         {
            FileName = applicationPath,
            Arguments = string.Join(" ", arguments),
         };

         _process = new Process
         {
            StartInfo = StartInfo
         };
      }

      /// <summary>
      /// <returns>true</returns> if the process exited normally. If the process was stopped, returns false
      /// </summary>
      public bool ExitedNormally => _exited;

      /// <summary>
      /// <returns>the exit code of the process</returns> if <see cref="ExitedNormally"/> returns true.
      /// throws an <seealso cref="InvalidOperationException"/> if the process has not exited or was aborted abnormally
      /// </summary>
      public int ReturnCode => _process.ExitCode;

      public virtual void Start()
      {
         _process?.Start();
      }

      public void Dispose()
      {
         _process.Dispose();
      }

      public virtual void Wait(CancellationToken token)
      {
         // we are passing ownsHandle = false because the started process owns the handle
         // not this process.
         using (var processSafeWaitHandle = new SafeWaitHandle(_process.Handle, false))
         using (var processFinishedWaitHandle = new ManualResetEvent(false))
         {
            processFinishedWaitHandle.SafeWaitHandle = processSafeWaitHandle;

            var finishingWaitHandle = waitForSignal(token.WaitHandle, processFinishedWaitHandle);

            if (finishingWaitHandle == processFinishedWaitHandle)
            {
               _exited = true;
               return;
            }
            _process.Kill();
            token.ThrowIfCancellationRequested();
         }
      }

      private static WaitHandle waitForSignal(WaitHandle token, WaitHandle processFinishedEvent)
      {
         var waitHandles = new[] { processFinishedEvent, token };

         var index = WaitHandle.WaitAny(waitHandles);

         return waitHandles[index];
      }
   }
}
