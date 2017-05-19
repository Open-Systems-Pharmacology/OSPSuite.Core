using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Starter.Tasks;

namespace OSPSuite.Starter.Services
{
   public class TransferOptimizedParametersToSimulationsTask : ITransferOptimizedParametersToSimulationsTask
   {
      public ICommand TransferParametersFrom(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         
         return new EmptyCommand<OSPSuiteExecutionContext>();
      }
   }
}