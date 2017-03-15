using OSPSuite.Core.Serialization.SimModel.Services;

namespace OSPSuite.Core.Domain
{
   
   /// <summary>
   /// Options used for simulation run
   /// </summary>
   public class SimulationRunOptions 
   {
      /// <summary>
      /// Mode used to create the model for the SimModel kernel. Default is Full
      /// </summary>
      public SimModelExportMode SimModelExportMode { get; set; }

      /// <summary>
      /// Specifies whether negative values check is on or off. Default is <c>true</c>
      /// </summary>
      public bool CheckForNegativeValues { get; set; }

      public SimulationRunOptions()
      {
         SimModelExportMode = SimModelExportMode.Full;
         CheckForNegativeValues = true;
      }

   }
}
