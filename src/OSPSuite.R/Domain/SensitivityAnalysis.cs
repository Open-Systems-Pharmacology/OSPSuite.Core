using System.Collections.Generic;
using OSPSuite.Core.Domain;

namespace OSPSuite.R.Domain
{
   public class SensitivityAnalysis
   {
      public Simulation Simulation { get; }

      public int NumberOfSteps { get; set; } = Constants.DEFAULT_SENSITIVITY_NUMBER_OF_STEPS;

      public double VariationRange { get; set; } = Constants.DEFAULT_SENSITIVITY_VARIATION_RANGE;

      public readonly List<IParameter> _parameters = new List<IParameter>();

      public SensitivityAnalysis(Simulation simulation)
      {
         Simulation = simulation;
      }

      public void AddParameter(IParameter parameter) => AddParameters(new[] {parameter});

      public void AddParameters(IEnumerable<IParameter> parameters) => _parameters.AddRange(parameters);

      public IParameter[] Parameters => _parameters.ToArray();
   }
}