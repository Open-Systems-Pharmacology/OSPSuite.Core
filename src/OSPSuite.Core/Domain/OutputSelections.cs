using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public class OutputSelections : IUpdatable, IEnumerable<QuantitySelection>
   {
      private readonly HashSet<QuantitySelection> _allOutputs;

      public OutputSelections()
      {
         _allOutputs = new HashSet<QuantitySelection>();
      }

      public virtual void AddOutput(QuantitySelection quantitySelection)
      {
         _allOutputs.Add(quantitySelection);
      }

      public virtual void RemoveOutput(QuantitySelection quantitySelection)
      {
         _allOutputs.Remove(quantitySelection);
      }

      public virtual IEnumerable<QuantitySelection> AllOutputs => _allOutputs;

      public virtual void Clear()
      {
         _allOutputs.Clear();
      }

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