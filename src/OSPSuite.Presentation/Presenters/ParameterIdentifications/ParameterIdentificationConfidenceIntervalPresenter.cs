using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface IParameterIdentificationConfidenceIntervalPresenter : IPresenter<IParameterIdentificationConfidenceIntervalView>
   {
      void CalculateConfidenceIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult);
   }

   public class ParameterIdentificationConfidenceIntervalPresenter : AbstractPresenter<IParameterIdentificationConfidenceIntervalView, IParameterIdentificationConfidenceIntervalPresenter>, IParameterIdentificationConfidenceIntervalPresenter
   {
      private readonly IConfidenceIntervalCalculator _confidenceIntervalCalculator;

      public ParameterIdentificationConfidenceIntervalPresenter(IParameterIdentificationConfidenceIntervalView view, IConfidenceIntervalCalculator confidenceIntervalCalculator) : base(view)
      {
         _confidenceIntervalCalculator = confidenceIntervalCalculator;
      }

      public void CalculateConfidenceIntervalFor(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         if (runResult.JacobianMatrix == null)
         {
            _view.DeleteBinding();
            return;
         }

         var confidenceInterval = _confidenceIntervalCalculator.ConfidenceIntervalFrom(runResult.JacobianMatrix, runResult.BestResult.ResidualsResult);
         var confidenceIntervalDTOs = mapFrom(confidenceInterval, parameterIdentification, runResult).ToList();

         _view.BindTo(confidenceIntervalDTOs);
      }

      private IEnumerable<ParameterConfidenceIntervalDTO> mapFrom(ICache<string, double> confidenceInterval, ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         if (!confidenceInterval.Any())
            return Enumerable.Empty<ParameterConfidenceIntervalDTO>();

         return from optimizedParameter in runResult.BestResult.Values
                     let identificationParameter = parameterIdentification.IdentificationParameterByName(optimizedParameter.Name)
                     where identificationParameter != null
                     select confidenceIntervalDTOFrom(confidenceInterval, identificationParameter, optimizedParameter);
      }

      private ParameterConfidenceIntervalDTO confidenceIntervalDTOFrom(ICache<string, double> confidenceInterval, IdentificationParameter identificationParameter, OptimizedParameterValue optimizedParameter)
      {
         var value = optimizedParameter.Value;
         var displayUnit = identificationParameter.DisplayUnit;
         var valueInDisplayUnit = identificationParameter.Dimension.BaseUnitValueToUnitValue(displayUnit, value);
         var confidenceIntervalInDisplayUnit = identificationParameter.Dimension.BaseUnitValueToUnitValue(displayUnit, confidenceInterval[identificationParameter.Name]);

         return new ParameterConfidenceIntervalDTO
         {
            Name = identificationParameter.Name,
            Value = valueInDisplayUnit,
            ConfidenceInterval = confidenceIntervalInDisplayUnit,
            Unit = displayUnit
         };
      }
   }
}