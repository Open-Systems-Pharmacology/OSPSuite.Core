namespace OSPSuite.R
{
   public class SimulationRunOptions
   {
      /// <summary>
      ///    (Maximal) number of cores to be used (1 per default)
      /// </summary>
      public int NumberOfCoresToUse { get; set; }

      /// <summary>
      ///    Specifies whether negative values check is on or off. Default is <c>true</c>
      /// </summary>
      public bool CheckForNegativeValues { get; set; }
   }
}