using System;

namespace OSPSuite.Core.Extensions
{
   public static class IntegerExtensions
   {
      /// <summary>
      /// Rounds the <paramref name="numberToRoundUp"/> to the next multiple of multiple. Rounds positive and negative numbers up.
      /// <code>15.RoundUpToMultiple(16) = 16</code>
      /// </summary>
      public static int RoundUpToMultiple(this int numberToRoundUp, int multiple)
      {
         if (multiple == 0)
            return numberToRoundUp;

         var remainder = Math.Abs(numberToRoundUp) % multiple;
         if (remainder == 0)
            return numberToRoundUp;
         if (numberToRoundUp < 0)
            return -(Math.Abs(numberToRoundUp) - remainder);
         return numberToRoundUp + multiple - remainder;
      }
   }
}
