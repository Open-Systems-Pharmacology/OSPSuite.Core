using System.Collections.Generic;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationSingleRunAnalysisPresenter : IParameterIdentificationAnalysisPresenter
   {
      ParameterIdentificationRunResult SelectedRunResults { get; set; }
      IReadOnlyList<ParameterIdentificationRunResult> AllRunResults { get; }
   }
}