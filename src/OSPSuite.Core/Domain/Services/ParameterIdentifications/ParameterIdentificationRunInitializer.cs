using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationRunInitializer
   {
      Task<ParameterIdentification> InitializeRun(CancellationToken cancellationToken);
      /// <summary>
      /// 0-based run index
      /// </summary>
      int RunIndex { get;  }

      IParameterIdentificationRun Run { get;  }
   }

   public abstract class ParameterIdentificationRunInitializer : IParameterIdentificationRunInitializer
   {
      protected readonly ICloneManagerForModel _cloneManager;
      public int RunIndex { get; private set; }
      public IParameterIdentificationRun Run { get; }

      protected ParameterIdentification ParameterIdentification { get; private set; }

      protected ParameterIdentificationRunInitializer(ICloneManagerForModel cloneManager, IParameterIdentificationRun parameterIdentificationRun)
      {
         _cloneManager = cloneManager;
         Run = parameterIdentificationRun;
         Run.InitializeWith(this);
      }

      public abstract Task<ParameterIdentification> InitializeRun(CancellationToken cancellationToken);

      public void Initialize(ParameterIdentification parameterIdentification, int runIndex)
      {
         ParameterIdentification = parameterIdentification;
         RunIndex = runIndex;
         Run.RunResult.Index = runIndex + 1;
      }
   }
}