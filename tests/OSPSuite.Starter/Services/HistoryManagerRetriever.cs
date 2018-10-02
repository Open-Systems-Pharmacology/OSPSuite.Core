using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Services
{
   public class HistoryManagerRetriever : IHistoryManagerRetriever
   {
      private IHistoryManager _current;

      public IHistoryManager Current
      {
         get
         {
            if (_current == null)
               _current = IoC.Resolve<IHistoryManager>();

            return _current;
         }
      }
   }
}