using OSPSuite.Core.Domain;

namespace OSPSuite.R.Domain
{
   public class SimulationRunOptions : RunOptions
   {
      /// <summary>
      ///    Specifies whether the solver should automatically reduce tolerances when a simulation run fails. Default is <c>true</c>
      /// </summary>
      public bool AutoReduceTolerances { get; set; } = true;

      /// <summary>
      ///    Specifies whether progress bar should be shown during simulation run. Default is <c>true</c>
      /// </summary>
      public bool ShowProgress { get; set; } = true;
   }
}