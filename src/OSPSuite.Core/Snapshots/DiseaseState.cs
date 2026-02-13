using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Snapshots;

public class DiseaseState : IWithName
{
   /// <summary>
   ///    Name of disease state associated with OriginData
   /// </summary>
   public string Name { get; set; }

   /// <summary>
   ///    List of disease state parameters associated with the selected disease state
   /// </summary>
   public Parameter[] Parameters { get; set; }
}