namespace OSPSuite.R.Extensions
{
   public static class ArrayExtensions
   {
      public static T[] ToNetArray<T>(this T[] array, T singleEntry) where T : class
      {
         return array ?? (singleEntry != null ? new[] {singleEntry} : null);
      }

      public static T[] ToNetArray<T>(this T[] array, T? singleEntry) where T : struct
      {
         return array ?? (singleEntry.HasValue ? new[] {singleEntry.Value} : null);
      }
   }
}