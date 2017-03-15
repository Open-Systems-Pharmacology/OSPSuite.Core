namespace OSPSuite.Core.Maths.Optimization.NelderMead
{
    public sealed class RegressionResult
    {
       public RegressionResult(TerminationReason terminationReason, double[] constants, double errorValue, int evaluationCount)
        {
            TerminationReason = terminationReason;
            Constants = constants;
            ErrorValue = errorValue;
            EvaluationCount = evaluationCount;
        }

        public TerminationReason TerminationReason { get; }

       public double[] Constants { get; }

       public double ErrorValue { get; }

       public int EvaluationCount { get; }
    }

    public enum TerminationReason
    {
        MaxFunctionEvaluations,
        Converged,
        Unspecified
    }
}