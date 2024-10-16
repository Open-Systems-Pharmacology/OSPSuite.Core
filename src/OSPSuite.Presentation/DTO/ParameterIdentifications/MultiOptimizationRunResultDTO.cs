﻿namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class MultiOptimizationRunResultDTO : RunResultDTO
   {
      public string Message { get; set; }
      public double BestError { get; set; }
      public int Index { get; set; }
      public string Description { get; set; }
      public double CurrentError { get; set; }
   }
}