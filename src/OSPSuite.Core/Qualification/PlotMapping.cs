namespace OSPSuite.Core.Qualification
{
   public class PlotMapping : IReferencingSimulation, IWithSectionReference
   {
      public string Project { get; set; }
      public string Simulation { get; set; }
      public object Plot { get; set; }
      public int? SectionId { get; set; }
      public string SectionReference { get; set; }
   }
}