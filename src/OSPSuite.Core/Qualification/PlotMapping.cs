namespace OSPSuite.Core.Qualification
{
   public class PlotMapping : IReferencingSimulation
   {
      public int SectionId { get; set; }
      public string Project { get; set; }
      public string Simulation { get; set; }
      public object Plot { get; set; }
   }
}