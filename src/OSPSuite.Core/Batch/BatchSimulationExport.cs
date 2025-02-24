using System.Collections.Generic;

namespace OSPSuite.Core.Batch
{
   public class BatchSimulationExport
   {
      public string Name { get; set; }

      /// <summary>
      /// Time Array used in the simulation
      /// </summary>
      public BatchValues Times { get; set; }

      /// <summary>
      /// Absolute tolerance used in the simulation
      /// </summary>
      public double AbsTol { get; set; }

      /// <summary>
      /// Relative tolerance used in the simulation
      /// </summary>
      public double RelTol { get; set; }

      /// <summary>
      ///   Specifies whether negative values check is on or off
      /// </summary>
      public bool CheckForNegativeValues { get; set; }

      /// <summary>
      /// Auto Reduce Tolerance
      /// </summary>
      public bool AutoReduceTolerances { get; set; }

      public List<BatchOutputValues> OutputValues { get; set; } = new List<BatchOutputValues>();
      public List<ParameterValue> ParameterValues { get; set; } = new List<ParameterValue>();
   }
}