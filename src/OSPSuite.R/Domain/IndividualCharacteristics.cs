using System.Collections.Generic;
using OSPSuite.Core.Snapshots;

namespace OSPSuite.R.Domain;

/// <summary>
///    Wrapper object for .net that encapsulates origin data and disease state
/// </summary>
public class IndividualCharacteristics : OriginData
{
   public void AddDiseaseStateParameter(Parameter diseaseStateParameter)
   {
      DiseaseStateParameters = DiseaseStateParameters == null ? [diseaseStateParameter] : new List<Parameter>(DiseaseStateParameters) { diseaseStateParameter }.ToArray();
   }

   public int? Seed { get; set; }
}