using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

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

      public void UpdateSensitivityParameterName(string oldParameterName, string newParameterName)
      {
         var allParametersToRename = _allPKParameterSensitivities.Where(x => string.Equals(x.ParameterName, oldParameterName)).ToList();
         allParametersToRename.Each(x => x.ParameterName = newParameterName);
      }

      public bool HasPKParameterSensitivityWithId(string id) => _allPKParameterSensitivities.Exists(x => string.Equals(x.Id, id));

      public IReadOnlyList<PKParameterSensitivity> AllPKParameterSensitivitiesFor(string pkParameterName, string outputPath, double totalSensitivityThreshold)
      {
         var allPossiblePKParameterSensitivities = allPKParametersForSelectionWithDefinedSensitivity(pkParameterName, outputPath).OrderByDescending(x => Math.Abs(x.Value)).ToList();
         return sensitivitiesUpToTotalSensitivity(allPossiblePKParameterSensitivities, totalSensitivityThreshold).ToArray();
      }

      private IEnumerable<PKParameterSensitivity> allPKParametersForSelectionWithDefinedSensitivity(string pkParameterName, string outputPath)
      {
         return AllFor(pkParameterName, outputPath).Where(x => !double.IsNaN(x.Value));
      }

      private static IEnumerable<PKParameterSensitivity> sensitivitiesUpToTotalSensitivity(IReadOnlyList<PKParameterSensitivity> orderedSensitivities, double totalSensitivityThreshold)
      {
         var totalSensitivity = orderedSensitivities.Sum(x => Math.Abs(x.Value));
         var runningSensitivity = 0.0;
         return orderedSensitivities.TakeWhile(x =>
         {
            if (runningSensitivity / totalSensitivity >= totalSensitivityThreshold)
               return false;

            runningSensitivity += Math.Abs(x.Value);
            return true;
         });
      }

      public virtual int Count => _allPKParameterSensitivities.Count;
   }
}