namespace OSPSuite.Core.Maths.Optimization.NelderMead
{
    public sealed class SimplexConstant
    {
        public SimplexConstant(double value, double initialPerturbation)
        {
            Value = value;
            InitialPerturbation = initialPerturbation;
        }

        /// <summary>
        /// The value of the constant
        /// </summary>
        public double Value { get; set; }

        // The size of the initial perturbation
        public double InitialPerturbation { get; set; }
    }
}