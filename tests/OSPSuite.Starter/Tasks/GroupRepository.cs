using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;

namespace OSPSuite.Starter.Tasks
{
   internal class GroupRepository : IGroupRepository
   {
      public IEnumerable<IGroup> All()
      {
         return Enumerable.Empty<IGroup>();
      }

      public IGroup GroupByName(string groupName)
      {
         return new Group{Name=groupName};
      }

      public IGroup GroupById(string groupId)
      {
         return new Group { Name = groupId };
      }

      public void Clear()
      {
         //
      }

      public void AddGroup(IGroup @group)
      {
         //
      }

   }
}