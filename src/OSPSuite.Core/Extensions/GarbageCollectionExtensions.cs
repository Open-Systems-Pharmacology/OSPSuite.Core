using System;
using System.Runtime;

namespace OSPSuite.Core.Extensions
{
   public static class GarbageCollectionExtensions
   {
      /// <summary>
      /// Forces and immediate garbage collection compacting the Large Object Heap
      /// </summary>
      /// <param name="caller"></param>
      public static void GCCollectAndCompact(this object caller)
      {
         GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
         GC.Collect();
      }
   }
}