using System;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class ParameterIdentificationRunResult : IWithDescription
   {
      public virtual string Description { get; set; }
      public virtual int Index { get; set; }
      public virtual OptimizationRunResult BestResult { get; set; } = new OptimizationRunResult();
      public OptimizationRunProperties Properties { get; set; } = new OptimizationRunProperties(0);

      public virtual double TotalError => BestResult.TotalError;
      public virtual int NumberOfEvaluations => Properties.NumberOfEvaluations;
      public virtual RunStatus Status { get; set; }
      public virtual string Message { get; set; }
      public virtual JacobianMatrix JacobianMatrix { get; set; }
      public virtual TimeSpan Duration { get; set; }

      public ParameterIdentificationRunResult()
      {
         Status = RunStatus.Created;
         Message = string.Empty;
      }

      public string SingleLineDescription
      {
         get
         {
            if (string.IsNullOrEmpty(Description))
               return Index.ToString();

            return $"{Index} - {Description.Replace(Environment.NewLine, " / ")}";
         }
      }
   }
}