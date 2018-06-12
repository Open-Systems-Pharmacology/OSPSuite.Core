using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Comparison
{
   /// <summary>
   /// Object created when comparing two objects
   /// </summary>
   public class DiffReport : IReadOnlyList<DiffItem>
   {
      private readonly IList<DiffItem> _allItems;

      public DiffReport()
      {
         _allItems = new List<DiffItem>();
      }

      public void AddRange(IEnumerable<DiffItem>  diffItems)
      {
         diffItems.Each(Add);
      }

      public void Add(DiffItem diffItem)
      {
         var emptyItem = diffItem as EmptyDiffItem;
         if(emptyItem==null)
            _allItems.Add(diffItem);
      }

      public IEnumerable<DiffItem> All()
      {
         return _allItems;
      }

      public bool IsEmpty => _allItems.Count == 0;

      public IEnumerator<DiffItem> GetEnumerator()
      {
         return _allItems.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public int Count => _allItems.Count;

      public DiffItem this[int index] => _allItems[index];
   }
}