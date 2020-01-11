using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Populations
{
   public class IndividualValues
   {
      public int? IndividualId { get; set; }

      private readonly Cache<string, ParameterValue> _parameterValues = new Cache<string, ParameterValue>(x => x.ParameterPath);

      public ICache<string, string> Covariates { get; } = new Cache<string, string>();

      public virtual IReadOnlyCollection<ParameterValue> ParameterValues => _parameterValues;

      public virtual void AddParameterValue(ParameterValue parameterValue) => _parameterValues.Add(parameterValue);

      public virtual void AddCovariate(string covariateName, string covariateValue) => Covariates.Add(covariateName, covariateValue);

      public virtual ParameterValue ParameterValue(string parameterPath) => _parameterValues[parameterPath];

      public virtual string CovariateValue(string covariateName) => Covariates[covariateName];
   }
}