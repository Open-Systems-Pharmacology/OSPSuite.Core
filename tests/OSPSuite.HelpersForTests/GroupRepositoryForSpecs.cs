using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.Helpers
{
   public class GroupRepositoryForSpecs : IGroupRepository
   {
      private readonly List<IGroup> _allGroups;

      public GroupRepositoryForSpecs()
      {
         _allGroups = new List<IGroup>();
      }

      public IEnumerable<IGroup> All()
      {
         return _allGroups;
      }

      public IGroup GroupByName(string groupName)
      {
         return _allGroups.FindByName(groupName) ?? _allGroups[0];
      }

      public IGroup GroupById(string groupId)
      {
         return _allGroups.FindById(groupId) ?? _allGroups[0];
      }

      public void Clear()
      {
         _allGroups.Clear();
      }

      public void AddGroup(IGroup group)
      {
         _allGroups.Add(group);
      }
   }
}