using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public class SimulationResults : IEnumerable<IndividualResults>
   {
      /// <summary>
      ///    This id is only used for optimal serialization and should not be changed
      /// </summary>
      [EditorBrowsable(EditorBrowsableState.Never)]
      public virtual int Id { get; set; }

      public virtual ISet<IndividualResults> AllIndividualResults { get; set; } = new HashSet<IndividualResults>();

      public virtual QuantityValues Time { get; set; }

      public virtual IndividualResults[] IndividualResultsAsArray() => AllIndividualResults?.ToArray();

      //We need to add the objects in a thread safe manner to the list that is intrinsically not thread safe
      private readonly object _locker = new object();

      public virtual void Add(IndividualResults individualResults)
      {
         individualResults.SimulationResults = this;
         lock (_locker)
         {
            AllIndividualResults.Add(individualResults);
         }
      }

      public virtual void AddRange(IEnumerable<IndividualResults> individualResultsSet)
      {
         lock (_locker)
         {
            individualResultsSet.Each(Add);
         }
      }

      /// <summary>
      ///    Returns whether values were calculated for the individual with id <paramref name="individualId" /> or not
      /// </summary>
      public virtual bool HasResultsFor(int individualId) => ResultsFor(individualId) != null;

      public virtual int Count
      {
         get
         {
            lock (_locker)
            {
               return AllIndividualResults.Count;
            }
         }
      }

      /// <summary>
      ///    Returns the results calculated for the individual with id <paramref name="individualId" /> or null it they do not
      ///    exist
      /// </summary>
      public virtual IndividualResults ResultsFor(int individualId)
      {
         lock (_locker)
         {
            return AllIndividualResults.FirstOrDefault(x => x.IndividualId == individualId);
         }
      }

      /// <summary>
      ///    Returns all values calculated for the quantity with path <paramref name="quantityPath" /> and ordered by individual
      ///    id.
      /// </summary>
      public virtual IReadOnlyList<QuantityValues> AllValuesFor(string quantityPath)
      {
         lock (_locker)
         {
            return AllIndividualResults.Select(x => x.ValuesFor(quantityPath)).ToArray();
         }
      }

      public virtual IEnumerator<IndividualResults> GetEnumerator()
      {
         return AllIndividualResults.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public virtual void Clear()
      {
         Time = null;
         AllIndividualResults.Clear();
      }

      public virtual IReadOnlyList<string> AllQuantityPaths()
      {
         lock (_locker)
         {
            return AllIndividualResults.Any() ? AllIndividualResults.First().Select(x => x.QuantityPath).ToArray() : Array.Empty<string>();
         }
      }

      public virtual IReadOnlyList<int> AllIndividualId()
      {
         lock (_locker)
         {
            return AllIndividualResults.Select(x => x.IndividualId).ToArray();
         }
      }

      protected internal virtual void ReorderByIndividualId()
      {
         lock (_locker)
         {
            AllIndividualResults = new HashSet<IndividualResults>(AllIndividualResults.OrderBy(x => x.IndividualId));
         }
      }
   }

   public class NullSimulationResults : SimulationResults
   {
   }
}