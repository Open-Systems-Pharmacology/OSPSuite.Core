using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivyAnalysisVariationDataCreator
   {
      VariationData CreateForRun(SensitivityAnalysis sensitivityAnalysis);
   }

   public class SensitivyAnalysisVariationDataCreator : ISensitivyAnalysisVariationDataCreator
   {
      public VariationData CreateForRun(SensitivityAnalysis sensitivityAnalysis)
      {
         var variationData =  new VariationData
         {
            Name = sensitivityAnalysis.Name,
            ParameterPaths = sensitivityAnalysis.AllSensitivityParameters.Select(x => x.ParameterSelection.Path).ToList(),
            DefaultValues = sensitivityAnalysis.AllSensitivityParameters.Select(x => x.Parameter.Value).ToList(),
         };

         sensitivityAnalysis.AllSensitivityParameters.Each(parameter =>
         {
            variationData.AddVariationValues(parameter.Name, sensitivityAnalysis.AllParameterVariationsFor(parameter).ToList());
         });

         return variationData;
      }
   }
}