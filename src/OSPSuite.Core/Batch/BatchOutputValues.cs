namespace OSPSuite.Core.Batch
{
   public class BatchOutputValues
   {
      public string Path { get; set; }
      public float[] Values { get; set; }
      public string Dimension { get; set; }
      public double ComparisonThreshold { get; set; }
   }
}