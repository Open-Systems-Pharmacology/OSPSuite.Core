using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Builder
{
   public class OriginDataItems
   {
      private readonly Cache<string, OriginDataItem> _cache = new Cache<string, OriginDataItem>(getKey: x => x.Name);

      public IReadOnlyList<OriginDataItem> AllDataItems => _cache.ToList();

      public ValueOrigin ValueOrigin { set; get; } = new ValueOrigin();

      public void AddOriginDataItem(OriginDataItem originDataItem)
      {
         _cache[originDataItem.Name]= originDataItem;
      }
   }
}