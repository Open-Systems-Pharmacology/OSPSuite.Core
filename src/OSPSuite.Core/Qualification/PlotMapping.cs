namespace OSPSuite.Core.Qualification
{
   public class PlotMapping : IReferencingSimulation, IWithSectionReference
   {
      public string Project { get; set; }

      public string Simulation { get; set; }

      public int? SectionId { get; set; }

      public string SectionReference { get; set; }

      //put last to ensure that other properties are generated first
      public object Plot { get; set; }
   }
}