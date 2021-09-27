using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class StandardParameterIdentificationRunInitializer : ParameterIdentificationRunInitializer
   {
      public StandardParameterIdentificationRunInitializer(ICloneManagerForModel cloneManager, IParameterIdentificationRun parameterIdentificationRun) : base(cloneManager,parameterIdentificationRun)
      {
      }

      public override Task<ParameterIdentification> InitializeRun(CancellationToken cancellationToken)
      {
         return Task.FromResult(_cloneManager.Clone(ParameterIdentification));
      }
   }
}