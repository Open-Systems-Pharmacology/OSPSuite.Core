using System;

namespace OSPSuite.Presentation.Core
{
   public class DragDropInfo
   {
      private readonly WeakReference _reference;

      public DragDropInfo(object subject)
      {
         _reference = new WeakReference(subject);
      }

      public object Subject
      {
         get { return _reference.Target; }
      }
   }
}