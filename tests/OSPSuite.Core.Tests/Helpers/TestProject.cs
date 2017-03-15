using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Helpers
{
   public class TestProject : Project
   {
      public override bool HasChanged { get; set; }

      public override IReadOnlyCollection<T> All<T>()
      {
         return new List<T>();
      }

      public override IEnumerable<IUsesObservedData> AllUsersOfObservedData { get; } = new List<IUsesObservedData>();
   }
}