using System.Threading.Tasks;

namespace OSPSuite.CLI.Core.Services
{
   public interface IBatchRunner<TBatchOptions>
   {
      Task RunBatchAsync(TBatchOptions runOptions);
   }
}