using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Infrastructure.ORM.History
{
   public interface IHistoryManagerFactory
   {
      IHistoryManager Create();
   }
}