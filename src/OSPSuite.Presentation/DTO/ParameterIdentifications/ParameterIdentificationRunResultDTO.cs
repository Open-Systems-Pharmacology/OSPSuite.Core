using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class ParameterIdentificationRunResultDTO : RunResultDTO
   {
      public List<OptimizedParameterDTO> OptimizedParameters { get; } = new List<OptimizedParameterDTO>();

      public int Index => RunResult.Index;
      public string Description => RunResult.Description;
      public double TotalError => RunResult.TotalError;
      public int NumberOfEvaluations => RunResult.NumberOfEvaluations;
      public TimeSpan Duration => RunResult.Duration;
      public string Message => RunResult.Message;
      public override RunStatus Status => RunResult.Status;

      public ParameterIdentificationRunResult RunResult { get; }
      public bool ValueIsCloseToBoundary => OptimizedParameters.Any(x => x.ValueIsCloseToBoundary);
      public ApplicationIcon BoundaryCheckIcon => ValueIsCloseToBoundary ? ApplicationIcons.Warning : ApplicationIcons.OK;
      public Image LegendImage { get; set; }

      public ParameterIdentificationRunResultDTO(ParameterIdentificationRunResult runResult)
      {
         RunResult = runResult;
      }

      public void AddOptimizedParameter(OptimizedParameterDTO optimizedParameterDTO)
      {
         OptimizedParameters.Add(optimizedParameterDTO);
      }
   }
}