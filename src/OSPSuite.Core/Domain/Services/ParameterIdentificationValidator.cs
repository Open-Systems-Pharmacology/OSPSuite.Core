using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IParameterIdentificationValidator
   {
      ValidationResult Validate(ParameterIdentification parameterIdentification);
   }

   public class ParameterIdentificationValidator : IParameterIdentificationValidator
   {
      private readonly IFullPathDisplayResolver _fullPathDisplayResolver;

      public ParameterIdentificationValidator(IFullPathDisplayResolver fullPathDisplayResolver)
      {
         _fullPathDisplayResolver = fullPathDisplayResolver;
      }

      public ValidationResult Validate(ParameterIdentification parameterIdentification)
      {
         var validationResult = new ValidationResult();
         if (!parameterIdentification.AllOutputMappings.Any())
            validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.NoOutputMappingDefined);

         if (parameterIdentification.AllOutputMappings.Any(x => !x.IsValid))
            validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.OutputMappingIsInvalid);

         if (parameterIdentification.AllOutputMappings.Any(x => !x.DimensionsAreConsistent()))
            validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.OutputMappingHasInconsistentDimension);

         if (!parameterIdentification.AllIdentificationParameters.Any())
            validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.NoIdentificationParameterDefined);

         allIdentificaitonParametersWithUndefinedLinkedParametersIn(parameterIdentification).Each(p => { validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.LinkedParameterIsNotValidInIdentificationParameter(p.Name)); });

         if (parameterIdentification.Configuration.AlgorithmProperties == null)
            validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.NoOptimizationAlgorithmSelected);

         validateWeights(parameterIdentification, validationResult);

         parameterIdentification.OutputMappings.All.DistinctBy(mapping => mapping.Output).Each(mapping => validateScalingForSharedOutputMappings(mapping, parameterIdentification, validationResult));

         return validationResult;
      }

      private static IEnumerable<IdentificationParameter> allIdentificaitonParametersWithUndefinedLinkedParametersIn(ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.AllIdentificationParameters.Where(x => x.AllLinkedParameters.Any(linkedParameter => !linkedParameter.IsValid));
      }

      private void validateWeights(ParameterIdentification parameterIdentification, ValidationResult validationResult)
      {
         var allDataRepositoryWeights = allWeightedDataRepositoryWeights(parameterIdentification);
         var allPointWeights = allDataPointWeights(parameterIdentification);

         if (allDataRepositoryWeights.Union(allPointWeights).Any(weight => weight < 0))
         {
            validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.WeightValueCannotBeNegative);
         }
      }

      private static IEnumerable<float> allDataPointWeights(ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.OutputMappings.All.Where(x => x.WeightedObservedData != null).SelectMany(x => x.WeightedObservedData.Weights);
      }

      private static IEnumerable<float> allWeightedDataRepositoryWeights(ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.OutputMappings.All.Select(x => x.Weight);
      }

      private void validateScalingForSharedOutputMappings(OutputMapping mapping, ParameterIdentification parameterIdentification, ValidationResult validationResult)
      {
         var allOutputScalings = scalingsMappedForOutput(mapping, parameterIdentification);

         if (allOutputScalings.Count() == 1) return;

         var displayPathAsString = _fullPathDisplayResolver.FullPathFor(parameterIdentification, addSimulationName: true);
         validationResult.AddMessage(NotificationType.Error, parameterIdentification, Error.OutputsDoNotAllHaveTheSameScaling(displayPathAsString));
      }

      private static IEnumerable<Scalings> scalingsMappedForOutput(OutputMapping mapping, ParameterIdentification parameterIdentification)
      {
         return parameterIdentification.OutputMappings.All.Where(outputMapping => outputMapping.Output == mapping.Output).Select(x => x.Scaling).Distinct();
      }
   }
}