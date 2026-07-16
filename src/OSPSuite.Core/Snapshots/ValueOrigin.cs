using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots
{
   public class ValueOrigin : IWithDescription
   {
      public int? Id { get; set; }
      public ValueOriginSourceId? Source { get; set; }
      public ValueOriginDeterminationMethodId? Method { get; set; }
      public string Description { get; set; }
   }
}