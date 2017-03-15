using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class SensitivityAnalysisRunResult
   {
      private readonly List<PKParameterSensitivity> _allPKParameterSensitivities = new List<PKParameterSensitivity>();

      public virtual IReadOnlyList<PKParameterSensitivity> AllPKParameterSensitivities => _allPKParameterSensitivities;

      public virtual void AddPKParameterSensitivity(PKParameterSensitivity parameterSensitivity)
      {
         _allPKParameterSensitivities.Add(parameterSensitivity);
      }

      public IEnumerable<PKParameterSensitivity> AllFor(string pkParameterName, string outputPath)
      {
         return _allPKParameterSensitivities.Where(x => string.Equals(x.QuantityPath, outputPath) && string.Equals(x.PKParameterName, pkParameterName));
      } 
   }
}