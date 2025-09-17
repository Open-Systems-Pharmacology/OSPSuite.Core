using System;
using System.Threading;
using OSPSuite.Core.Services;

namespace OSPSuite.Helpers
{
   public class HeavyWorkManagerForSpecs : IHeavyWorkManager
   {
      public event EventHandler<HeavyWorkEventArgs> HeavyWorkedFinished = delegate { };

      public bool Start(Action heavyWorkAction, CancellationTokenSource cts = default)
      {
         return Start(heavyWorkAction, string.Empty);
      }

      public virtual bool Start(Action heavyWorkAction, string caption, CancellationTokenSource cts = default)
      {
         heavyWorkAction();
         return true;
      }

      public void StartAsync(Action heavyWorkAction)
      {
         heavyWorkAction();
         HeavyWorkedFinished(this, new HeavyWorkEventArgs(true));
      }
   }

   public class HeavyWorkManagerFailingForSpecs : HeavyWorkManagerForSpecs
   {
      public override bool Start(Action heavyWorkAction, string caption, CancellationTokenSource cts = default)
      {
         base.Start(heavyWorkAction, caption);
         return false;
      }
   }
}