namespace OSPSuite.Core.Qualification
{
   public class QualificationMapping
   {
      public SimulationMapping[] SimulationMappings { get; set; }
      public ObservedDataMapping[] ObservedDataMappings { get; set; }
      public PlotMapping[] Plots { get; set; }
      public InputMapping[] Inputs { get; set; }
   }
}