using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Helpers
{
   public class TestProject : Project
   {
      private Cache<string, IWithName> _allCache = new Cache<string, IWithName>();
      public override bool HasChanged { get; set; }

      public override IReadOnlyCollection<T> All<T>()
      {
         return _allCache.OfType<T>().ToList();
      }

      public void Add<T>(T itemToAdd) where T : IWithName
      {
         _allCache[itemToAdd.Name] = itemToAdd;
      }

      public override IEnumerable<IUsesObservedData> AllUsersOfObservedData { get; } = new List<IUsesObservedData>();
   }
}