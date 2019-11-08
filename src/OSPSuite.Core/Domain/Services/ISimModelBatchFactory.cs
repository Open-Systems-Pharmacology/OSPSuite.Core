using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimModelBatchFactory
   {
      ISimModelBatch Create();
   }

   class SimModelBatchFactory : DynamicFactory<ISimModelBatch>, ISimModelBatchFactory
   {
   }
}