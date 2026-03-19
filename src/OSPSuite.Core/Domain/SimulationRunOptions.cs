using OSPSuite.Core.Serialization.SimModel.Services;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Options used for simulation run
   /// </summary>
   public class SimulationRunOptions
   {
      /// <summary>
      ///    Mode used to create the model for the SimModel kernel. Default is Full
      /// </summary>
      public SimModelExportMode SimModelExportMode { get; set; }

      /// <summary>
      ///    Specifies whether the solver should automatically reduce tolerances when a simulation run fails. Default is <c>true</c>
      /// </summary>
      public bool AutoReduceTolerances { get; set; }

      public SimulationRunOptions()
      {
         SimModelExportMode = SimModelExportMode.Full;
         AutoReduceTolerances = true;
      }
   }
}
