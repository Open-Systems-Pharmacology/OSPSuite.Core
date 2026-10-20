using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.R.Domain
{
   /// <summary>
   ///    Outputs and PK parameters to keep when converting PK-Analyses.
   ///    All outputs (or all PK parameters) are kept until quantity paths (or PK parameter names) are added
   /// </summary>
   public class PKAnalysesSelection
   {
      //null until quantity paths are added, so that all outputs are kept
      private HashSet<string> _quantityPaths;

      //null until PK parameter names are added, so that all PK parameters are kept
      private HashSet<string> _pkParameterNames;

      /// <summary>
      ///    Keeps only the outputs whose paths were added. Adding no path keeps no output
      /// </summary>
      public void AddQuantityPaths(params string[] quantityPaths) => _quantityPaths = addTo(_quantityPaths, quantityPaths);

      /// <summary>
      ///    Keeps only the PK parameters whose names were added. Adding no name keeps no PK parameter
      /// </summary>
      public void AddPKParameterNames(params string[] pkParameterNames) => _pkParameterNames = addTo(_pkParameterNames, pkParameterNames);

      public bool Keeps(QuantityPKParameter quantityPKParameter) =>
         keeps(_quantityPaths, quantityPKParameter.QuantityPath) && keeps(_pkParameterNames, quantityPKParameter.Name);

      private static HashSet<string> addTo(HashSet<string> entries, string[] entriesToAdd)
      {
         var allEntries = entries ?? new HashSet<string>();
         allEntries.UnionWith(entriesToAdd);
         return allEntries;
      }

      private static bool keeps(HashSet<string> entries, string entry) => entries == null || entries.Contains(entry);
   }
}
