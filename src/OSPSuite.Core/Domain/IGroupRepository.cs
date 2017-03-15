using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public interface IGroupRepository : IRepository<IGroup>
   {
      IGroup GroupByName(string groupName);
      IGroup GroupById(string groupId);
      void Clear();
      void AddGroup(IGroup group);
   }
}