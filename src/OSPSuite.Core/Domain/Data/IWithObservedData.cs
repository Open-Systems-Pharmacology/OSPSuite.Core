using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Data
{
   public interface IWithObservedData
   {
      void AddObservedData(DataRepository dataRepository);
      IEnumerable<DataRepository> AllObservedData();
      bool UsesObservedData(DataRepository dataRepository);
      void RemoveObservedData(DataRepository dataRepository);
   }
}