namespace OSPSuite.Core.Batch
{
   public class BatchValues
   {
      /// <summary>
      /// Unit in which the values are saved
      /// </summary>
      public string Unit { get; set; }

      /// <summary>
      /// Values saved in Unit
      /// </summary>
      public float[] Values { get; set; }

      /// <summary>
      /// Dimension in which the values are saved
      /// </summary>
      public string Dimension { get; set; }
   }

   public class BatchOutputValues: BatchValues
   {
      public string Path { get; set; }
      public double ComparisonThreshold { get; set; }
   }
}