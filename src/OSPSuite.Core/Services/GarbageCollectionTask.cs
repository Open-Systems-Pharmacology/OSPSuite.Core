using System;
using System.Runtime;

namespace OSPSuite.Core.Services
{
   public static class GarbageCollectionTask
   {
      /// <summary>
      ///    Force GC Collection compacting the Large Object Heap. Implementation very similar to what DotMemory
      ///    is doing according to one of their post
      /// </summary>
      public static void ForceGC()
      {
         GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
         for (int i = 0; i < 4; i++)
         {
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
         }
      }
   }
}