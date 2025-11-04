using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots
{
   public class ParameterIdentification : SnapshotBase
   {
      public string[] Simulations { get; set; }
      public ParameterIdentificationConfiguration Configuration { get; set; }
      public OutputMapping[] OutputMappings { get; set; }
      public IdentificationParameter[] IdentificationParameters { get; set; }
      public ParameterIdentificationAnalysis[] Analyses { get; set; }
   }

   public class IdentificationParameter : ParameterContainerSnapshotBase
   {
      public Scalings Scaling { get; set; }
      public bool? UseAsFactor { get; set; }
      public bool? IsFixed { get; set; }
      public string[] LinkedParameters { get; set; }
   }

   public abstract class ParameterContainerSnapshotBase : SnapshotBase
   {
      public Parameter[] Parameters { get; set; }
   }
}