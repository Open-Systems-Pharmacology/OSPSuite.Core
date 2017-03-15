using OSPSuite.Core.Domain.PKAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisPKAnalysesTask : IPKAnalysesTask
   {
   }

   public class SensitivityAnalysisPKAnalysesTask : PKAnalysesTask, ISensitivityAnalysisPKAnalysesTask
   {
      public SensitivityAnalysisPKAnalysesTask(ILazyLoadTask lazyLoadTask, IPKValuesCalculator pkValuesCalculator, IPKParameterRepository pkParameterRepository, IPKCalculationOptionsFactory pkCalculationOptionsFactory) : base(lazyLoadTask, pkValuesCalculator, pkParameterRepository, pkCalculationOptionsFactory)
      {
      }

      public override bool PKParameterCanBeUsed(PKParameter pkParameter, PKCalculationOptions pkCalculationOptions)
      {
         return base.PKParameterCanBeUsed(pkParameter, pkCalculationOptions) && !isNormParameter(pkParameter);
      }

      private bool isNormParameter(PKParameter pkParameter)
      {
         return pkParameter.Name.EndsWith(Constants.PKParameters.NormSuffix);
      }
   }
}