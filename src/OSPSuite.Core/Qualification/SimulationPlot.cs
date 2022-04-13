namespace OSPSuite.Core.Qualification
{
   public class SimulationPlot : IReferencingSimulation, IWithSectionReference
   {
      public string Simulation { get; set; }
      public string Project { get; set; }
      public int? SectionId { get; set; }
      public string SectionReference { get; set; }
   }
}