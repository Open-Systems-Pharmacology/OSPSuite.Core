using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IIdentificationParameterFactory
   {
      IdentificationParameter CreateFor(ParameterSelection parameterSelection, ParameterIdentification parameterIdentification);
      IdentificationParameter CreateFor(IEnumerable<ParameterSelection> parameterSelections, ParameterIdentification parameterIdentification);
   }

   public class IdentificationParameterFactory : IIdentificationParameterFactory
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IContainerTask _containerTask;
      private readonly IIdentificationParameterTask _identificationParameterTask;

      public IdentificationParameterFactory(IObjectBaseFactory objectBaseFactory, IContainerTask containerTask, IIdentificationParameterTask identificationParameterTask)
      {
         _objectBaseFactory = objectBaseFactory;
         _containerTask = containerTask;
         _identificationParameterTask = identificationParameterTask;
      }

      public IdentificationParameter CreateFor(ParameterSelection parameterSelection, ParameterIdentification parameterIdentification)
      {
         return CreateFor(new[] {parameterSelection}, parameterIdentification);
      }

      public IdentificationParameter CreateFor(IEnumerable<ParameterSelection> parameterSelections, ParameterIdentification parameterIdentification)
      {
         var unmappedParameters = parameterSelections.Where(s => !parameterIdentification.IdentifiesParameter(s)).ToList();
         var parameters = unmappedParameters.Select(x => x.Parameter).ToList();
         if (!parameters.Any())
            return null;

         ensureThatDimensionsAreEqual(parameters);

         var nameToUse = uniqueNameFor(parameterIdentification, parameters);

         var identificationParameter = _objectBaseFactory.Create<IdentificationParameter>().WithName(nameToUse);
         identificationParameter.UseAsFactor = false;

         _identificationParameterTask.AddParameterRangeBasedOn(identificationParameter, parameters[0]);
         identificationParameter.Scaling = defaultScalingBasedOn(parameters[0]);

         unmappedParameters.Each(identificationParameter.AddLinkedParameter);

         return identificationParameter;
      }

      private Scalings defaultScalingBasedOn(IParameter parameter)
      {
         var defaultScalingBasedOnDimension = parameter.Dimension.Name.IsOneOf(Constants.Dimension.LOG_UNITS, Constants.Dimension.FRACTION) ? Scalings.Linear : Scalings.Log;
         if (!parameter.MinValue.HasValue)
            return defaultScalingBasedOnDimension;

         if (parameter.MinValue <= 0 && defaultScalingBasedOnDimension == Scalings.Log)
            return Scalings.Linear;

         return defaultScalingBasedOnDimension;
      }

      private string uniqueNameFor(ParameterIdentification parameterIdentification, IReadOnlyList<IParameter> parameters)
      {
         var name = parameters.AllDistinctValues(x => x.Name).First();
         return _containerTask.CreateUniqueName(parameterIdentification.AllIdentificationParameters, name, canUseBaseName: true);
      }

      private void ensureThatDimensionsAreEqual(IEnumerable<IParameter> parameters)
      {
         var allDimensions = parameters.Select(x => x.Dimension).Distinct().ToList();
         if (allDimensions.Count != 1)
            throw new DimensionMismatchException(allDimensions);
      }
   }
}