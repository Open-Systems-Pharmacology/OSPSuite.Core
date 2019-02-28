using System.Collections.Generic;

namespace OSPSuite.Core.Domain
{
   public class Category<TObject> : ObjectBase where TObject : CategoryItem
   {
      protected readonly IList<TObject> _allItems;
      private TObject _defaultItem;

      public Category()
      {
         _allItems = new List<TObject>();
      }

      public virtual IEnumerable<TObject> AllItems()
      {
         return _allItems;
      }

      public virtual TObject DefaultItem
      {
         get => _defaultItem ?? _allItems[0];
         set => _defaultItem = value;
      }

      public virtual void Add(TObject item)
      {
         _allItems.Add(item);
      }
   }
}