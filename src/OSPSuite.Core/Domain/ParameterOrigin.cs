namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Represents the information defining a parameter in a simulation.
   ///    This information can be used in for instance commands to identify a parameter or find the orgin of a parameter
   ///    in a simulation (From which building block did the parameter come, with which type etc)
   /// </summary>
   public class ParameterOrigin
   {
      public ParameterOrigin()
      {
         BuilingBlockId = string.Empty;
         SimulationId = string.Empty;
         ParameterId = string.Empty;
      }

      /// <summary>
      ///    Id of building block in which the parameter was defined. (only for parameter defined in simulation, null otherwise)
      /// </summary>
      public string BuilingBlockId { get; set; }

      /// <summary>
      ///    Id of simulation for a parameter defined in simulation (necessary for lazy load from commmand)
      /// </summary>
      public string SimulationId { get; set; }

      /// <summary>
      ///    Id of parameter used to create the simulation parameter  (only for parameter defined in simulation, null otherwise)
      /// </summary>
      public string ParameterId { get; set; }

      public ParameterOrigin Clone()
      {
         return new ParameterOrigin {BuilingBlockId = BuilingBlockId, ParameterId = ParameterId, SimulationId = SimulationId};
      }
   }
}