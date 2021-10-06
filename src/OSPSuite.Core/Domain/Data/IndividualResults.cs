using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   /// <summary>
   ///    Represents the simulation results for a specific individual. The Individual is identified with the IndividualId
   /// </summary>
   public class IndividualResults : IEnumerable<QuantityValues>
   {
      /// <summary>
      ///    This id is only used for optimal serialization and should not be changed
      /// </summary>
      [EditorBrowsable(EditorBrowsableState.Never)]
      public virtual int Id { get; set; }

      /// <summary>
      ///    Id of the individual for which the results were calculated (0-based)
      /// </summary>
      public virtual int IndividualId { get; set; }

      /// <summary>
      ///    The time values for this individual
      /// </summary>
      public virtual QuantityValues Time { get; set; }

      /// <summary>
      ///    The parent simulation results. This is only used for optimal serialization
      /// </summary>
      [EditorBrowsable(EditorBrowsableState.Never)]
      public virtual SimulationResults SimulationResults { get; set; }

      public virtual ISet<QuantityValues> AllValues { get; set; }

      public virtual QuantityValues[] ValuesAsArray() => AllValues?.ToArray();


      public IndividualResults()
      {
         AllValues = new HashSet<QuantityValues>();
      }

      public virtual void Add(QuantityValues values)
      {
         AllValues.Add(values);
      }

      /// <summary>
      ///    Returns whether values were calculated for the quantity with path <paramref name="quantityPath" /> or not
      /// </summary>
      public virtual bool HasValuesFor(string quantityPath)
      {
         return QuantityValuesFor(quantityPath) != null;
      }

      /// <summary>
      ///    Returns the QuantityValues defined for the quantity with path <paramref name="quantityPath" /> or null it they do not exist
      /// </summary>
      public virtual QuantityValues QuantityValuesFor(string quantityPath)
      {
         return AllValues.FirstOrDefault(x => Equals(x.QuantityPath, quantityPath));
      }

      /// <summary>
      ///    Returns the values defined for the quantity with path <paramref name="quantityPath" /> or null it they do not exist
      /// </summary>
      public float[] ValuesFor(string quantityPath) => QuantityValuesFor(quantityPath)?.Values;

      public virtual IEnumerator<QuantityValues> GetEnumerator()
      {
         return AllValues.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      /// <summary>
      ///    This method should be called to synchronize time references between results and values.
      ///    This cannot be done in Time{set;} because of LazyLoading issues with NHibernate
      /// </summary>
      public virtual void UpdateQuantityTimeReference()
      {
         AllValues.Each(x => x.Time = Time);
      }
   }
}