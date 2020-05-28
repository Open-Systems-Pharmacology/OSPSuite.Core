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
         return _allPKParameterSensitivities.Where(
            x => string.Equals(x.QuantityPath, outputPath) && string.Equals(x.PKParameterName, pkParameterName));
      }

      public PKParameterSensitivity PKParameterSensitivityFor(string pkParameterName, string outputPath, string parameterName)
      {
         return AllFor(pkParameterName, outputPath).Find(x => string.Equals(parameterName, x.ParameterName));
      }

      public double PKParameterSensitivityValueFor(string pkParameterName, string outputPath, string parameterName)
      {
         return PKParameterSensitivityFor(pkParameterName, outputPath, parameterName)?.Value ?? double.NaN;
      }

      public void UpdateSensitivityParameterName(string oldParameterName, string newParameterName)
      {
         var allParametersToRename = _allPKParameterSensitivities.Where(x => string.Equals(x.ParameterName, oldParameterName)).ToList();
         allParametersToRename.Each(x => x.ParameterName = newParameterName);
      }

      public bool HasPKParameterSensitivityWithId(string id) => _allPKParameterSensitivities.Exists(x => string.Equals(x.Id, id));

      public IReadOnlyList<PKParameterSensitivity> AllPKParameterSensitivitiesFor(string pkParameterName, string outputPath,
         double totalSensitivityThreshold)
      {
         var allPossiblePKParameterSensitivities = allPKParametersForSelectionWithDefinedSensitivity(pkParameterName, outputPath)
            .OrderByDescending(x => Math.Abs(x.Value)).ToList();
         return sensitivitiesUpToTotalSensitivity(allPossiblePKParameterSensitivities, totalSensitivityThreshold).ToArray();
      }

      private IEnumerable<PKParameterSensitivity> allPKParametersForSelectionWithDefinedSensitivity(string pkParameterName, string outputPath)
      {
         return AllFor(pkParameterName, outputPath).Where(x => !double.IsNaN(x.Value));
      }

      private static IEnumerable<PKParameterSensitivity> sensitivitiesUpToTotalSensitivity(IReadOnlyList<PKParameterSensitivity> orderedSensitivities,
         double totalSensitivityThreshold)
      {
         var totalSensitivity = orderedSensitivities.Sum(x => Math.Abs(x.Value));

         //We want to return all
         if (totalSensitivityThreshold == 1)
            return orderedSensitivities;

         var runningSensitivity = 0.0;
         return orderedSensitivities.TakeWhile(x =>
         {
            if (runningSensitivity / totalSensitivity >= totalSensitivityThreshold)
               return false;

            runningSensitivity += Math.Abs(x.Value);
            return true;
         });
      }

      public string[] AllPKParameterNames => _allPKParameterSensitivities.Select(x => x.PKParameterName).Distinct().ToArray();

      public string[] AllQuantityPaths => _allPKParameterSensitivities.Select(x => x.QuantityPath).Distinct().ToArray();

      public virtual int Count => _allPKParameterSensitivities.Count;
   }
}