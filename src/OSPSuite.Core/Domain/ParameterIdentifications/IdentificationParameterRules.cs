using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   internal static class IdentificationParameterRules
   {
      private static readonly NumericFormatter<double> _numericFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      public static IEnumerable<IBusinessRule> All
      {
         get
         {
            yield return nameNotEmpty;
            yield return minLessThanMax;
            yield return maxGreaterThanMin;
            yield return startValueBetweenMinAndMax;
            yield return minStrictBiggerThanZeroForLogScaling;
            yield return nameUnique;
            yield return minimumConsistentWithLinkedParameters;
            yield return maximumConsistentWithLinkedParameters;
         }
      }

      private static IBusinessRule nameNotEmpty { get; } = GenericRules.NonEmptyRule<IdentificationParameter>(x => x.Name);

      private static IBusinessRule minimumConsistentWithLinkedParameters { get; } = CreateRule.For<IdentificationParameter>()
         .Property(item => item.MinValue)
         .WithRule((parameter, value) => isValidExtreme(parameter, value, isMinValueConsistent))
         .WithError((param, value) => messagesForInconsistentExtremes(allInconsistentExtremesFor(param, value, isMinValueConsistent),
            Rules.Parameters.MinimumMustBeGreaterThanOrEqualTo, Rules.Parameters.MinimumMustBeGreaterThan,
            x => x.MinIsAllowed, x => x.MinValue));

      private static IBusinessRule maximumConsistentWithLinkedParameters { get; } = CreateRule.For<IdentificationParameter>()
         .Property(item => item.MaxValue)
         .WithRule((parameter, value) => isValidExtreme(parameter, value, isMaxValueConsistent))
         .WithError((param, value) => messagesForInconsistentExtremes(allInconsistentExtremesFor(param, value, isMaxValueConsistent),
            Rules.Parameters.MaximumMustBeLessThanOrEqualTo, Rules.Parameters.MaximumMustBeLessThan,
            x => x.MaxIsAllowed, parameter => parameter.MaxValue));

      private static bool isMaxValueConsistent(IParameter parameter, double newMaximum, bool useAsFactor)
      {
         if (parameter?.MaxValue == null)
            return true;

         if (useAsFactor)
            newMaximum = newMaximum * parameter.Value;

         if (parameter.MaxIsAllowed)
            return newMaximum <= parameter.MaxValue.Value;

         return newMaximum < parameter.MaxValue.Value;
      }

      private static bool isMinValueConsistent(IParameter parameter, double newMinimum, bool useAsFactor)
      {
         if (parameter?.MinValue == null)
            return true;

         if (useAsFactor)
            newMinimum = newMinimum * parameter.Value;

         if (parameter.MinIsAllowed)
            return newMinimum >= parameter.MinValue.Value;

         return newMinimum > parameter.MinValue.Value;
      }

      private static bool isValidExtreme(IdentificationParameter identificationParameter, double value, Func<IParameter, double, bool, bool> consistencyChecker)
      {
         return !allInconsistentExtremesFor(identificationParameter, value, consistencyChecker).Any();
      }

      private static IEnumerable<ParameterSelection> allInconsistentExtremesFor(IdentificationParameter identificationParameter, double value, Func<IParameter, double, bool, bool> consistencyChecker)
      {
         return identificationParameter.AllLinkedParameters.Where(x => x.IsValid && !consistencyChecker(x.Parameter, value, identificationParameter.UseAsFactor));
      }

      private static string messagesForInconsistentExtremes(
         IEnumerable<ParameterSelection> allInconsistentExtremes,
         Func<string, string, string, string> extremeAllowedFunc,
         Func<string, string, string, string> extremeNotAllowedFunc,
         Func<IParameter, bool> extremeIsAllowed,
         Func<IParameter, double?> valueFunc)
      {
         return allInconsistentExtremes.Select(x =>
         {
            var parameter = x.Parameter;
            var value = valueFunc(parameter);
            if (!value.HasValue)
               return string.Empty;

            var errorRetrieverFunc = extremeIsAllowed(parameter) ? extremeAllowedFunc : extremeNotAllowedFunc;
            var formattedDisplayValue = _numericFormatter.Format(parameter.ConvertToDisplayUnit(value.Value));
            return errorRetrieverFunc(formattedDisplayValue, parameter.DisplayUnit?.Name,  x.FullQuantityPath);
         }).Where(x => !string.IsNullOrEmpty(x)).ToString(Environment.NewLine);
      }

      private static IBusinessRule minLessThanMax { get; } = CreateRule.For<IdentificationParameter>()
         .Property(item => item.MinValue)
         .WithRule((x, value) => value < x.MaxValue)
         .WithError((param, value) => Rules.Parameters.MinLessThanMax);

      private static IBusinessRule maxGreaterThanMin { get; } = CreateRule.For<IdentificationParameter>()
         .Property(item => item.MaxValue)
         .WithRule((x, value) => x.MinValue < value)
         .WithError((param, value) => Rules.Parameters.MaxGreaterThanMin);

      private static IBusinessRule startValueBetweenMinAndMax { get; } = CreateRule.For<IdentificationParameter>()
         .Property(item => item.StartValue)
         .WithRule((x, value) => x.MinValue <= value && x.MaxValue >= value)
         .WithError((param, value) => Rules.Parameters.ValueShouldBeBetweenMinAndMax);

      private static IBusinessRule minStrictBiggerThanZeroForLogScaling { get; } = CreateRule.For<IdentificationParameter>()
         .Property(item => item.MinValue)
         .WithRule((x, value) => validateScaling(value, x.Scaling))
         .WithError((param, value) => Rules.Parameters.MinShouldBeStrictlyGreaterThanZeroForLogScale);

      private static bool validateScaling(double minValue, Scalings scaling) => scaling != Scalings.Log || minValue > 0;

      private static IBusinessRule nameUnique { get; } = CreateRule.For<IdentificationParameter>()
         .Property(x => x.Name)
         .WithRule((currentIdentificationParameter, name) =>
         {
            var parameterIdentification = currentIdentificationParameter.ParameterIdentification;

            var otherIdentificationParameter = parameterIdentification?.IdentificationParameterByName(name);
            if (otherIdentificationParameter == null)
               return true;

            return ReferenceEquals(otherIdentificationParameter, currentIdentificationParameter);
         })
         .WithError((field, name) => Error.NameAlreadyExistsInContainerType(name, ObjectTypes.IdentificationParameter));
   }
}