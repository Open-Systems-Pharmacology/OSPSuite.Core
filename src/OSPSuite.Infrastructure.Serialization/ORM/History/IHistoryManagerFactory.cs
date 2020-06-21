using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Infrastructure.Serialization.ORM.History
{
   public interface IHistoryManagerFactory
   {
      IHistoryManager Create();
   }
}