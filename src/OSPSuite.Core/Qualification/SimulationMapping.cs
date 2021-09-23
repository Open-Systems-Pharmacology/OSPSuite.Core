namespace OSPSuite.Core.Qualification
{
   public class SimulationMapping : IReferencingSimulation
   {
      /// <summary>
      /// Name of project
      /// </summary>
      public string Project { get; set; }

      /// <summary>
      /// Name of simulation
      /// </summary>
      public string Simulation { get; set; }

      /// <summary>
      /// Path where simulation artifacts will be exported
      /// </summary>
      public string Path { get; set; }

      /// <summary>
      /// Name of simulation file. It is not always the name of the simulation as the simulation file is stripped from all invalid characters
      /// </summary>
      public string SimulationFile { get; set; }

   }
}