using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.R.MinimalImplementations
{
   public class GroupRepository : IGroupRepository
   {
      private readonly List<IGroup> _allGroups = new List<IGroup>();

      public IEnumerable<IGroup> All() => _allGroups;

      public IGroup GroupByName(string groupName)
      {
         return _allGroups.FindByName(groupName) ?? _allGroups[0];
      }

      public IGroup GroupById(string groupId)
      {
         return _allGroups.FindById(groupId) ?? _allGroups[0];
      }

      public void Clear() => _allGroups.Clear();

      public void AddGroup(IGroup group) => _allGroups.Add(@group);
   }
}