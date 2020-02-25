using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Domain
{
   public class SensitivityAnalysis
   {
      public ISimulation Simulation { get; }

      public int NumberOfSteps { get; set; } = Constants.DEFAULT_SENSITIVITY_NUMBER_OF_STEPS;

      public double VariationRange { get; set; } = Constants.DEFAULT_SENSITIVITY_VARIATION_RANGE;

      private readonly HashSet<string> _parameterPaths = new HashSet<string>();
      private readonly List<DynamicPKParameter> _dynamicPKParameters = new List<DynamicPKParameter>();

      public SensitivityAnalysis(ISimulation simulation)
      {
         Simulation = simulation;
      }

      public void AddParameter(IParameter parameter) => AddParameterPath(parameter.ConsolidatedPath());

      public void AddParameters(IEnumerable<IParameter> parameters) => parameters.Each(AddParameter);

      public void AddParameterPath(string parameterPath) => _parameterPaths.Add(parameterPath);

      public void AddParameterPaths(IEnumerable<string> parameterPaths) => parameterPaths.Each(AddParameterPath);

      public string[] ParameterPaths => _parameterPaths.ToArray();

      public void AddDynamicPKParameters(IEnumerable<DynamicPKParameter> dynamicPKParameters) => dynamicPKParameters.Each(AddDynamicPKParameter);

      public void AddDynamicPKParameter(DynamicPKParameter dynamicPKParameter) => _dynamicPKParameters.Add(dynamicPKParameter);

      public IReadOnlyList<DynamicPKParameter> AllDynamicParameters => _dynamicPKParameters;
   }
}