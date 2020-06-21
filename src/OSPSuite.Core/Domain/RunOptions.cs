namespace OSPSuite.Core.Domain
{
   public class RunOptions
   {
      /// <summary>
      ///    (Maximal) number of cores to be used (1 per default)
      /// </summary>
      public int NumberOfCoresToUse { get; set; } = 1;
   }
}