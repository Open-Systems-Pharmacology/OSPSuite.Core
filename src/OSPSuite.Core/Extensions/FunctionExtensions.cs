using System;

namespace OSPSuite.Core.Extensions
{
   public static class FunctionExtensions
   {
      public static T Compose<T>(this Func<T, T> f1, Func<T, T> f2, T value)
      {
         return f2.Invoke(f1.Invoke(value));
      }
   }
}
