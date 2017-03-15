using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class StandardParameterIdentificationRunInitializer : ParameterIdentifcationRunInitializer
   {
      public StandardParameterIdentificationRunInitializer(ICloneManagerForModel cloneManager, IParameterIdentificationRun parameterIdentificationRun) : base(cloneManager,parameterIdentificationRun)
      {
      }

      public override Task<ParameterIdentification> InitializeRun()
      {
         return Task.FromResult(_cloneManager.Clone(ParameterIdentification));
      }
   }
}