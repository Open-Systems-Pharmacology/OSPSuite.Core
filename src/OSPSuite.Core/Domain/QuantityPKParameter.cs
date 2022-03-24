using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public class QuantityPKParameter : IWithDimension, IWithName
   {
      /// <summary>
      ///    The values. One item for each individual
      /// </summary>
      public virtual float[] ValuesAsArray => ValueCache.ToArray();

      /// <summary>
      ///    The values cache. The key will be the ind of the individual
      /// </summary>
      public virtual Cache<int, float> ValueCache { get; } = new Cache<int, float>(onMissingKey: x => float.NaN);

      /// <summary>
      ///    Path of underlying quantity for which pk-analyses were performed
      /// </summary>
      public virtual string QuantityPath { get; set; }

      /// <summary>
      ///    Dimension of pk parameter
      /// </summary>
      public virtual IDimension Dimension { get; set; }

      /// <summary>
      ///    Name of PK Output (Cmax, Tmax etc...)
      /// </summary>
      public virtual string Name { get; set; }

      public override string ToString() => Id;

      /// <summary>
      ///    Set the pkValue for the individual with id <paramref name="individualId" />
      /// </summary>
      public virtual void SetValue(int individualId, float pkValue)
      {
         ValueCache[individualId] = pkValue;
      }

      /// <summary>
      ///    Id representing the PK parameter uniquely in the simulation hierarchy
      /// </summary>
      public virtual string Id => CreateId(QuantityPath, Name);

      public virtual int Count => ValueCache.Count;

      /// <summary>
      ///    Returns the PK-Parameter value defined for individual with id <paramref name="individualId" /> or NaN otherwise
      /// </summary>
      public virtual float ValueFor(int individualId) => ValueCache[individualId];

      public static string CreateId(string quantityPath, string pkParameterName)
      {
         return new[] {quantityPath, pkParameterName}.ToPathString();
      }
   }
}