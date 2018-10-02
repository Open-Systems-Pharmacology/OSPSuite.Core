using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Domain.Services
{
   public interface IHistoryManagerRetriever
   {
      IHistoryManager Current { get; }
   }
}