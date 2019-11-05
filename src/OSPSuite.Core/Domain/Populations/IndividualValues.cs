using System.Collections.Generic;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Populations
{
   public class IndividualValues
   {
      private readonly Cache<string, ParameterValue> _parameterValues = new Cache<string, ParameterValue>(x => x.ParameterPath);
      public Covariates Covariates { get; set; }

      public IndividualValues()
      {
         Covariates = new Covariates();
      }

      public virtual IReadOnlyCollection<ParameterValue> ParameterValues => _parameterValues;

      public virtual void AddParameterValue(ParameterValue parameterValue) => _parameterValues.Add(parameterValue);

      public virtual ParameterValue ParameterValue(string parameterPath) => _parameterValues[parameterPath];
   }
}