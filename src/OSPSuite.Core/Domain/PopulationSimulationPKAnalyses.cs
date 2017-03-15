using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public class PopulationSimulationPKAnalyses
   {
      private readonly ICache<string, QuantityPKParameter> _pkAnalyses;

      public PopulationSimulationPKAnalyses()
      {
         _pkAnalyses = new Cache<string, QuantityPKParameter>(x => x.Id, x => null);
      }

      /// <summary>
      ///    Returns the <see cref="QuantityPKParameter" /> whose id is <paramref name="id" />
      ///    If the pk parameter is not found for the given id, returns null
      /// </summary>
      public virtual QuantityPKParameter PKParameterBy(string id)
      {
         return _pkAnalyses[id];
      }

      /// <summary>
      ///    Returns the <see cref="QuantityPKParameter" /> named <paramref name="pkParameter" /> that was calculated
      ///    for the quantity with path <paramref name="quantityPath" />.
      ///    If the pk parameter is not found for the given combination, returns null
      /// </summary>
      public virtual QuantityPKParameter PKParameterFor(string quantityPath, string pkParameter)
      {
         return PKParameterBy(QuantityPKParameter.CreateId(quantityPath, pkParameter));
      }

      /// <summary>
      /// Returns true if a <see cref="QuantityPKParameter" /> named <paramref name="pkParameter" />  was calculated
      /// for the quantity with path <paramref name="quantityPath" /> otherwise false.
      /// </summary>
      public virtual bool HasPKParameterFor(string quantityPath, string pkParameter)
      {
         return PKParameterFor(quantityPath, pkParameter) != null;
      }

      /// <summary>
      ///    Returns all pk parameters defined for the given <paramref name="quantityPath" />
      /// </summary>
      public virtual IReadOnlyList<QuantityPKParameter> AllPKParametersFor(string quantityPath)
      {
         return _pkAnalyses.Where(x => string.Equals(x.QuantityPath, quantityPath)).ToList();
      }

      /// <summary>
      ///    Returns all distinct pk parameter names defined for the given <paramref name="quantityPath" />
      /// </summary>
      /// <returns></returns>
      public virtual IReadOnlyList<string> AllPKParameterNamesFor(string quantityPath)
      {
         return AllPKParametersFor(quantityPath).Select(x => x.Name).ToList();
      }

      public virtual IEnumerable<QuantityPKParameter> All()
      {
         return _pkAnalyses;
      }

      public virtual void AddPKAnalysis(QuantityPKParameter quantityPKParameter)
      {
         _pkAnalyses[quantityPKParameter.Id] = quantityPKParameter;
      }
   }

   public class NullPopulationSimulationPKAnalyses : PopulationSimulationPKAnalyses
   {
   }
}