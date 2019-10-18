using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class OutputSelections : IUpdatable, IEnumerable<QuantitySelection>
   {
      private readonly HashSet<QuantitySelection> _allOutputs;

      public OutputSelections()
      {
         _allOutputs = new HashSet<QuantitySelection>();
      }

      public virtual void AddOutput(QuantitySelection quantitySelection) => _allOutputs.Add(quantitySelection);

      public virtual void RemoveOutput(QuantitySelection quantitySelection) => _allOutputs.Remove(quantitySelection);

      /// <summary>
      ///    Adds an output corresponding to the <paramref name="quantity" />. The corresponding <see cref="QuantitySelection" />
      ///    will be added using a consolidated path
      /// </summary>
      public virtual void AddQuantity(IQuantity quantity)
      {
         if (quantity == null)
            return;

         _allOutputs.Add(new QuantitySelection(quantity.ConsolidatedPath(), quantity.QuantityType));
      }

      /// <summary>
      ///    Removes the <see cref="QuantitySelection" /> whose path is equal to the consolidated path of
      ///    <paramref name="quantity" />
      /// </summary>
      public virtual void RemoveQuantity(IQuantity quantity)
      {
         if (quantity == null)
            return;

         RemoveOutput(new QuantitySelection(quantity.ConsolidatedPath(), quantity.QuantityType));
      }

      public virtual IEnumerable<QuantitySelection> AllOutputs => _allOutputs;

      public virtual QuantitySelection[] OutputsAsArray => AllOutputs?.ToArray();

      public virtual void Clear() => _allOutputs.Clear();

      public virtual bool HasSelection => _allOutputs.Count > 0;

      public virtual void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceSelection = source as OutputSelections;
         if (sourceSelection == null) return;
         Clear();
         sourceSelection.AllOutputs.Each(x => AddOutput(x.Clone()));
      }

      public virtual OutputSelections Clone()
      {
         var clone = new OutputSelections();
         clone.UpdatePropertiesFrom(this, null);
         return clone;
      }

      public IEnumerator<QuantitySelection> GetEnumerator()
      {
         return _allOutputs.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}