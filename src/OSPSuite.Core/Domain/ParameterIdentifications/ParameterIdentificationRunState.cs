using System.Collections.Generic;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class ParameterIdentificationRunState
   {
      public virtual ParameterIdentificationRunResult RunResult { get; }
      public virtual OptimizationRunResult CurrentResult { get; }
      public virtual IReadOnlyList<float> ErrorHistory { get; }
      public virtual IReadOnlyCollection<IdentificationParameterHistory> ParametersHistory { get;  }
      public virtual OptimizationRunResult BestResult => RunResult.BestResult;
      public virtual RunStatus Status => RunResult.Status;
      public virtual string Message => RunResult.Message;

      public ParameterIdentificationRunState(ParameterIdentificationRunResult runResult, OptimizationRunResult currentResult, IReadOnlyList<float> errorHistory, IReadOnlyCollection<IdentificationParameterHistory> parametersHistory)
      {
         RunResult = runResult;
         ErrorHistory = errorHistory;
         ParametersHistory = parametersHistory;
         CurrentResult = currentResult;
      }
   }
}