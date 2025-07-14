using System.Collections.Concurrent;
using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
    public class RGroupRepository : IGroupRepository
    {
        private readonly ConcurrentDictionary<string, IGroup> _allGroups = new ConcurrentDictionary<string, IGroup>();

        public IEnumerable<IGroup> All() => _allGroups.Values;

        public IGroup GroupByName(string groupName) => getOrAdd(groupName);

        public IGroup GroupById(string groupId) => getOrAdd(groupId);

        private IGroup getOrAdd(string groupIdOrName)
        {
            return _allGroups.GetOrAdd(groupIdOrName, x => new Group { Id = groupIdOrName, Name = groupIdOrName });
        }
        public void Clear() => _allGroups.Clear();

        public void AddGroup(IGroup group) => getOrAdd(group.Id);
    }
}